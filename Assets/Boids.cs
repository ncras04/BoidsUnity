using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public bool Selected = false;

    public Vector3 direction;
    [SerializeField] public float influenceRadius;
    [SerializeField] public float separationForce;
    [SerializeField] public float fieldOfView;
    [SerializeField] public float speed = 0.05f;

    List<Boids> foundBoids;

    private void Awake()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void Update()
    {
        Selected = false;
    }
    private void LateUpdate()
    {
        foundBoids = new List<Boids>();
        if (BoidSpawner.m_qtree.GetItems(transform.position, influenceRadius, ref foundBoids))
        {
            foreach (Boids boid in foundBoids)
            {
                if (boid != this)
                {
                    float dist = Vector2.Distance(boid.transform.position, transform.position);
                    if ((dist <= influenceRadius) && Vector2.Dot((boid.transform.position - transform.position).normalized, direction) > fieldOfView)
                    {
                        float ratio = Mathf.Clamp01(dist / separationForce);
                        direction -= ratio * (boid.transform.position - transform.position);
                    }
                }
            }

            direction = direction.normalized;
        }

        transform.position += speed * Time.deltaTime * direction;
    }
    private void OnDrawGizmos()
    {
        if (Selected)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, direction);
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
