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
    public SignalPreset noSignalPreset;
    public SignalGenerator signalGenerator;
    public AudioSource audioSource;
    public Collider surface;
    [HideInInspector]
    public OutputUI outputUI;
    
    //Current points of contact with touchable objects
    Dictionary<int, ContactParameters> activeContacts = new Dictionary<int, ContactParameters>();

    //Gets set by UI when matched
    public void setUI(OutputUI outputUI) {
        this.outputUI = outputUI;
    }


    //Contact

    private void OnTriggerEnter(Collider other) {
        int id = other.GetInstanceID();

        if (activeContacts.ContainsKey(id)) return;

        Touchable t = other.GetComponent<Touchable>();
        if (t != null) {
            ContactParameters cp = new ContactParameters(surface.transform.position, t, this);
            activeContacts.Add(id, cp);
            t.onContactStart();
        }
        
    }

    private void FixedUpdate() {
        foreach (ContactParameters cp in activeContacts.Values) {
            Vector3 direction; float distance; Vector3 velocity;

            //interaction calculation
            if (Physics.ComputePenetration(cp.touchable.touchCollider, cp.touchable.transform.position, cp.touchable.transform.rotation, surface, surface.transform.position, surface.transform.rotation, out direction, out distance)) {

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

            cp.touchable.onContact(cp);

            //UI updates
            if (outputUI != null) outputUI.text.text = Mathf.RoundToInt(signalGenerator.signal.sineAmplitude * 100f) + "%";

        }
    }
/*
    private void OnTriggerStay(Collider other) {
        int id = other.GetInstanceID();
        if (!activeContacts.ContainsKey(id)) return;
        ContactParameters cp = activeContacts[id];

        Vector3 direction; float distance; Vector3 velocity;

        //interaction calculation
        if (Physics.ComputePenetration(cp.touchable.touchCollider, cp.touchable.transform.position, cp.touchable.transform.rotation, surface, surface.transform.position, surface.transform.rotation, out direction, out distance)) {

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
        if (cp.touchable.objectSettings != null) cp.touchable.objectSettings.doMove(cp, cp.touchable);
        

        //signal generation
        SignalData si = cp.touchable.materialSettings.getSignal(cp);
        signalGenerator.loadPreset(si);

        //UI updates
        if (outputUI != null) outputUI.text.text = Mathf.RoundToInt(signalGenerator.signal.sineAmplitude * 100f) + "%";

        Debug.Log("OnTriggerStay");

    }

*/

    private void OnTriggerExit(Collider other) {
        int id = other.GetInstanceID();
        if (activeContacts.ContainsKey(id)) {
            activeContacts[id].touchable.onContactEnd();
            activeContacts.Remove(id);
            exitCleanup();
        }
        
    }

    void exitCleanup() {
        signalGenerator.loadPreset(noSignalPreset.data);
        if (outputUI != null) outputUI.text.text = "0%";
    }


    public class ContactParameters {

        public ContactParameters(Vector3 initialPosition, Touchable t, StimulationOutput so) {
            currentPosition = previousPosition = initialPosition;
            this.touchable = t;
            this.stimulationOutput = so;
        }

        //instances
        public Touchable touchable;
        public StimulationOutput stimulationOutput;
        //filters
        public RollingAverageFilter positionFilter = new RollingAverageFilter(5);
        public RollingAverageFilter velocityFilter = new RollingAverageFilter(1);
        public RollingAverageFilter modulationFilter = new RollingAverageFilter(50);
        //current
        public Vector3 outputDirection = Vector3.zero;
        public Vector3 currentPosition;
        public float penetrationDistance = 0; //in mm
        public Vector3 penetrationDirection = Vector3.zero;
        public float penetrationVelocity = 0; //in mm/s
        public float perpendicularVelocity = 0; //in mm/s
        //previous
        public Vector3 previousOutputDirection = Vector3.zero;
        public float previousPenetrationDistance = 0; //in mm
        public Vector3 previousPenetrationDirection = Vector3.zero;
        public float previousPenetrationVelocity = 0; //in mm/s
        public float previousPerpendicularVelocity = 0; //in mm/s
        public Vector3 previousPosition;

    }

}


