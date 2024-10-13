using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqualizationReference : MonoBehaviour{
    public static EqualizationReference instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        if (doEQToggle != null) doEQToggle.isOn = doEqualization;
        int i = 30;
        foreach(Slider s in sliders) {
            s.value = signalEqualization.Evaluate(i);
            i += 30;
        }
        audioClipsSlider.value = audioClipEqualization;
    }

    public bool doEqualization = true;
    public AnimationCurve signalEqualization;
    [Range(0, 1)]
    public float audioClipEqualization;

    [Space]
    [Header("UI Refs")]
    [Space]

    public Toggle doEQToggle;
    public List<Slider> sliders;
    public Slider audioClipsSlider;

    public void onDoEqualization(bool value) {
        doEqualization = value;
    }

    public void onValueChanged(float value) {
        signalEqualization.ClearKeys();
        int i = 30;
        foreach (Slider s in sliders) {
            signalEqualization.AddKey(i, s.value);
            i += 30;
        }
        audioClipEqualization = audioClipsSlider.value;
    }
}
