using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SignalData;
using static StimulationOutput;

public class Touchable : MonoBehaviour{

    [Space]
    [Header("References")]
    [Space]

    public Collider touchCollider;

    [Space]
    [Header("Material Settings")]
    [Space]
    [Header("- Pressure rendering")]
    [Space]

    public bool renderPressure = true;
    [Tooltip("Depth of max pressure, in mm")]
    [Range(0f, 20f)]
    public float pressureMaxDepth = 10f;
    [Tooltip("Normalized intensity of pressure feedback over normalized penetration depth")]
    public AnimationCurve pressureResponse;

    public SignalParameter pressureEffect;
    public float minPressureEffectValue = 0;
    public float maxPressureEffectValue = 1;


    [Space]
    [Header("- Texture rendering")]
    [Space]

    public bool renderTexture = false;
    [Tooltip("Texture frequency in mm")]
    [Range(0f, 10f)]
    public float spatialPeriod = 3f; //in mm
    [Tooltip("Normalized intensity of texture feedback over normalized penetration depth")]
    public AnimationCurve textureResponse;

    public SignalParameter textureEffect;
    public float minTextureEffectValue = 0;
    public float maxTextureEffectValue = 1;

    [Space]
    [Header("Constants")]
    [Space]

    public List<SignalParameterConstant> constantParameters = new List<SignalParameterConstant>();

    [Space]
    [Header("Debug")]
    [Space]

    public int inContact = 0;

    [Serializable]
    public class SignalParameterConstant { public SignalParameter signalParameter = SignalParameter.GlobalAmplitude; public float value = 0; }


    public virtual void onContactStart() {
        inContact++;
    }

    public virtual SignalData onContact(ContactParameters cp) {
        SignalData si = new SignalData();

        foreach (SignalParameterConstant spc in constantParameters) si.set(spc.signalParameter, spc.value);

        if (renderPressure) {

            float pressureEffectNormalizedValue = 0;
            float pDepth = Mathf.Clamp(cp.penetrationDistance, 0, pressureMaxDepth);
            if (pressureResponse.keys.Length > 0) pressureEffectNormalizedValue = pressureResponse.Evaluate(pDepth / pressureMaxDepth);
            else pressureEffectNormalizedValue = mapValue(pDepth, 0, pressureMaxDepth, 0, 1);

            si.set(pressureEffect, mapValue(pressureEffectNormalizedValue, 0, 1, minPressureEffectValue, maxPressureEffectValue));

        }

        if (renderTexture) {
            if (spatialPeriod != 0) {
                float textureEffectNormalizedValue = 0;
                float pDepth = Mathf.Clamp(cp.penetrationDistance, 0, pressureMaxDepth);
                textureEffectNormalizedValue = textureResponse.Evaluate(pDepth / pressureMaxDepth) * Mathf.Clamp((cp.perpendicularVelocity / spatialPeriod), 0, 400);
                si.set(textureEffect, mapValue(textureEffectNormalizedValue, 0, 1, minPressureEffectValue, maxPressureEffectValue));
            }
        }

        return si;
    }

    public virtual void onContactEnd() {
        inContact--;
    }

    public void onSettingsUpdated() {
        
    }

    protected float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

}
