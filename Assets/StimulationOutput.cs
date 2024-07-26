using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StimulationOutput : MonoBehaviour {

    [Header("Output")]
    [Space]

    public int outputChannelID = 0;
    public bool simulateMotion = true;

    [Space]
    [Header("Physics")]
    [Space]

    [Tooltip("Velocity in mm/s")]
    public float minPerpendicularVelocity = 1f;

    [Space]
    [Header("References")]
    [Space]

    public SignalGenerator signalGenerator;
    public AudioSource audioSource;
    public Collider surface;
    [HideInInspector]
    public OutputUI outputUI;
    
    //Current points of contact with touchable objects
    Dictionary<int, Coroutine> engagedContactRoutines = new Dictionary<int, Coroutine>();

    //Gets set by UI when matched
    public void setUI(OutputUI outputUI) {
        this.outputUI = outputUI;
    }


    //Contact

    private void OnTriggerStay(Collider other) {
        int id = other.GetInstanceID();

        if (engagedContactRoutines.ContainsKey(id)) return;

        Touchable t = other.GetComponent<Touchable>();
        if (t != null) {
            engagedContactRoutines.Add(id, StartCoroutine(engagedContactRoutine(t)));
        }

        Movable m = other.GetComponent<Movable>();
        if (m != null) {
            engagedContactRoutines.Add(id, StartCoroutine(engagedContactRoutine(m)));
        }
    }


    private void OnTriggerExit(Collider other) {
        int id = other.GetInstanceID();
        if (engagedContactRoutines.ContainsKey(id)) {
            StopCoroutine(engagedContactRoutines[id]);
            engagedContactRoutines.Remove(id);
        }
    }

    IEnumerator engagedContactRoutine(Touchable touchable) {

        ContactParameters cp = new ContactParameters(surface.transform.position);
        Vector3 direction; float distance; Vector3 velocity;
        while (true) {
            //interaction calculation
            if (Physics.ComputePenetration(touchable.collider, touchable.transform.position, touchable.transform.rotation, surface, surface.transform.position, surface.transform.rotation, out direction, out distance)) {

                //outputs
                cp.penetrationDirection = direction.normalized;
                cp.penetrationDistance = distance * 1000f;  // in mm

                //save previous
                cp.previousPenetrationDistance = cp.penetrationDistance;
                cp.previousPenetrationDirection = cp.penetrationDirection;
                cp.previousPenetrationVelocity = cp.penetrationVelocity;
                cp.previousPerpendicularVelocity = cp.perpendicularVelocity;
                cp.previousPosition = cp.currentPosition;

                //calculate new
                cp.currentPosition = cp.positionFilter.getValue(surface.transform.position * 1000); // in mm
                velocity = (cp.currentPosition - cp.previousPosition) / Time.fixedDeltaTime; //in mm/s
                cp.penetrationVelocity = Vector3.Dot(velocity, cp.penetrationDirection); //in mm/s
                cp.perpendicularVelocity = cp.velocityFilter.getValue((velocity.magnitude - (cp.penetrationVelocity * cp.penetrationDirection).magnitude)); // in mm/s
                
            }



            //signal generation
            SignalData si = touchable.materialSettings.getSignal(cp);
            signalGenerator.loadPreset(si);


            //  modulation
            /*
            if (simulateMotion && cp.perpendicularVelocity > minPerpendicularVelocity) {
                signalGenerator.signal.ampModFrequency = cp.modulationFilter.getValue(touchable.materialSettings.getTextureFrequency(cp.perpendicularVelocity));
            } else {
                signalGenerator.signal.ampModFrequency = 0;
            }*/


            //UI updates
            if(outputUI != null) outputUI.text.text = Mathf.RoundToInt(signalGenerator.signal.sineAmplitude * 100f)+"%";
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator engagedContactRoutine(Movable movable) {

        ContactParameters cp = new ContactParameters(surface.transform.position);
        Vector3 direction; float distance; Vector3 velocity;
        while (true) {
            //interaction calculation
            if (Physics.ComputePenetration(movable.collider, movable.transform.position, movable.transform.rotation, surface, surface.transform.position, surface.transform.rotation, out direction, out distance)) {

                //outputs
                cp.penetrationDirection = direction.normalized;
                cp.penetrationDistance = distance * 1000f;  // in mm

                //save previous
                cp.previousPenetrationDistance = cp.penetrationDistance;
                cp.previousPenetrationDirection = cp.penetrationDirection;
                cp.previousPenetrationVelocity = cp.penetrationVelocity;
                cp.previousPerpendicularVelocity = cp.perpendicularVelocity;
                cp.previousPosition = cp.currentPosition;

                //calculate new
                cp.currentPosition = cp.positionFilter.getValue(surface.transform.position * 1000); // in mm
                velocity = (cp.currentPosition - cp.previousPosition) / Time.fixedDeltaTime; //in mm/s
                cp.penetrationVelocity = Vector3.Dot(velocity, cp.penetrationDirection); //in mm/s
                cp.perpendicularVelocity = cp.velocityFilter.getValue((velocity.magnitude - (cp.penetrationVelocity * cp.penetrationDirection).magnitude)); // in mm/s

            }

            //movable update
            movable.doMove(cp);

            //signal generation
            SignalData si = movable.materialSettings.getSignal(cp);
            signalGenerator.loadPreset(si);


            //  modulation
            /*
            if (simulateMotion && cp.perpendicularVelocity > minPerpendicularVelocity) {
                signalGenerator.signal.ampModFrequency = cp.modulationFilter.getValue(touchable.materialSettings.getTextureFrequency(cp.perpendicularVelocity));
            } else {
                signalGenerator.signal.ampModFrequency = 0;
            }*/


            //UI updates
            if (outputUI != null) outputUI.text.text = Mathf.RoundToInt(signalGenerator.signal.sineAmplitude * 100f) + "%";
            yield return new WaitForFixedUpdate();
        }
    }


    public class ContactParameters {

        public ContactParameters(Vector3 initialPosition) {
            currentPosition = previousPosition = initialPosition;
        }

        public RollingAverageFilter positionFilter = new RollingAverageFilter(5);
        public RollingAverageFilter velocityFilter = new RollingAverageFilter(1);
        public RollingAverageFilter modulationFilter = new RollingAverageFilter(50);
        //current
        public Vector3 currentPosition;
        public float penetrationDistance = 0; //in mm
        public Vector3 penetrationDirection = Vector3.zero;
        public float penetrationVelocity = 0; //in mm/s
        public float perpendicularVelocity = 0; //in mm/s
        //previous
        public float previousPenetrationDistance = 0; //in mm
        public Vector3 previousPenetrationDirection = Vector3.zero;
        public float previousPenetrationVelocity = 0; //in mm/s
        public float previousPerpendicularVelocity = 0; //in mm/s
        public Vector3 previousPosition;

    }

}


