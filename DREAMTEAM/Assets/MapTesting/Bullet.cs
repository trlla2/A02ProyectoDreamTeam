using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]public float speed = 10f;
    public Rigidbody2D rb;
    [SerializeField] public float bounceTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up* speed;
    }

    private void OnTriggerEnter2D(Collider2D targetHit)
    {
        if (bounceTime == 0f)
        {
            Destroy(gameObject);
        }
    }
}
