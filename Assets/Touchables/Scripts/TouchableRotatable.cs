using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StimulationOutput;

public class TouchableRotatable : Touchable{
    [Header("Rotatable Settings")]
    [Space]

    public float maxDegrees = 0;
    public AnimationCurve resistance;
    public float springBack = 0;

    [Space]
    [Header("References")]
    [Space]

    public Rigidbody rotatableRigidbody;
    public Transform rotatableObject;

    [Space]
    [Header("Debug")]
    [Space]

    public bool inPlace = true;

    [HideInInspector]
    public Quaternion initialRotation = Quaternion.identity;

    private void Start() {
        if (rotatableObject != null) initialRotation = rotatableObject.rotation;
    }

    private void Update() {
        if (inContact == 0) onNoContact();
    }

    public override SignalData onContact(ContactParameters cp) {

        if (rotatableObject == null) return base.onContact(cp);

        //displacement
        Vector3 displacement = cp.penetrationDirection * cp.penetrationDistance / 1000f;


        //resistance
        if (maxDegrees > 0 && resistance.keys.Length > 0) {
            float degrees = Quaternion.Angle(rotatableObject.rotation, initialRotation);
            if (degrees > maxDegrees) displacement = Vector3.zero;
            else {
                float appliedResistance = resistance.Evaluate(degrees / maxDegrees);
                if(appliedResistance != 0) displacement *= 1f / appliedResistance;
            }
        }

        if (displacement == Vector3.zero) rotatableRigidbody.angularVelocity = Vector3.zero;
        else rotatableObject.GetComponent<Rigidbody>().AddForceAtPosition(displacement, cp.stimulationOutput.transform.position);
        inPlace = false;

        return base.onContact(cp);
    }

    protected virtual void onNoContact() {
        if (!inPlace && springBack > 0) {
            Quaternion targetRotation = initialRotation;
            rotatableObject.rotation = Quaternion.Lerp(rotatableObject.rotation, targetRotation, springBack * Time.deltaTime);
            if (rotatableObject.rotation == targetRotation) inPlace = true;
        }
    }

}
