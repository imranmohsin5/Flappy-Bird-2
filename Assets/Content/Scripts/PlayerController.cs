using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpStrength = 5f;          // Strength of the player's jump
    public float gravityForce = -9.81f;      // Force of gravity affecting the player
    public float tiltAngle = 5f;             // Angle at which the player tilts
    public float minYPosition = -5f;         // Minimum Y position of the player
    public float maxYPosition = 5f;          // Maximum Y position of the player

    public GameController gameController;   // Reference to the GameController script
    public AudioSource SFXTarget;

    private Vector3 moveDirection;           // Direction of player movement

    private void OnEnable()
    {
        // Reset player's Y position to 0 when enabled
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;

        // Reset movement direction
        moveDirection = Vector3.zero;
    }

    private void Update()
    {
        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Set upward movement direction for jump
            moveDirection = Vector3.up * jumpStrength;
        }

        // Apply gravity to movement direction
        moveDirection.y += gravityForce * Time.deltaTime;

        // Update player position based on movement direction
        transform.position += moveDirection * Time.deltaTime;

        // Clamp player's Y position within defined limits
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minYPosition, maxYPosition);
        transform.position = clampedPosition;

        // Tilt the player based on the direction of movement
        Vector3 rotation = transform.eulerAngles;
        rotation.z = moveDirection.y * tiltAngle;
        transform.eulerAngles = rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collision events
        if (other.gameObject.CompareTag("Barrier"))         // If colliding with a hazard
        {
            gameController.EndGameBtn();                       // End the game
            SFXTarget.Play();
        }
        else if (other.gameObject.CompareTag("Target")) // If colliding with a collectible
        {
            gameController.IncreaseScore(1);                 // Increase score by 1
            SFXTarget.Play();
        }
    }
}
