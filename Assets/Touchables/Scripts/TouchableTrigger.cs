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
    [Header("Debug")]
    [Space]
    public bool isTriggered = false;



    public override void onContactStart() {
        base.onContactStart();
        if (!isTriggered) {
            isTriggered = true;
            OnTriggered?.Invoke();
        }
    }

    public override void onContactEnd() {
        base.onContactEnd();
        if (isTriggered && inContact > 0) {
            isTriggered = false;
            OnUnTriggered?.Invoke();
        }
    }
}