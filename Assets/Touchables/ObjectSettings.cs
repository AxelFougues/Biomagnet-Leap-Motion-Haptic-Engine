using UnityEngine;
using static StimulationOutput;

[CreateAssetMenu(menuName = "Object Settings")]
public class ObjectSettings : ScriptableObject{

    [Header("Movable Object")]
    [Space]

    
    public Vector3 constraintAxis = Vector3.zero;
    public float maxDistance = 0;
    public AnimationCurve resistance;

    [Space]

    public float springBack = 0;


    public void doMove(ContactParameters cp, Touchable touchable) {
        if (touchable.movableObject == null) return;

        //displacement
        Vector3 displacement = cp.penetrationDirection * cp.penetrationDistance / 1000f;

        //axis constraint
        if (constraintAxis != Vector3.zero) {
            float magnitude = Vector3.Project(displacement, constraintAxis).magnitude * Vector3.Dot(displacement.normalized, constraintAxis);
            displacement = constraintAxis.normalized * magnitude;
            if (Vector3.Angle(displacement, constraintAxis) > 0) return;
        }

        //resistance
        if (maxDistance > 0 && resistance.keys.Length > 0) {
            float totalDistance = Vector3.Distance(touchable.initialPosition, touchable.movableObject.transform.position + displacement) * 1000f;
            float normalizedDistance = totalDistance / maxDistance;
            displacement *= 1f - resistance.Evaluate(normalizedDistance);
        }

        touchable.movableObject.transform.position = touchable.movableObject.transform.position + displacement;
        touchable.inPlace = false;
    }

    public void doRelease(Touchable touchable) {
        if (!touchable.inPlace && springBack > 0) {
            touchable.movableObject.position = Vector3.Lerp(touchable.movableObject.position, touchable.initialPosition, springBack * Time.deltaTime);
            if (touchable.movableObject.position == touchable.initialPosition) touchable.inPlace = true;
        }
    }

}
