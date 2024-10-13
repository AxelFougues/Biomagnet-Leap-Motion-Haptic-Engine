using UnityEngine;
using UnityEngine.Events;
using static StimulationOutput;

public class TouchableToggle : TouchableMovable {
    [Space]
    [Header("Toggle Settings")]
    [Space]

    public AudioClip triggerPressFeedback;
    public AudioClip triggerReleaseFeedback;
    [Space]
    public AudioClip unTriggerPressFeedback;
    public AudioClip unTriggerReleaseFeedback;

    public float triggerDepth;
    public float latchDepth;

    public float visualLatchDepth;

    [Space]
    [Header("Events")]
    [Space]

    public UnityEvent OnTriggered;
    public UnityEvent OnUnTriggered;

    [Space]
    [Header("Debug")]
    [Space]
    public bool isTriggered = false;
    public bool hasLatched = false;

    public override SignalData onContact(ContactParameters cp) {
        
        float depth = Vector3.Distance(movableObject.position, initialPosition) * 1000;
        //Pressing
        if (!isTriggered && !hasLatched && depth >= triggerDepth) {
            trigger(true);
            if (EqualizationReference.instance.doEqualization) cp.stimulationOutput.audioClipSource.volume = EqualizationReference.instance.audioClipEqualization;
            cp.stimulationOutput.audioClipSource.PlayOneShot(triggerPressFeedback);
        }
        if (isTriggered && hasLatched && depth >= triggerDepth) {
            trigger(false);
            if (EqualizationReference.instance.doEqualization) cp.stimulationOutput.audioClipSource.volume = EqualizationReference.instance.audioClipEqualization;
            cp.stimulationOutput.audioClipSource.PlayOneShot(unTriggerPressFeedback);
        }
        //Releasing
        if (isTriggered && !hasLatched && depth <= latchDepth) {
            if (EqualizationReference.instance.doEqualization) cp.stimulationOutput.audioClipSource.volume = EqualizationReference.instance.audioClipEqualization;
            cp.stimulationOutput.audioClipSource.PlayOneShot(triggerReleaseFeedback);
            hasLatched = true;
        }
        if (!isTriggered && hasLatched && depth <= latchDepth) {
            if (EqualizationReference.instance.doEqualization) cp.stimulationOutput.audioClipSource.volume = EqualizationReference.instance.audioClipEqualization;
            cp.stimulationOutput.audioClipSource.PlayOneShot(unTriggerReleaseFeedback);
            hasLatched = false;
        }

        return base.onContact(cp);
    }

    public void trigger(bool state) {
        if (state == isTriggered) return;
        if (state) OnTriggered?.Invoke();
        else OnUnTriggered?.Invoke();
        isTriggered = state;
    }

}
