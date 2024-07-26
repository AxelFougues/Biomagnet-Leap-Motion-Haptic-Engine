using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StimulationOutput;

public class Movable : Touchable{

    public Transform visualObject;

    public Vector3 constrainAxis = Vector3.zero;
    public float constrainDistance = 0;
    public AnimationCurve resistance;

    public void doMove(ContactParameters contactParameters) {
        
    }

}
