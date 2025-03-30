using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TankMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float RotationSpeed;

    private Rigidbody2D rb;
    

    float rotation = 0;
    float horizontalInput;
    float verticalInput;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(GetComponent<Tank_Behaviour>().GetPlayer() == 1)
        {
            horizontalInput = Input.GetAxisRaw("HorizontalAD");
            verticalInput = Input.GetAxisRaw("VerticalWS");
        } else if(GetComponent<Tank_Behaviour>().GetPlayer() == 2)
        {
            horizontalInput = Input.GetAxisRaw("HorizontalKeys");
            verticalInput = Input.GetAxisRaw("VerticalKeys");
        }

        //calculate new position and rotation values 
        float VerticalVel = verticalInput * Time.deltaTime * speed * 100.0f;
        rotation += horizontalInput * Time.deltaTime * RotationSpeed * 100.0f;
        //apply
        rb.velocity = transform.up * VerticalVel;
        rb.transform.rotation = Quaternion.Euler(0, 0, -rotation);
    }
}
