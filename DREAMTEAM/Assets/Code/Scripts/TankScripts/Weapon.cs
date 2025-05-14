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
    [SerializeField] private GameObject explosionShield;

    [Header("Events")]
    public UnityEvent OnShoot;
    public UnityEvent OnSetPowerUp;
    

    private PowerUpEffect currentPowerUp; // Referencia al Power-Up actual

    //infinite bounce
    private bool isInfiniteBounceActive = false;

    //bullet speed up
    private bool isBulletSpeedBoostActive = false;

    //triple shoot
    private bool isTripleShotActive = false;
    private float powerTime = 65f;

    //Hitscan laser
    private bool isHitscanLaserActive = false;
    private HitscanLaser hitscanLaserPowerUp;
    
    //burst fire
    private bool isBurstFireActive = false;
    private int burstCount;
    private float burstDelay;
    private Coroutine burstCoroutine;

    //shield
    [SerializeField] private GameObject shield;
    public bool isShieldActive;

    private Tank_Behaviour tb;

    private bool P1CanSoot = true;
    private bool P2CanSoot = true;


    [SerializeField]
    private float fireRate = 0.7f;
    private WaitForSeconds wait;
    [SerializeField]
    private float shieldInvulnerableFrames = 0.1f;
    private WaitForSeconds waitShield;
    private void Start()
    {
        tb = GetComponent<Tank_Behaviour>();
        firePointActive = firePoint.GetComponent<FirePointActive>();
        if (shield != null)
        {
            shield.SetActive(false);
        }

        wait = new WaitForSeconds(fireRate);
        waitShield = new WaitForSeconds(shieldInvulnerableFrames);

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

    private IEnumerator InvulnerableShieldFrames()
    {
        yield return waitShield;
        isShieldActive = false;
    }
    public void ActivateShield()
    {
        isShieldActive = true;
        if (shield != null)
        {
            shield.SetActive(true);
        }
    }

    public void ShieldHit()
    {
        Instantiate(explosionShield, transform.position, Quaternion.identity);// Explotion
        StartCoroutine(InvulnerableShieldFrames());
        if (shield != null)
        {
            shield.SetActive(false);
        }
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
            else if (isBurstFireActive && currentPowerUp is BurstFirePowerUp)
            {
                if (burstCoroutine != null) StopCoroutine(burstCoroutine);
                burstCoroutine = StartCoroutine(BurstFireRoutine());
            }
            else if (isHitscanLaserActive && hitscanLaserPowerUp != null)
            {
                Vector3 origin = firePoint.transform.position;
                Vector3 direction = firePoint.transform.up;
                hitscanLaserPowerUp.FireLaser(origin, direction);

                //shootSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx);
                //GameObject temp = Instantiate(shootParticles, fireParticlePoint.position, fireParticlePoint.rotation);
              //  Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);
                OnShoot.Invoke();

                return;
            }
            else
            {
                GameObject bulletInstance = Instantiate(bulletSprite, firePoint.transform.position, firePoint.transform.rotation);
                Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

                bulletScript.tankParentRef = gameObject;
                bulletScript.SetTankParent(this.gameObject);
                bulletScript.bounceTime = isInfiniteBounceActive ? 9999 : 3;

                
            }

            shootSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx);
            GameObject temp = Instantiate(shootParticles, fireParticlePoint.position, fireParticlePoint.rotation);
            Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);
            OnShoot.Invoke();
        }
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        currentPowerUp = powerUp;
        powerUpSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx); //Random Pitch
        OnSetPowerUp.Invoke();// invoke event
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
        else if (currentPowerUp is BurstFirePowerUp burstFire)
        {
            Debug.Log("Active BurstFire");
            isBurstFireActive = true;
            burstCount = burstFire.BulletsPerBurst;
            burstDelay = burstFire.DelayBetweenShots;
        }
        else if (currentPowerUp is HitscanLaser hitscan) //hitscan laser
        {
            Debug.Log("Active HitscanLaser");
            isHitscanLaserActive = true;
            hitscanLaserPowerUp = hitscan;
        }
        else if (currentPowerUp is ShieldPowerUp)
        {
            Debug.Log("Active Shield");
            ActivateShield();  // Special activation without timer
            return;  // Skip the timer setup
        }


        StartCoroutine(DisableAfterTime(powerTime));
        Debug.Log("Disabled PowerUp");


        
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
        else if (isBurstFireActive)
        {
            isBurstFireActive = false;
            if (burstCoroutine != null) StopCoroutine(burstCoroutine);
        }
        else if (isHitscanLaserActive)
        {
            isHitscanLaserActive = false;
        }
    }
    private IEnumerator BurstFireRoutine()
    {
        for (int i = 0; i < burstCount; i++)
        {
           
            GameObject bulletInstance = Instantiate(bulletSprite, firePoint.transform.position, firePoint.transform.rotation);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();


            bulletScript.bounceTime = isInfiniteBounceActive ? 9999 : 3;
            bulletScript.tankParentRef = gameObject;        



            shootSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx);
            GameObject temp = Instantiate(shootParticles, fireParticlePoint.position, fireParticlePoint.rotation);
            Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);
            OnShoot.Invoke();
            

            if (i < burstCount - 1)
            {
                yield return new WaitForSeconds(burstDelay);
            }
        }
    }

    public float GetFireRate()
    {
        return fireRate;
    }
}
