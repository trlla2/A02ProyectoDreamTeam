using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HitscanLaser")]
public class HitscanLaser : PowerUpEffect
{
    public int maxReflectionCount = 5;
    public float maxStepDistance = 100.0f;
    public GameObject laserSpritePrefab;  // Prefab de sprite del rayo
    private float spriteHeight = -1f; // Se inicializa en -1 para calcularla la primera vez


    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.SetPowerUp(this);
        }
    }

    public void FireLaser(Vector3 origin, Vector3 direction)
    {
        int reflectionsRemaining = maxReflectionCount;
        Vector3 currentPos = origin;
        Vector3 currentDir = direction;

        while (reflectionsRemaining > 0)
        {
            RaycastHit hit;

            Debug.DrawRay(currentPos, currentDir * maxStepDistance, Color.blue, 1f);
            Debug.Log("Casting ray from: " + currentPos + " in direction: " + currentDir);

            if (Physics.Raycast(currentPos, currentDir, out hit, maxStepDistance))
            {
                Vector3 endPos = hit.point;
                CreateLaserSegment(currentPos, endPos);

                KillTank(hit.collider.gameObject);

                currentDir = Vector3.Reflect(currentDir, hit.normal);
                currentPos = endPos;
                reflectionsRemaining--;
            }
            else
            {
                Vector3 endPos = currentPos + currentDir * maxStepDistance;
                CreateLaserSegment(currentPos, endPos);
                break;
            }

            Debug.Log("Hit " + hit.collider.name);
        }
    }


    private void CreateLaserSegment(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float length = direction.magnitude;

        GameObject laserSegment = Instantiate(laserSpritePrefab);
        Destroy(laserSegment, 1f); // Destruir tras 0.5 segundos

        // Posicionar en el centro del rayo
        laserSegment.transform.position = start + direction * 0.5f;

        // Rotar el sprite para que apunte en la dirección del rayo
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        laserSegment.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        // Calcular la altura del sprite una sola vez
        if (spriteHeight <= 0f)
        {
            SpriteRenderer sr = laserSpritePrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteHeight = sr.bounds.size.y;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer no encontrado en el prefab del rayo.");
                spriteHeight = 1f; // fallback por si falla
            }
        }

        // Escalar en Y para cubrir la longitud del rayo
        float scaleY = length / spriteHeight;
        laserSegment.transform.localScale = new Vector3(1f, scaleY, 1f);
    }

    private void KillTank(GameObject obj)
    {
        Tank_Behaviour tank = obj.GetComponent<Tank_Behaviour>();
        if (tank != null)
        {
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

}
