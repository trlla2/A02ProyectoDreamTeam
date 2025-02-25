using UnityEngine;

//No se si este codigo sera el final, solo es para tener algo que se mueva
[RequireComponent(typeof(Rigidbody2D))]
public class TankMove : MonoBehaviour
{
    public float speed;
    public float RotationSpeed;

    Rigidbody2D rb;

    float movementX;
    float movementY;

    //kinda wish this wasnt here but couldn't think of another way
    float rotation = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementX = 0;
        movementY = 0;
    }

    void FixedUpdate()
    {
        //    //calculate new position and rotation values 
        //    float VerticalVel = Input.GetAxis("Vertical") * Time.deltaTime * speed * 100.0f;
        //    rotation += Input.GetAxis("Horizontal") * Time.deltaTime * RotationSpeed * 100.0f;
        //    //aply
        //    rb.velocity = transform.up * VerticalVel;
        //    rb.transform.rotation = Quaternion.Euler(0, 0, -rotation);

        rb.velocity = new Vector2(movementX * speed * Time.deltaTime, movementY * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementY = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movementY = -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movementX = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movementX = 1;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            movementY = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            movementX = 0;
        }
    }
}
