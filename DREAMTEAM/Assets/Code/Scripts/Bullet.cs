using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float bulletSpeed = 10f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public float bounceTime = 3f;

    void Start()
    {
        SetVelocity();
        // Clamp z values
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D targetHit)
    {
        if (targetHit.gameObject.GetComponent<Tank_Behaviour>())
        {
            if (targetHit.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 1)
            {
                GameManager.Instance.GetTank1IsDead();
                targetHit.gameObject.GetComponent<Tank_Behaviour>().Dead();
            }
            else if (targetHit.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 2)
            {
                GameManager.Instance.GetTank2IsDead();
                targetHit.gameObject.GetComponent<Tank_Behaviour>().Dead();
            }
            Destroy(this.gameObject);
        }
        else if (!targetHit.collider.CompareTag("Bullet"))
        {
            if (bounceTime <= 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position,
                    new Vector3(transform.position.x + rb.velocity.normalized.x,
                               transform.position.y + rb.velocity.normalized.y, 0));
                Vector2 newDirection = Vector2.Reflect(rb.velocity.normalized, hit.normal);
                rb.velocity = newDirection.normalized * bulletSpeed * TimeEvent.speedModifier;
                bounceTime--;
            }
        }
    }

    public void SetVelocity()
    {
        rb.velocity = transform.up * bulletSpeed * TimeEvent.speedModifier;
    }
}