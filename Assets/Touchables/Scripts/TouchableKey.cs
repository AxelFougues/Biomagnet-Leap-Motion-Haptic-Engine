using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static StimulationOutput;

public class TouchableKey : TouchableRotatable{

    [Space]
    [Header("Key Settings")]
    [Space]

    public AudioClip triggerRotationFeedback;
    [Space]
    public AudioClip releaseFeedback;
    [Space]
    public float triggerAngle;
    public float unTriggerAngle;
    public float freeUnTriggerAngle;

    [Space]
    [Header("Events")]
    [Space]

    public UnityEvent OnTriggered;
    public UnityEvent OnUnTriggered;

    [Space]
    [Header("Debug")]
    [Space]
    public bool isTriggered = false;



    public override SignalData onContact(ContactParameters cp) {

        float degrees = Quaternion.Angle(rotatableObject.rotation, initialRotation);
        //Pressing
        if (!isTriggered && degrees >= triggerAngle) {
            doTrigger(true);
            cp.stimulationOutput.audioSource.PlayOneShot(triggerRotationFeedback);
        }
        //Releasing
        else if (isTriggered && degrees <= unTriggerAngle) {
            doTrigger(false);
            cp.stimulationOutput.audioSource.PlayOneShot(releaseFeedback);
        }

        return base.onContact(cp);
    }

    protected override void onNoContact() {
        base.onNoContact();
        if (isTriggered) {
            float degrees = Quaternion.Angle(rotatableObject.rotation, initialRotation);
            if (degrees <= freeUnTriggerAngle) doTrigger(false);
        }
    }

    public void doTrigger(bool state) {
        if (state == isTriggered) return;
        if (state) OnTriggered?.Invoke();
        else OnUnTriggered?.Invoke();
        isTriggered = state;
    }

}
