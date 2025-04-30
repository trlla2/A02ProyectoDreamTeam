using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    [SerializeField] public PowerUpEffect powerUpEffect;  // Referencia al efecto de PowerUp
    [SerializeField] private GameObject SpawnParticles;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (powerUpEffect != null)
        {
            spriteRenderer.sprite = powerUpEffect.powerUpSprite; // Cambia el sprite seg�n el efecto
        }

    }

    private void Start()
    {
        GameObject temp = Instantiate(SpawnParticles, transform.position, transform.rotation); // spawn particles and sfx
        Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);// desptroy gameobject at the end
    }

    private void OnTriggerEnter(Collider trigger)
    {
        Weapon weapon = trigger.gameObject.GetComponent<Weapon>(); // Verifica si el objeto tiene un Weapon

        if (weapon != null)
        {
            Debug.Log("Collision");
            powerUpEffect.Apply(trigger.gameObject);  // Aplica el efecto al objeto con Weapon
            Destroy(gameObject);  // Destruye el Power-Up despu�s de activarlo
        }
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        powerUpEffect = powerUp;
        spriteRenderer.sprite = powerUpEffect.powerUpSprite; // Cambia el sprite seg�n el efecto

    }
}