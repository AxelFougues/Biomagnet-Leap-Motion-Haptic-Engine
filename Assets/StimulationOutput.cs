using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StimulationOutput : MonoBehaviour {

    const int COLLISION_SMOOTHING_BUFFER = 3;


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
    [Header("Equalization")]
    [Space]
    public bool doEqualization = true;
    public AnimationCurve equalization;

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
    Dictionary<int, RollingAverageFilter> triggerFilters = new Dictionary<int, RollingAverageFilter>();

    //Gets set by UI when matched
    public void setUI(OutputUI outputUI) {
        this.outputUI = outputUI;
    }

    public void setStereoPan(int pan) {
        signalGenerator.stereoPan = pan;
    }

    //Contact

    private void OnTriggerEnter(Collider other) {
        int id = other.GetInstanceID();

        if (activeContacts.ContainsKey(id)) {
            activeContacts[id].collidesDuringFrame = true;
            return;
        }
        Touchable t = other.GetComponent<Touchable>();
        if (t != null) {
            ContactParameters cp = new ContactParameters(surface.transform.position, t, this);
            cp.collidesDuringFrame = true;
            activeContacts.Add(id, cp);
            t.onContactStart(this);
            triggerFilters.Add(id, new RollingAverageFilter(COLLISION_SMOOTHING_BUFFER));
        }
        
    }

    private void FixedUpdate() {
        List<int> lost = new List<int>();

        foreach (int id in activeContacts.Keys) {
            ContactParameters cp = activeContacts[id];
            if (Mathf.RoundToInt(triggerFilters[id].getValue(cp.collidesDuringFrame ? 1 : 0)) == 0) {
                lost.Add(id);
            } else {


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

                SignalData sd = cp.touchable.onContact(cp);

                //Equalization
                if (doEqualization) {
                    if (sd.sineAmplitude > 0) Mathf.Clamp01(sd.sineAmplitude *= equalization.Evaluate(sd.sineFrequency));
                    if (sd.sawAmplitude > 0) Mathf.Clamp01(sd.sawAmplitude *= equalization.Evaluate(sd.sawFrequency));
                    if (sd.squareAmplitude > 0) Mathf.Clamp01(sd.squareAmplitude *= equalization.Evaluate(sd.squareFrequency));
                }
                signalGenerator.loadPreset(sd);

                //UI updates
                if (outputUI != null) outputUI.text.text = Mathf.RoundToInt(signalGenerator.signal.sineAmplitude * 100f) + "%";

            }
        }

        foreach (int id in lost) {
            activeContacts[id].touchable.onContactEnd(this);
            activeContacts.Remove(id);
            triggerFilters.Remove(id);
        }
        if(lost.Count > 0) exitCleanup();
    }


    private void OnTriggerExit(Collider other) {
        int id = other.GetInstanceID();
        if (activeContacts.ContainsKey(id)) {

            activeContacts[id].collidesDuringFrame = false;
            //activeContacts[id].touchable.onContactEnd();
            //activeContacts.Remove(id);
            //exitCleanup();
        }
        
    }

    private void OnDisable() {
        foreach (KeyValuePair<int, ContactParameters> cp in activeContacts) {
            cp.Value.touchable.onContactEnd(this);
        }
        activeContacts.Clear();
        exitCleanup();
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

        public bool collidesDuringFrame = true;
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


