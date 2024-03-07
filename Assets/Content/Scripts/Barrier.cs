using UnityEngine;

public class Barrier : MonoBehaviour
{
    public Transform topTransform;       // Reference to the top part of the Barrier
    public Transform bottomTransform;    // Reference to the bottom part of the Barrier
    public float movementSpeed = 5f;     // Speed at which the pipe moves

    private float leftEdge;              // Leftmost edge of the screen

    private void Start()
    {
        // Calculate the left edge of the screen in world coordinates
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        // Move the Barrier to the left
        transform.position += movementSpeed * Time.deltaTime * Vector3.left;

        // Check if the Barrier has moved out of view
        if (transform.position.x < leftEdge)
        {
            // Destroy the Barrier
            Destroy(gameObject);
        }
    }
}
