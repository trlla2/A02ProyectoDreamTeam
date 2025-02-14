using UnityEngine;

//No se si este codigo sera el final, solo es para tener algo que se mueva
[RequireComponent(typeof(Rigidbody2D))]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float RotationSpeed;

    private Rigidbody2D rb;

    //kinda wish this wasnt here but couldn't think of another way
    float rotation = 0; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //calculate new position and rotation values 
        float VerticalVel = Input.GetAxis("Vertical") * Time.deltaTime * speed * 100.0f;
        rotation += Input.GetAxis("Horizontal") * Time.deltaTime * RotationSpeed * 100.0f;
        //aply
        rb.velocity = transform.up * VerticalVel;
        rb.transform.rotation = Quaternion.Euler(0,0, -rotation);
    }
}
