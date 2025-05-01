using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("SFX Settings")]
    [SerializeField] private AudioSource movementSfx;
    [SerializeField, Range(0, 3)] private float maxRandomPitchSfx = 1.2f;
    [SerializeField, Range(0, 3)] private float minRandomPitchSfx = 0.98f;

    private Rigidbody rb;
    private float rotation = 0;
    private float horizontalInput;
    private float verticalInput;
    private float initialSpeed;
    private bool invertControls = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
        movementSfx.mute = true;
    }

    void FixedUpdate()
    {
        HandlePlayerInput();
        ApplyMovement();
    }

    private void HandlePlayerInput()
    {
        int player = GetComponent<Tank_Behaviour>().GetPlayer();

        if (player == 1) // Player 1 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalAD");
            verticalInput = Input.GetAxisRaw("VerticalWS");
        }
        else if (player == 2) // Player 2 controls
        {
            horizontalInput = Input.GetAxisRaw("HorizontalKeys");
            verticalInput = Input.GetAxisRaw("VerticalKeys");
        }

        // Apply control inversion
        if (invertControls)
        {
            horizontalInput *= -1;
            verticalInput *= -1;
        }

        // Handle SFX
        movementSfx.mute = (horizontalInput == 0 && verticalInput == 0);
        if (!movementSfx.mute)
        {
            movementSfx.pitch = Random.Range(minRandomPitchSfx, maxRandomPitchSfx);
        }
    }

    private void ApplyMovement()
    {
        float currentSpeed = speed * TimeEvent.speedModifier;
        float currentRotationSpeed = rotationSpeed * TimeEvent.speedModifier;

        float verticalVel = verticalInput * Time.deltaTime * currentSpeed * 100f;
        rotation += horizontalInput * Time.deltaTime * currentRotationSpeed * 100f;

        rb.velocity = transform.up * verticalVel;
        rb.transform.rotation = Quaternion.Euler(0, 0, -rotation);
    }

    public float GetInitialSpeed() => initialSpeed;

    public void ModifyControls(bool invert, float newSpeed, float duration)
    {
        invertControls = invert;
        speed = newSpeed;
        StopAllCoroutines();
        StartCoroutine(RestoreControlsAfter(duration));
    }

    private System.Collections.IEnumerator RestoreControlsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        invertControls = false;
        speed = initialSpeed;
    }
}