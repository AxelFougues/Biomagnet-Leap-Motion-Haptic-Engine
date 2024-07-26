using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SignalPreset")]
public class SignalPreset : ScriptableObject {

    public SignalData data = new SignalData();
}

[Serializable]
public class SignalData {
    public enum SignalParameter { GlobalAmplitude, SineAmplitude, SineFrequency, SquareAmplitude, SquareFrequency, SawAmplitude, SawFrequency, AmpModFrequency, FreqModFreq, FreqModAmplitude }

    [Header("Volume / Frequency")]
    [Range(0.0f, 1.0f)]
    public float globalAmplitude = 0f;
    [Space(10)]

    [Header("Tone Adjustment")]
    [Range(0.0f, 1.0f)]
    public float sineAmplitude = 0f;
    [Range(0.0f, 500f)]
    public float sineFrequency = 0f;

    [Space(5)]
    [Range(0.0f, 1.0f)]
    public float squareAmplitude = 0f;
    [Range(0.0f, 500f)]
    public float squareFrequency = 0f;

    [Space(5)]
    [Range(0.0f, 1.0f)]
    public float sawAmplitude = 0f;
    [Range(0.0f, 500f)]
    public float sawFrequency = 0f;

    [Space(10)]

    [Header("Amplitude Modulation")]
    [Range(0.0f, 30.0f)]
    public float ampModFrequency = 0f;
    [Header("Frequency Modulation")]
    [Range(0.0f, 30.0f)]
    public float freqModFrequency = 0f;
    [Range(0f, 100.0f)]
    public float freqModAmplitude = 0f;

    public SignalData() { }

    public void set(SignalParameter signalParameter, float value) {
        switch (signalParameter) {
            case SignalParameter.GlobalAmplitude: globalAmplitude = Mathf.Clamp01(value); break;
            case SignalParameter.SineAmplitude: sineAmplitude = Mathf.Clamp01(value); break;
            case SignalParameter.SineFrequency: sineFrequency = value; break;
            case SignalParameter.SquareAmplitude: squareAmplitude = Mathf.Clamp01(value); break;
            case SignalParameter.SquareFrequency: squareFrequency = value; break;
            case SignalParameter.SawAmplitude: sawAmplitude = Mathf.Clamp01(value); break;
            case SignalParameter.SawFrequency: sawFrequency = value; break;
            case SignalParameter.AmpModFrequency: ampModFrequency = value; break;
            case SignalParameter.FreqModFreq: freqModFrequency = value; break;
            case SignalParameter.FreqModAmplitude: freqModAmplitude = Mathf.Clamp01(value); break;

        }
    }

    public SignalData(SignalData data) {
        if (data == null) return;
        globalAmplitude = Mathf.Clamp01(data.globalAmplitude);

        sineAmplitude = Mathf.Clamp01(data.sineAmplitude);
        sineFrequency = data.sineFrequency;

        squareAmplitude = Mathf.Clamp01(data.squareAmplitude);
        squareFrequency = data.squareFrequency;

        sawAmplitude = Mathf.Clamp01(data.sawAmplitude);
        sawFrequency = data.sawFrequency;

        ampModFrequency = data.ampModFrequency;

        freqModFrequency = data.freqModFrequency;
        freqModAmplitude = Mathf.Clamp(data.freqModAmplitude, 0, 100);
    }


}
