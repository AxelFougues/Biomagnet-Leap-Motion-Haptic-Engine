using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SignalData;
using VectorField;

public class TouchableField : Touchable{

    [Space]
    [Header("References")]
    [Space]

    public VectorField.VectorField vectorField;

    [Space]
    [Header("Material Settings")]
    [Space]
    [Header("- Strength rendering")]
    [Space]

    public bool renderStrength = true;
    [Tooltip("Normalized intensity of feedback over normalized field strength")]
    public AnimationCurve strengthResponse;

    public SignalParameter strengthEffect;
    public float minStrengthEffectValue = 0;
    public float maxStrengthEffectValue = 1;


    [Space]
    [Header("- Direction rendering")]
    [Space]

    public bool renderDirection = false;
    [Tooltip("Normalized intensity of feedback over normalized offset angle to field")]
    public AnimationCurve directionResponse;

    public SignalParameter directionEffect;
    public float minDirectionEffectValue = 0;
    public float maxDirectionEffectValue = 1;



    public override SignalData onContact(StimulationOutput.ContactParameters cp) {
        SignalData si = new SignalData();

        foreach (SignalParameterConstant spc in constantParameters) si.set(spc.signalParameter, spc.value);
        

        if (renderStrength) {

            float strengthEffectNormalizedValue = 0;
            strengthEffectNormalizedValue = strengthResponse.Evaluate(vectorField.getStrength(cp.stimulationOutput.transform));
            si.set(strengthEffect, mapValue(strengthEffectNormalizedValue, 0, 1, minPressureEffectValue, maxPressureEffectValue));
            
        }

        if (renderDirection) {

            float directionEffectNormalizedValue = 0;
            directionEffectNormalizedValue = directionResponse.Evaluate(vectorField.sampleAlignementNormalized(cp.stimulationOutput.transform));
            si.set(directionEffect, mapValue(directionEffectNormalizedValue, 0, 1, minPressureEffectValue, maxPressureEffectValue));

        }

        return si;
    }
}
