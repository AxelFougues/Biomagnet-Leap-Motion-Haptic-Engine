using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorField : MonoBehaviour{

    public List<Transform> vectors;
    public float maxDistance = 10f;
    [Space]
    public Vector3 gizmoScale = Vector3.one;
    public Vector3Int gizmoCount = Vector3Int.one * 10;
    public Gradient gizmoColor = new Gradient();

    public float sampleDirectional(Vector3 position, Quaternion rotation) {
        if(vectors.Count == 0) return 0;
        Quaternion localRotation = Quaternion.Euler(getDirection(position));
        return 1f - (Quaternion.Angle(rotation, localRotation) / 180f);
    }

    //Returns a normalized vector if all vector's scale.z is 0-1
    public Vector3 getDirection(Vector3 point) {
        if (vectors.Count == 0) return Vector3.zero;
        Vector3 direction = Vector3.zero;
        float sumWeights = 0;

        foreach (Transform vector in vectors) {
            float distance = Vector3.Distance(point, vector.position);
            float localStrength = vector.localScale.magnitude / 3f;
            float maxReach = localStrength * maxDistance;
            if (distance < maxReach) {
                float weight = (maxReach - distance) / maxReach;
                direction += vector.forward * localStrength * weight;
                sumWeights += weight;
            }
        }

        if (sumWeights == 0) return Vector3.zero;

        return direction / sumWeights;
    }



    //Returns 0-1 if all vector's scale.z is 0-1
    public float getStrength(Transform transform) { return getStrength(transform.position); }
    public float getStrength(Vector3 point) {
        if (vectors.Count == 0) return 0;
        float strength = 0;
        float sumWeights = 0;

        foreach (Transform vector in vectors) {
            float distance = Vector3.Distance(point, vector.position);
            float localStrength = vector.localScale.magnitude / 3f;
            float maxReach = localStrength * maxDistance;
            if (distance < maxReach) {
                float weight = (maxReach - distance) / maxReach;
                strength += localStrength * weight;
                sumWeights += weight;
            }
        }

        if (sumWeights == 0) return 0;

        return strength / sumWeights;
    }


    private void OnDrawGizmos() {
        for (int x = 0; x < gizmoCount.x; x++) {
            for (int y = 0; y < gizmoCount.y; y++) {
                for (int z = 0; z < gizmoCount.z; z++) {
                    Vector3 position = new Vector3(
                        transform.position.x - ((gizmoCount.x/2) * gizmoScale.x) + (x * gizmoScale.x),
                        transform.position.y - ((gizmoCount.y / 2) * gizmoScale.y) +(y * gizmoScale.y),
                        transform.position.z - ((gizmoCount.z / 2) * gizmoScale.z) + (z * gizmoScale.z));
                    float strength = getStrength(position);
                    if (strength > 0) {
                        Vector3 direction = getDirection(position);
                        ForGizmo(position, direction, gizmoColor.Evaluate(strength), 0.25f, 10);
                    }
                }
            }
        }

        foreach (Transform vector in vectors) {
            ForGizmo(vector.position, vector.forward, gizmoColor.Evaluate(vector.localScale.magnitude/3f), 0.25f, 20);
        }
    }

    public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

}
