using UnityEngine;

[CreateAssetMenu(fileName = "BoidData", menuName = "Scriptable Objects/BoidData")]
public class BoidData : ScriptableObject
{
    [SerializeField][Range(0, 20)] public float influenceRadius;
    [SerializeField][Range(0, 1)] public float separationRadius;
    [SerializeField][Range(1, 100)] public float inverseSeparationForce;
    [SerializeField][Range(1, 100)] public float inverseAlignmentForce;
    [SerializeField][Range(1, 100)] public float inverseCenterOfMassForce;
    [SerializeField][Range(1, 10000)] public float inverseCenterOfFieldForce;
    [SerializeField][Range(0, 1)] public float fieldOfView;
    [SerializeField][Range(0, 1)] public float speed = 0.05f;
}
