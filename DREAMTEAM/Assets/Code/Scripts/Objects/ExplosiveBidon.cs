using UnityEngine;

public class ExplosiveBidon : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionEffect;
    [SerializeField] private float explosionRadius;

    public void Explode()
    {
        // Create explosion effect
        if (ExplosionEffect != null)
        {
            Instantiate(ExplosionEffect, transform.position, transform.rotation);
        }

        // Detect players in explosion radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            Tank_Behaviour tank = hit.GetComponent<Tank_Behaviour>();
            if (tank != null)
            {
                // Check which player was killed
                if (tank.GetPlayer() == 1)
                {
                    GameManager.Instance.GetTank1IsDead();
                    tank.Dead();
                }
                else if (tank.GetPlayer() == 2)
                {
                    GameManager.Instance.GetTank2IsDead();
                    tank.Dead();
                }
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
