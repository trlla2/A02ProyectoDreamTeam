using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float RotationSpeed;

    private Rigidbody2D rb;

    float rotation = 0;
    float horizontalInput;
    float verticalInput;
    float initialSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
    }

    void FixedUpdate()
    {
        if(GetComponent<Tank_Behaviour>().GetPlayer() == 1) // Player 1 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalAD");
            verticalInput = Input.GetAxisRaw("VerticalWS");

        } else if(GetComponent<Tank_Behaviour>().GetPlayer() == 2) // Player 2 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalKeys");
            verticalInput = Input.GetAxisRaw("VerticalKeys");
        }

        if (verticalInput != 0)
        {
            speed += Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, MaxSpeed);
        }
        else 
        {
            speed = initialSpeed;
        }
        //calculate new position and rotation values 
        float VerticalVel = verticalInput * Time.deltaTime * speed * 100.0f;
        rotation += horizontalInput * Time.deltaTime * RotationSpeed * 100.0f;

        //apply
        rb.velocity = transform.up * VerticalVel;
        rb.transform.rotation = Quaternion.Euler(0, 0, -rotation);
    }
}
