using UnityEngine;
using static StimulationOutput;

public class TouchableMovable : Touchable{
    [Header("Movable Settings")]
    [Space]

    public Vector3 constraintAxis = Vector3.zero;
    public float maxDistance = 0;
    public AnimationCurve resistance;
    public float springBack = 0;

    [Space]
    [Header("References")]
    [Space]

    public Transform movableObject;

    [Space]
    [Header("Debug")]
    [Space]

    public bool inPlace = true;

    [HideInInspector]
    public Vector3 initialPosition = Vector3.zero;

    private void Start() {
        if (movableObject != null) initialPosition = movableObject.position;
    }

    private void Update() {
        if (inContact == 0) onNoContact();
    }

    public override SignalData onContact(ContactParameters cp) {
        
        if (movableObject == null) return base.onContact(cp);

        //displacement
        Vector3 displacement = cp.penetrationDirection * cp.penetrationDistance / 1000f;

        //axis constraint
        if (constraintAxis != Vector3.zero) {
            Vector3 globalAxis = movableObject.TransformDirection(constraintAxis);
            float magnitude = Vector3.Project(displacement, globalAxis).magnitude * Vector3.Dot(displacement.normalized, globalAxis);
            displacement = globalAxis.normalized * magnitude;
            if (Vector3.Angle(displacement, globalAxis) > 0) return base.onContact(cp);
        }

        //resistance
        if (maxDistance > 0 && resistance.keys.Length > 0) {
            float totalDistance = Vector3.Distance(initialPosition, movableObject.transform.position + displacement) * 1000f;
            float normalizedDistance = totalDistance / maxDistance;
            displacement *= 1f - resistance.Evaluate(normalizedDistance);
        }

        movableObject.transform.position = movableObject.transform.position + displacement;
        inPlace = false;

        return base.onContact(cp);
    }

    protected virtual void onNoContact() {
        if (!inPlace && springBack > 0) {
            Vector3 targetPosition = initialPosition;
            movableObject.position = Vector3.Lerp(movableObject.position, targetPosition, springBack * Time.deltaTime);
            if (movableObject.position == targetPosition) inPlace = true;
        }
    }
}
