using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //initialize variables for prefab bullet
    [SerializeField] public float speed = 1000000f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public float bounceTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        rb.velocity = transform.forward * speed; //give direction to bullet when initiate
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.IsAwake());

        //Debug.Log(rb.velocity);
    }

    private void OnCollisionEnter2D (Collision2D targetHit)
    {
        Debug.Log ("Impacto");
        if (bounceTime == 0f)
        {
            Destroy(gameObject); //if bounce times is 0, destroy the bullet
        }
        else
        {
            var contact = targetHit.GetContact(0); //seeks for contact
            Vector2 newDirection = Vector2.Reflect(contact.normal, rb.velocity.normalized); //calculate the direction of the bullet that it has to bounce
            bounceTime--; //minus bounce time until it destroys
        }
    }
}
