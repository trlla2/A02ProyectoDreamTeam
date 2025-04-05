using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float bulletSpeed = 10f;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public float bounceTime = 3f;

    void Start()
    {
        SetVelocity(transform.up.normalized);
        // Clamp z values
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Destroy(this.gameObject, 10f);
    }
    private void OnCollisionEnter(Collision targetHit)
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
                // Calculate new direction by rotating the velocity 45 degrees
                Vector3 currentDirection = rb.velocity.normalized;

                // Determine rotation direction (left/right) based on collision position
                Vector3 collisionObjectCenter = targetHit.transform.position;
                Vector3 bulletPosition = transform.position;
                Vector3 collisionSide = (collisionObjectCenter - bulletPosition).normalized;

                // Rotate 45 degrees clockwise or counter-clockwise based on collision side
                float rotationAngle = (collisionSide.x > 0) ? 45f : -45f;
                Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
                Vector3 newDirection = rotation * currentDirection;

                SetVelocity(newDirection);
                bounceTime--;
            }
        }
    }

    public void SetVelocity(Vector3 dir)
    {
        rb.velocity = dir * bulletSpeed * TimeEvent.speedModifier;
    }
}