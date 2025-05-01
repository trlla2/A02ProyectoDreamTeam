using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HitscanLaser")]
public class HitscanLaser : PowerUpEffect
{
    public int maxReflectionCount = 5;
    public float maxStepDistance = 5.0f;
    public GameObject laserSpritePrefab;  // Prefab de sprite del rayo

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
            if (Physics.Raycast(currentPos, currentDir, out hit, maxStepDistance))
            {
                Vector3 endPos = hit.point;
                CreateLaserSegment(currentPos, endPos);

                TryKillTank(hit.collider.gameObject);

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
    Vector3 normalizedDir = direction.normalized;

    GameObject laserSegment = Instantiate(laserSpritePrefab, start, Quaternion.identity);

    // Rotar para que apunte en la dirección correcta
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    laserSegment.transform.rotation = Quaternion.Euler(0f, 0f, angle);

    // Escalar en Y si el sprite apunta hacia arriba
    laserSegment.transform.localScale = new Vector3(laserSegment.transform.localScale.x, length, laserSegment.transform.localScale.z);

    // Reposicionar al centro del rayo
    laserSegment.transform.position += normalizedDir * length * 0.5f;
    }

    private void TryKillTank(GameObject obj)
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
