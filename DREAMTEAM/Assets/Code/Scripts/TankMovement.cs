using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioSource movementSfx;
    [SerializeField, Range(0, 3)] private float maxRandomPitchSfx = 1.2f;
    [SerializeField, Range(0, 3)] private float minRandomPitchSfx = 0.98f;

    private Rigidbody rb;
    private float rotation = 0;
    private float horizontalInput;
    private float verticalInput;
    private float initialSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;

        movementSfx.mute = true; // mute SFX loop

    }

    void FixedUpdate()
    {
        if (GetComponent<Tank_Behaviour>().GetPlayer() == 1) // Player 1 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalAD");
            verticalInput = Input.GetAxisRaw("VerticalWS");

            if(Input.GetAxisRaw("HorizontalAD") != 0 || Input.GetAxisRaw("VerticalWS") != 0) // if ure moving
            {
                movementSfx.mute = false; // Unmute SFX loop
                movementSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx); // Random PitchSound
            }
            else
            {
                movementSfx.mute = true; // mute SFX loop
            }
        }
        else if (GetComponent<Tank_Behaviour>().GetPlayer() == 2) // Player 2 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalKeys");
            verticalInput = Input.GetAxisRaw("VerticalKeys");

            if (Input.GetAxisRaw("HorizontalKeys") != 0 || Input.GetAxisRaw("VerticalKeys") != 0) // if ure moving
            {
                movementSfx.mute = false; // Unmute SFX loop
                movementSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx); // Random PitchSound
            }
            else
            {
                movementSfx.mute = true; // mute SFX loop
            }
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