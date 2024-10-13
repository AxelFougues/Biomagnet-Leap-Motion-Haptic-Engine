using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrequencySweep : MonoBehaviour{

    public Touchable touchable;
    public int sweepMin = 20;
    public int sweepMax = 250;
    public float sweepSpeed = 0.01f;
    public TMP_Text displayText;

    private void Start() {
        touchable.constantParameters[1].value = sweepMin;
    }

    private void Update() {
        if (touchable.inContact > 0) {

            touchable.constantParameters[1].value += sweepSpeed * Time.deltaTime;

            if (touchable.constantParameters[1].value >= sweepMax) touchable.constantParameters[1].value = sweepMin;

            if (displayText != null) displayText.text = Mathf.RoundToInt(touchable.constantParameters[1].value) + "Hz";
        }
    }
}
