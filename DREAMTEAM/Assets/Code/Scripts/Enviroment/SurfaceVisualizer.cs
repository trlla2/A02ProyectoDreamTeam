using UnityEngine;

public class SurfaceVisualizer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color reflectionColor = Color.red;

    private RaycastHit hit;
    private bool hasHit;
    private Vector3 reflectionDirection;

    void Update()
    {
        // Cast ray forward from object's position
        hasHit = Physics.Raycast(transform.position, transform.forward, out hit, rayLength);

        if (hasHit)
        {
            // Calculate reflection direction using vector math
            Vector3 incomingDirection = transform.forward;
            reflectionDirection = Vector3.Reflect(incomingDirection, hit.normal);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
        if (!hasHit) return;

        // Draw main ray
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, hit.point);

        // Draw normal line
        Gizmos.color = normalColor;
        Gizmos.DrawLine(hit.point, hit.point + hit.normal);

        // Draw reflection line
        Gizmos.color = reflectionColor;
        Gizmos.DrawLine(hit.point, hit.point + reflectionDirection);
    }
}