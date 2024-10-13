using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchableTrigger : Touchable{

    [Space]
    [Header("Events")]
    [Space]

    public UnityEvent OnTriggered;
    public UnityEvent OnUnTriggered;

    [Space]
    [Header("Audio Feedback")]
    [Space]
    [Range(0, 1)]
    public float eqOverride = 0;
    public AudioClip triggerFeedback;

    [Space]
    [Header("Debug")]
    [Space]
    public bool autoTriggerInEditor = false;
    public bool isTriggered = false;

    private void Start() {
#if UNITY_EDITOR
        if (autoTriggerInEditor) onContactStart(null);
#endif
    }

    public override void onContactStart(StimulationOutput stimulationOutput) {
        base.onContactStart(stimulationOutput);
        if (!isTriggered) {
            isTriggered = true;
            OnTriggered?.Invoke();
            if (triggerFeedback != null && stimulationOutput != null) {
                if(eqOverride > 0) stimulationOutput.audioClipSource.volume = eqOverride;
                else if (EqualizationReference.instance.doEqualization) stimulationOutput.audioClipSource.volume = EqualizationReference.instance.audioClipEqualization;
                stimulationOutput.audioClipSource.PlayOneShot(triggerFeedback);
            }
        }
    }

    public override void onContactEnd(StimulationOutput stimulationOutput) {
        base.onContactEnd(stimulationOutput);
        
        if (isTriggered && inContact == 0) {
            isTriggered = false;
            OnUnTriggered?.Invoke();
        }
    }
}
