using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public bool Selected = false;

    public Vector3 m_direction;
    public BoidData m_data;

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
        Vector3? centerOfMass = null;

        int countedBoids = 0;

        if (BoidSpawner.m_qtree.GetItems(transform.position, m_data.influenceRadius, ref foundBoids))
        {
            foreach (Boids boid in foundBoids)
                if (boid != this)
                {
                    float dist = Vector2.Distance(boid.transform.position, transform.position);
                    if ((dist <= m_data.influenceRadius) &&
                        Vector2.Dot((boid.transform.position - transform.position).normalized, m_direction.normalized) > m_data.fieldOfView)
                    {
                        if (averageAlignmentHeading is null)
                            averageAlignmentHeading = boid.m_direction;
                        else
                            averageAlignmentHeading += boid.m_direction;

                        if (centerOfMass is null)
                            centerOfMass = boid.transform.position;
                        else
                            centerOfMass += boid.transform.position;

                        if (dist <= m_data.separationRadius)
                            separationHeading -= boid.transform.position - transform.position;

                        countedBoids++;
                    }
                }

            if (averageAlignmentHeading is not null && alignment)
                m_direction += ((averageAlignmentHeading.Value / countedBoids) - m_direction) / m_data.inverseAlignmentForce;

            if (centerOfMass is not null && cohesion)
                m_direction += ((centerOfMass.Value / countedBoids) - transform.position) / m_data.inverseCenterOfMassForce;

            m_direction += (Vector3.zero - transform.position) / m_data.inverseCenterOfFieldForce;

            if (separation)
                m_direction += separationHeading;

        }

        transform.position += m_data.speed * Time.deltaTime * m_direction;
    }
    private void OnDrawGizmos()
    {
        if (Selected)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
        //Gizmos.DrawRay(transform.position, (Vector3.zero - transform.position));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, Vector3.one * m_data.influenceRadius);

        foreach (var item in foundBoids)
        {
            if (Vector2.Distance(item.transform.position, transform.position) <= m_data.influenceRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(new Ray(transform.position, item.transform.position - transform.position));
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(item.transform.position, 0.2f);
            }
        }
    }
}
