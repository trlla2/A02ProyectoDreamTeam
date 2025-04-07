using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Setup")]
    public GameObject firePoint;
    public GameObject firePointL;
    public GameObject firePointR;
    public GameObject bulletSprite;

    FirePointActive firePointActive;

    [SerializeField] private AudioSource shootSfx;
    [SerializeField] private AudioSource powerUpSfx;
    [Range(0, 3)] private float maxRandomPitchSfx = 1.2f;
    [Range(0, 3)] private float minRandomPitchSfx = 0.98f;
    [SerializeField] private GameObject shootParticles;
    [SerializeField] private Transform  fireParticlePoint;

    [Header("Events")]
    public UnityEvent OnShoot;
    public UnityEvent OnSetPowerUp;
    

    private PowerUpEffect currentPowerUp; // Referencia al Power-Up actual

    private bool isInfiniteBounceActive = false;
    private bool isBulletSpeedBoostActive = false;
    private bool isTripleShotActive = false;
    private float powerTime = 5f;

    private Tank_Behaviour tb;

    private bool P1CanSoot = true;
    private bool P2CanSoot = true;

    [SerializeField]
    private WaitForSeconds wait = new WaitForSeconds(0.7f);
    private void Start()
    {
        tb = GetComponent<Tank_Behaviour>();
        firePointActive = firePoint.GetComponent<FirePointActive>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && tb.GetPlayer() == 1 && P1CanSoot)
        {
            Shoot();
            StartCoroutine(FireRateP1());
        }
        if (Input.GetButtonDown("Fire2") && tb.GetPlayer() == 2 && P2CanSoot)
        {
            Shoot();
            StartCoroutine(FireRateP2());
        }
    }
    IEnumerator FireRateP1()
    {
        P1CanSoot = false;
        yield return wait;
        P1CanSoot = true;
    }
    IEnumerator FireRateP2()
    {
        P2CanSoot = false;
        yield return wait;
        P2CanSoot = true;
    }
    void Shoot()
    {
        if (firePointActive.CanShoot)
        {
            if (isTripleShotActive && currentPowerUp is TripleShot tripleShotPowerUp)
            {
                tripleShotPowerUp.TripleShotFire();
            }
            else if (isBulletSpeedBoostActive && currentPowerUp is bulletSpeedBoost bulletSpeedBoost)
            {
                Debug.Log("BulletSpeedBoostActive");
                bulletSpeedBoost.BulletSpeedBoost();
            }
            else
            {

                GameObject bulletInstance = Instantiate(bulletSprite, firePoint.transform.position, firePoint.transform.rotation);
                Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

                if (isInfiniteBounceActive)
                {
                    bulletScript.bounceTime = 9999;
                }
                else
                {
                    bulletScript.bounceTime = 3;
                }
            }

            shootSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx); //Random Pitch
            GameObject temp = Instantiate(shootParticles, fireParticlePoint.position, fireParticlePoint.rotation); // Spawn particles
            Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration); // destroy particles when ended
            OnShoot.Invoke(); // invoke event
        }
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        currentPowerUp = powerUp;

        if (currentPowerUp is InfiniteBounce)
        {
            Debug.Log("Active InfiniteBounce");
            isInfiniteBounceActive = true;
        }
        else if (currentPowerUp is TripleShot) 
        {
            Debug.Log("Active TripleShot");
            isTripleShotActive = true;
            

        }
        else if (currentPowerUp is bulletSpeedBoost)
        {
            Debug.Log("Active InfiniteBounce");
            isBulletSpeedBoostActive = true;
        }

        StartCoroutine(DisableAfterTime(powerTime));
        Debug.Log("Disabled PowerUp");


        powerUpSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx); //Random Pitch
        OnSetPowerUp.Invoke();// invoke event
    }
    private IEnumerator DisableAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (isBulletSpeedBoostActive)
        {
            isBulletSpeedBoostActive = false;
        }
        else if (isInfiniteBounceActive)
        {
            isInfiniteBounceActive = false;
        }
        else if (isTripleShotActive)
        {
            isTripleShotActive = false;
        }
    }
}
