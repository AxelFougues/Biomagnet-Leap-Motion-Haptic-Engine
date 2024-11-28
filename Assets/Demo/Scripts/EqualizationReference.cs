using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqualizationReference : MonoBehaviour{
    public static EqualizationReference instance;

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

    bool initialized = false;

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
        if(audioClipsSlider != null) audioClipsSlider.value = audioClipEqualization;
        initialized = true;
    }

    public void onDoEqualization(bool value) {
        doEqualization = value;
    }

    public void onValueChanged(float value) {
        if (!initialized) return;
        signalEqualization.ClearKeys();
        int i = 30;
        foreach (Slider s in sliders) {
            signalEqualization.AddKey(i, s.value);
            i += 30;
        }
        if (audioClipsSlider != null) audioClipEqualization = audioClipsSlider.value;
    }
}
