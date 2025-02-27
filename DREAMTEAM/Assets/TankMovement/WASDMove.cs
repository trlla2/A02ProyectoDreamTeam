using UnityEngine;

//No se si este codigo sera el final, solo es para tener algo que se mueva
[RequireComponent(typeof(Rigidbody2D))]
public class WASDMove : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float RotationSpeed;

    private Rigidbody2D rb;

    //kinda wish this wasnt here but couldn't think of another way
    //float rotation = 0;

    float movementX;
    float movementY;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementX = 0;
        movementY = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movementX * speed * Time.deltaTime, movementY * speed + Time.deltaTime);

        float horizontalInput = Input.GetAxisRaw("HorizontalAD");
        float verticalInput = Input.GetAxisRaw("VerticalWS");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        }

        //if (Input.GetAxis("HorizontalAD")) //Input.GetKey("d")
        //{
        //    movementX = 1;
        //}else if (Input.GetKey("a") || Input.GetKey("left"))
        //{
        //    movementX = -1;
        //} else
        //{
        //    movementX = 0;
        //}

        //if (Input.GetKey("w") || Input.GetKey("up"))
        //{
        //    movementY = 1;
        //} else if (Input.GetKey("s") || Input.GetKey("down"))
        //{
        //    movementY = -1;
        //} else
        //{
        //    movementY = 0;
        //}
    }
}
