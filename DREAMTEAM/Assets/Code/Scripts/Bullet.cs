using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //initialize variables for prefab bullet
    [SerializeField] public float speed = 10f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public float bounceTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        rb.velocity = transform.up * speed; //give direction to bullet when initiate
    }

    private void OnCollisionEnter2D (Collision2D targetHit)
    {

        if (bounceTime <= 0f)
        {
            Destroy(gameObject); //if bounce times is 0, destroy the bullet
        }
        else
        {

            Debug.Log(transform.up);
            
            var contact = targetHit.GetContact(0); //seeks for contact
            Vector2 newDirection = Vector2.Reflect(transform.up, contact.normal); //calculate the direction of the bullet that it has to bounce
            rb.velocity = newDirection.normalized * speed;
            bounceTime--; //minus bounce time until it destroys
        }
    }
}
