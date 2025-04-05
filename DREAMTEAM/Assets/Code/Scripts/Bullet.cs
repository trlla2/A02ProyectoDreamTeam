using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public float bulletSpeed = 10f;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public int bounceTime = 3;
    [SerializeField] private float rayDistance = 0.5f;
    [SerializeField] private AudioSource bounceSfx;

    Vector3 currentDir;
    int bounces = 0;

    void Start()
    {
        SetVelocity();
        currentDir = transform.up;
        // Clamp z values
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
        Destroy(this.gameObject, 10f);
    }

    private void FixedUpdate()
    {
        rb.velocity = currentDir.normalized * bulletSpeed *TimeEvent.speedModifier;
        
        if (Physics.Raycast(transform.position, currentDir, out RaycastHit hit, rayDistance))
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                if (hit.collider.gameObject.GetComponent<Tank_Behaviour>())
                {
                    if (hit.collider.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 1)
                    {
                        GameManager.Instance.GetTank1IsDead();
                        hit.collider.gameObject.GetComponent<Tank_Behaviour>().Dead();
                    }
                    else if (hit.collider.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 2)
                    {
                        GameManager.Instance.GetTank2IsDead();
                        hit.collider.gameObject.GetComponent<Tank_Behaviour>().Dead();
                    }
                    Destroy(this.gameObject);
                }
                else if (!hit.collider.CompareTag("Bullet"))
                {
                    bounceSfx.Play();
                    if (bounces >= bounceTime)
                    {
                        DestroyImmediate(this.gameObject);
                    }
                    else
                    {
                        print("xd");
                        // Calculate reflection direction
                        Vector3 reflectionDirection = Vector3.Reflect(currentDir, hit.normal);
                        currentDir = reflectionDirection;
                        transform.rotation = Quaternion.LookRotation(transform.forward, currentDir);
                        bounces++;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + currentDir.normalized * rayDistance);
    }
    public void SetVelocity()
    {
        rb.velocity = transform.up * bulletSpeed * TimeEvent.speedModifier;
    }
}