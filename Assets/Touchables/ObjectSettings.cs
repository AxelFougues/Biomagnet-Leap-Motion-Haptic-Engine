using UnityEngine;
using static StimulationOutput;

[CreateAssetMenu(menuName = "Object Settings")]
public class ObjectSettings : ScriptableObject{

    [Header("Movable Object")]
    [Space]

    
    public Vector3 constraintAxis = Vector3.zero;
    public float maxDistance = 0;
    public AnimationCurve resistance;


    public void doMove(ContactParameters cp, Touchable touchable) {
        if (touchable.movableObject == null) return;

        //displacement
        Vector3 displacement = cp.penetrationDirection * cp.penetrationDistance / 1000f;

        //axis constraint
        if (constraintAxis != Vector3.zero) {
            displacement = Vector3.Dot(displacement, constraintAxis.normalized) * constraintAxis.normalized;
        }

        //resistance
        if (maxDistance > 0 && resistance.keys.Length > 0) {
            float totalDistance = Vector3.Distance(touchable.initialPosition, touchable.movableObject.transform.position + displacement) * 1000f;
            float normalizedDistance = totalDistance / maxDistance;
            displacement *= 1f - resistance.Evaluate(normalizedDistance);
        }

        touchable.movableObject.transform.position = touchable.movableObject.transform.position + displacement;
    }

}
