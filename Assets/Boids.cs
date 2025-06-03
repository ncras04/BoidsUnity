using UnityEngine;

public class Boids : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawIcon(transform.position, "Boid");
    }
}
