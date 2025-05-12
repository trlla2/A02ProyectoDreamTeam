using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public float bulletSpeed = 10f;
    [SerializeField] public Rigidbody rb;
    [SerializeField] private CapsuleCollider cC;
    [SerializeField] public int bounceTime = 3;
    [SerializeField] private float rayDistance = 0.5f;
    [SerializeField] private GameObject bounceFx;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private float tankParentInmunity = 0.25f;

    [HideInInspector]
    public GameObject tankParentRef;

    Vector3 currentDir;
    Vector3 rigthBound;
    Vector3 leftBound;
    int bounces = 0;

    void Start()
    {
        rigthBound = new Vector3(transform.localScale.x, 0,0);
        leftBound =  new Vector3(-transform.localScale.x, 0,0);

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
        transform.rotation = Quaternion.LookRotation(transform.forward, currentDir);
        RaycastHit hit;

        if(tankParentInmunity > 0)
        {
            tankParentInmunity -= Time.fixedDeltaTime;
            print(tankParentInmunity);
        }

        if (Physics.Raycast(transform.position, currentDir, out hit, rayDistance) || (Physics.Raycast(rigthBound + transform.position, currentDir, out hit, rayDistance)) || (Physics.Raycast(leftBound + transform.position, currentDir, out hit, rayDistance)))
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                Tank_Behaviour behaviour = hit.collider.gameObject.GetComponent<Tank_Behaviour>();
                if (behaviour != null)
                {
                   if(!(hit.collider.gameObject == tankParentRef && tankParentInmunity > 0))
                   {
                        GameManager.Instance.Freeze(); // hit stop

                        StartCoroutine(KillPlayer(behaviour.GetPlayer(), behaviour));
                   }
                }
                else if(hit.collider.gameObject.GetComponent<Interactable>())
                {
                    hit.collider.gameObject.GetComponent<Interactable>().BulletHit();
                    DestroyImmediate(gameObject);
                }
                else
                {
                    GameObject temp = Instantiate(bounceFx, transform.position, transform.rotation); // spawn particles and sfx
                    Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);// desptroy gameobject at the end
                    if (bounces >= bounceTime)
                    {
                        DestroyImmediate(gameObject);
                    }
                    else
                    {
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

    //mAKE SURE WE ARE DESTROYIG THE BULLET on player colision
    private void OnCollisionEnter(Collision hit)
    {
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            Tank_Behaviour behaviour = hit.collider.gameObject.GetComponent<Tank_Behaviour>();
            if (behaviour != null && !(hit.collider.gameObject == tankParentRef && tankParentInmunity > 0))
            {
                GameManager.Instance.Freeze(); // hit stop

                StartCoroutine(KillPlayer(behaviour.GetPlayer(), behaviour));
            }
            else if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                hit.collider.gameObject.GetComponent<Interactable>().BulletHit();
                Destroy(gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + currentDir.normalized * rayDistance);
        Gizmos.DrawLine(transform.position + rigthBound, transform.position + rigthBound + currentDir.normalized * rayDistance);
        Gizmos.DrawLine(transform.position + leftBound, transform.position + leftBound + currentDir.normalized * rayDistance);
    }
    public void SetVelocity()
    {
        rb.velocity = transform.up * bulletSpeed * TimeEvent.speedModifier;
    }

    private IEnumerator KillPlayer(int idPlayer, Tank_Behaviour behaviour)
    {
        hitSFX.Play();

        yield return new WaitForEndOfFrame();


        if (idPlayer == 1)
        {
            GameManager.Instance.GetTank1IsDead();

        }
        else if (idPlayer == 2)
        {
            GameManager.Instance.GetTank2IsDead();
        }

        behaviour.Dead();

        Destroy(this.gameObject);
    }

    public void SetTankParent(GameObject tankParent)
    {
        tankParentRef = tankParent;
    }
}