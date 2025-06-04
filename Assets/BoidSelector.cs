using System.Collections.Generic;
using UnityEngine;

public class BoidSelector : MonoBehaviour
{
    [SerializeField] float m_size;

    void LateUpdate()
    {
        List<Boids> foundBoids = new List<Boids>();
        if (BoidSpawner.m_qtree.GetItems(transform.position, m_size, ref foundBoids))
        {
            foreach (Boids boid in foundBoids)
            {
                if (IsInBounds(boid.transform.position))
                    boid.Selected = true;
            }
        }
    }

    private bool IsInBounds(Vector2 _objPos)
    {
        return
        (_objPos.x >= transform.position.x - m_size) && (_objPos.x <= transform.position.x + m_size) &&
        (_objPos.y >= transform.position.y - m_size) && (_objPos.y <= transform.position.y + m_size);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * m_size * 2);
    }
}
