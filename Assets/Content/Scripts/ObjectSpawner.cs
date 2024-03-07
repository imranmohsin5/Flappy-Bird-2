using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;      // Array of prefabs to spawn
    [SerializeField] private float spawnRate = 1f;              // Rate at which objects spawn
    [SerializeField] private Vector2 spawnHeightRange = new Vector2(-1f, 2f);    // Range of height for spawning objects
    [SerializeField] private bool randomizeRotation = true;     // Whether to randomize rotation of spawned objects

    private void OnEnable()
    {
        // Check if there are no prefabs assigned
        if (objectsToSpawn == null || objectsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned to spawn.");
            enabled = false; // Disable the Spawner component if no prefabs are assigned
            return;
        }

        // Start spawning objects
        InvokeRepeating(nameof(SpawnObject), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        // Stop spawning objects when disabled
        CancelInvoke(nameof(SpawnObject));
    }

    private void SpawnObject()
    {
        // Randomly select a prefab from the array
        GameObject selectedPrefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // Calculate spawn position within specified height range
        Vector3 spawnPosition = transform.position + Vector3.up * Random.Range(spawnHeightRange.x, spawnHeightRange.y);

        // Calculate spawn rotation
        Quaternion spawnRotation = randomizeRotation ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) : Quaternion.identity;

        // Instantiate the selected prefab at the calculated position and rotation
        Instantiate(selectedPrefab, spawnPosition, spawnRotation);
    }
}
