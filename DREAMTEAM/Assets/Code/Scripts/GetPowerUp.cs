using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpEffect powerUpEffect;  // Referencia al efecto de PowerUp

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (powerUpEffect != null)
        {
            spriteRenderer.sprite = powerUpEffect.powerUpSprite; // Cambia el sprite seg�n el efecto
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Weapon weapon = collision.gameObject.GetComponent<Weapon>(); // Verifica si el objeto tiene un Weapon

        if (weapon != null)
        {
            Debug.Log("Collision");
            powerUpEffect.Apply(collision.gameObject);  // Aplica el efecto al objeto con Weapon
            Destroy(gameObject);  // Destruye el Power-Up despu�s de activarlo
        }
    }
}