using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sweep : MonoBehaviour{

    public Touchable touchable;
    public int sweepMin = 20;
    public int sweepMax = 250;
    public float sweepSpeed = 0.01f;
    public bool inverse = false;
    public TMP_Text displayText;
    public string displayUnit = "Hz";
    public float displayMultiplier = 1f;
    public bool autoReset = false;

    bool inContact = false;

    private void Start() {
        if(inverse) touchable.constantParameters[1].value = sweepMax;
        else touchable.constantParameters[1].value = sweepMin;
    }

    private void Update() {
        if (autoReset && !inContact && touchable.inContact > 0) {

            if (inverse) touchable.constantParameters[1].value = sweepMax;
            else touchable.constantParameters[1].value = sweepMin;
            if (displayText != null) displayText.text = Mathf.RoundToInt(touchable.constantParameters[1].value) * displayMultiplier + displayUnit;
            inContact = true;

        } else if (touchable.inContact > 0) {

            if (inverse) touchable.constantParameters[1].value -= sweepSpeed * Time.deltaTime;
            else touchable.constantParameters[1].value += sweepSpeed * Time.deltaTime;

            if (inverse) {
                if (touchable.constantParameters[1].value <= sweepMin) touchable.constantParameters[1].value = sweepMax;
            } else {
                if (touchable.constantParameters[1].value >= sweepMax) touchable.constantParameters[1].value = sweepMin;
            }

            if (displayText != null) displayText.text = Mathf.RoundToInt(touchable.constantParameters[1].value * displayMultiplier) + displayUnit;

        } else if (inContact) {
            inContact = false;
        }
    }
}
