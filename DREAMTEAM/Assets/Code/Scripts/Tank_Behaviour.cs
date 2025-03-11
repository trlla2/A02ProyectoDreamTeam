using UnityEngine;

public class Tank_Behaviour : MonoBehaviour
{

    private Collider2D c;
    private void Start()
    {
        c = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bullet")
        {
            death();
        }
    }


    private void death()
    {
        // stuff before dying
        GameManager.Instance.GetTank1IsDead();

        Destroy(this.gameObject);
    }
}
