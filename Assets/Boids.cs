using UnityEngine;

public class Boids : MonoBehaviour
{
    public bool Selected = false;

    private void Update()
    {
        Selected = false;
    }
    private void OnDrawGizmos()
    {
        if (Selected)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
