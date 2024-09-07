using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorField {

    public class VectorField : MonoBehaviour {
        [Header("Strength field")]
        [Range(0, 1)]
        public float strengthDistanceMultiplier = 1;
        [Range(0, 100)]
        public float strengthDistancePower = 1;
        [Space]
        [Header("Vector field")]
        [Range(0, 5)]
        public float magnitudeDistanceMultiplier = 1;
        [Range(0, 10)]
        public float magnitudeDistancePower = 1;
        [Space]
        [Header("Gizmos")]
        [Space]
        public bool showStrength = true;
        [Space]
        public bool showVectors = true;
        public bool showDirection = true;
        public bool showMagnitude = true;
        [Space]
        public Vector3 gizmoScale = Vector3.one;
        public Vector3Int gizmoCount = Vector3Int.one * 10;
        public Gradient gizmoColor = new Gradient();

        //Get the angle between a Transform and the local field
        public float sampleAlignement(Vector3 position, Quaternion rotation) {
            if (transform.childCount == 0) return 0;
            Quaternion localRotation = Quaternion.Euler(getDirection(position));
            return Quaternion.Angle(rotation, localRotation);
        }
        public float sampleAlignementNormalized(Vector3 position, Quaternion rotation) { return 1f - sampleAlignement(position, rotation) / 180f; }
        public float sampleAlignement(Transform transform) { return sampleAlignement(transform.position, transform.rotation); }
        public float sampleAlignementNormalized(Transform transform) { return sampleAlignementNormalized(transform.position, transform.rotation); }

        //Get the direction (and magnitude) of the field at a point
        public Vector3 getDirection(Vector3 point) {
            if (transform.childCount == 0) return Vector3.zero;
            Vector3 direction = Vector3.zero;

            foreach (Transform vector in transform) {
                float distance = Mathf.Pow(Vector3.Distance(point, vector.position) * magnitudeDistanceMultiplier, magnitudeDistancePower);
                float localStrength = vector.localScale.z;
                float weight = localStrength / (distance + 1);
                direction += vector.forward * weight;
            }

            return direction;
        }
        public Vector3 getDirection(Transform transform) { return getDirection(transform.position); }
        public Vector3 getDirectionNormalized(Transform transform) { return getDirection(transform.position).normalized; }
        public Vector3 getDirectionNormalized(Vector3 point) { return getDirection(point).normalized; }

        //Get the strength of a field at a point
        public float getStrength(Vector3 point) {
            if (transform.childCount == 0) return 0;
            float strength = 0;

            foreach (Transform vector in transform) {
                float distance = Mathf.Pow(Vector3.Distance(point, vector.position) * strengthDistanceMultiplier, strengthDistancePower);
                float localStrength = vector.localScale.z;
                strength += localStrength / (distance + 1);
            }

            return strength;
        }
        public float getStrength(Transform transform) { return getStrength(transform.position); }
        public float getStrengthNormalized(Transform transform) { return getStrengthNormalized(transform.position); }
        public float getStrengthNormalized(Vector3 point) { return getStrength(point) / transform.childCount; }

        private void OnDrawGizmos() {
            if (showVectors) {
                foreach (Transform vector in transform) {
                    drawArrow(vector.position, vector.forward, gizmoColor.Evaluate(vector.localScale.z), 0.25f, 20);
                }
            }
            if (showStrength || showDirection || showMagnitude) {

                for (int x = 0; x < gizmoCount.x; x++) {
                    for (int y = 0; y < gizmoCount.y; y++) {
                        for (int z = 0; z < gizmoCount.z; z++) {

                            Vector3 position = new Vector3(
                                transform.position.x - ((gizmoCount.x / 2) * gizmoScale.x) + (x * gizmoScale.x),
                                transform.position.y - ((gizmoCount.y / 2) * gizmoScale.y) + (y * gizmoScale.y),
                                transform.position.z - ((gizmoCount.z / 2) * gizmoScale.z) + (z * gizmoScale.z));

                            float strength = 0;
                            if (showStrength) strength = getStrengthNormalized(position);

                            if (showDirection) {
                                Vector3 direction = Vector3.zero;
                                if (showMagnitude) direction = getDirection(position);
                                else direction = getDirectionNormalized(position);
                                drawArrow(position, direction, gizmoColor.Evaluate(Mathf.Clamp01(strength)), 0.25f, 10);
                            } else if (showStrength) {
                                drawX(position, 1, gizmoColor.Evaluate(strength));
                            }
                        }
                    }
                }

            }
        }

        public void drawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
            if (direction.sqrMagnitude <= 0.1f) return;
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        public void drawX(Vector3 pos, float size, Color color) {
            if (size <= float.Epsilon) return;
            Gizmos.color = color;
            if (gizmoCount.y > 1) {
                Gizmos.DrawRay(pos, Vector3.up * size);
                Gizmos.DrawRay(pos, Vector3.down * size);
            }
            if (gizmoCount.x > 1) {
                Gizmos.DrawRay(pos, Vector3.left * size);
                Gizmos.DrawRay(pos, Vector3.right * size);
            }
            if (gizmoCount.z > 1) {
                Gizmos.DrawRay(pos, Vector3.forward * size);
                Gizmos.DrawRay(pos, Vector3.back * size);
            }
        }

    }
}