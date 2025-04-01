using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private float rotation = 0;
    private float horizontalInput;
    private float verticalInput;
    private float initialSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
    }

    void FixedUpdate()
    {
        if (GetComponent<Tank_Behaviour>().GetPlayer() == 1) // Player 1 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalAD");
            verticalInput = Input.GetAxisRaw("VerticalWS");
        }
        else if (GetComponent<Tank_Behaviour>().GetPlayer() == 2) // Player 2 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalKeys");
            verticalInput = Input.GetAxisRaw("VerticalKeys");
        }

        // Apply global speed modifier
        float currentSpeed = speed * TimeEvent.speedModifier;
        float currentRotationSpeed = rotationSpeed * TimeEvent.speedModifier;

        // Calculate movement
        float verticalVel = verticalInput * Time.deltaTime * currentSpeed * 100.0f;
        rotation += horizontalInput * Time.deltaTime * currentRotationSpeed * 100.0f;

        // Apply movement
        rb.velocity = transform.up * verticalVel;
        rb.transform.rotation = Quaternion.Euler(0, 0, -rotation);
    }
}