using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

        if(targetHit.gameObject.GetComponent<Tank_Behaviour>()) // if is tank player
        {
            if(targetHit.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 1) // if is player 1
            {
                GameManager.Instance.GetTank1IsDead();// add points to game manager

                targetHit.gameObject.GetComponent<Tank_Behaviour>().Dead(); // Destroy tank
            }
            else if (targetHit.gameObject.GetComponent<Tank_Behaviour>().GetPlayer() == 2) // if is player 2
            {
                GameManager.Instance.GetTank2IsDead();// add points to game manager

                targetHit.gameObject.GetComponent<Tank_Behaviour>().Dead(); // Destroy tank

            }

            Destroy(this.gameObject);// Destroy this game manager
        }
        else
        {
            if (bounceTime <= 0f)
            {
                Destroy(gameObject); //if bounce times is 0, destroy the bullet
            }
            else
            {

                var contact = targetHit.GetContact(0); //seeks for contact
                Vector2 newDirection = Vector2.Reflect(transform.up, contact.normal); //calculate the direction of the bullet that it has to bounce
                rb.velocity = newDirection.normalized * speed;
                bounceTime--; //minus bounce time until it destroys
            }
        }
    }
}
