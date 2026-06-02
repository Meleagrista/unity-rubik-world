using UnityEngine;

public class DirectionsGizmo : MonoBehaviour
{
    [SerializeField] private bool showGizmo = false;

    private bool k_ignoreVerticalAxis = true;

    private void OnDrawGizmos()
    {
        if (!showGizmo)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);

        if (k_ignoreVerticalAxis)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
