using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public bool Selected = false;

    public Vector3 m_direction;
    [SerializeField][Range(0, 20)] public float influenceRadius;
    [SerializeField][Range(1, 100)] public float inverseSeparationForce;
    [SerializeField][Range(1, 100)] public float inverseAlignmentForce;
    [SerializeField][Range(1, 100)] public float inverseCenterOfMassForce;
    [SerializeField][Range(0, 1)] public float fieldOfView;
    [SerializeField][Range(0, 1)] public float speed = 0.05f;

    public bool separation, alignment, cohesion;

    List<Boids> foundBoids;

    private void Awake()
    {
        m_direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void Update()
    {
        Selected = false;
    }
    private void LateUpdate()
    {
        foundBoids = new List<Boids>();

        Vector3 separationHeading = Vector3.zero;
        Vector3? averageAlignmentHeading = null;

        int countedBoids = 0;

        if (BoidSpawner.m_qtree.GetItems(transform.position, influenceRadius, ref foundBoids))
        {
            foreach (Boids boid in foundBoids)
                if (boid != this)
                {
                    float dist = Vector2.Distance(boid.transform.position, transform.position);
                    if ((dist <= influenceRadius) &&
                        Vector2.Dot((boid.transform.position - transform.position).normalized, m_direction.normalized) > fieldOfView)
                    {
                        if (averageAlignmentHeading is null)
                            averageAlignmentHeading = boid.m_direction;
                        else
                            averageAlignmentHeading += boid.m_direction;

                        separationHeading -= boid.transform.position - transform.position;

                        countedBoids++;
                    }
                }

            if (averageAlignmentHeading is not null && alignment)
                m_direction += ((averageAlignmentHeading.Value / countedBoids) - m_direction) / inverseAlignmentForce;

            if (cohesion)
                m_direction += (BoidSpawner.centerOfMass - transform.position) / inverseCenterOfMassForce;

            if (separation)
                m_direction += separationHeading;

        }

        transform.position += speed * Time.deltaTime * m_direction;
    }
    private void OnDrawGizmos()
    {
        if (Selected)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, m_direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, Vector3.one * influenceRadius);

        foreach (var item in foundBoids)
        {
            if (Vector2.Distance(item.transform.position, transform.position) <= influenceRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(new Ray(transform.position, item.transform.position - transform.position));
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(item.transform.position, 0.2f);
            }
        }
    }
}
