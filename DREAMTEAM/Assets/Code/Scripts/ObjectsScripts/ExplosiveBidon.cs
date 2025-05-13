using UnityEngine;

public class ExplosiveBidon : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionEffect;
    [SerializeField] private float explosionRadius;

    bool exploded = false; //make sure we have no feedback explosions

    public void Explode()
    {
        if(exploded) return;

        // Create explosion effect
        if (ExplosionEffect != null)
        {
            Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Camera.main.GetComponent<ScreenShake>().Shake();// CameraShake
        }

        // Detect players in explosion radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {

            Tank_Behaviour tank = hit.GetComponent<Tank_Behaviour>();
            Interactable Int = hit.GetComponent<Interactable>();       

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
            if (Int != null && Int.gameObject != gameObject)
            {
                exploded = true;
                Int.BulletHit();
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
