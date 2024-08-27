using UnityEngine;
using UnityEngine.Events;
using static StimulationOutput;

public class TouchableButton : TouchableMovable{

    
    [Space]
    [Header("Buttton Settings")]
    [Space]

    public AudioClip triggerPressFeedback;
    [Space]
    public AudioClip unTriggerReleaseFeedback;
    [Space]
    public float triggerDepth;
    public float unTriggerDepth;

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
        
        float depth = Vector3.Distance(movableObject.position, initialPosition) * 1000;
        //Pressing
        if (!isTriggered && depth >= triggerDepth) {
            doTrigger(true);
            cp.stimulationOutput.audioSource.PlayOneShot(triggerPressFeedback);
        }
        //Releasing
        else if (isTriggered && depth <= unTriggerDepth) {
            doTrigger(false);
            cp.stimulationOutput.audioSource.PlayOneShot(unTriggerReleaseFeedback);
        }

        return base.onContact(cp);
    }

    protected override void onNoContact() {
        base.onNoContact();
        if (isTriggered && inPlace) doTrigger(false);
    }

    public void doTrigger(bool state) {
        if (state == isTriggered) return;
        if (state) OnTriggered?.Invoke();
        else OnUnTriggered?.Invoke();
        isTriggered = state;
    }

}
