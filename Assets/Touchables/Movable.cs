using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StimulationOutput;

public class Movable : Touchable{

    public Transform visualObject;

    public Vector3 constraintAxis = Vector3.zero;
    public float constraintDistance = 0;
    public AnimationCurve resistance;


    Vector3 initialPosition = Vector3.zero;

    private void Start() {
        initialPosition = visualObject.position;
    }

    public void doMove(ContactParameters cp) {
        Vector3 displacement = cp.penetrationDirection * cp.penetrationDistance;
        visualObject.transform.position = visualObject.transform.position + (displacement/1000f);
    }

}
