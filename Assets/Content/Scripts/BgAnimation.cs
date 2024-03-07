using UnityEngine;

public class BgAnimation : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 1f;  // Speed of the parallax animation
    private Renderer meshRenderer;                        // Renderer component reference

    private void Awake()
    {
        // Get the Renderer component
        meshRenderer = GetComponent<Renderer>();

        // Check if the Renderer is null
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on object: " + gameObject.name);
            enabled = false;  // Disable the script if Renderer is not found
        }
    }

    private void Update()
    {
        // Calculate texture offset based on animation speed and time
        float offset = animationSpeed * Time.deltaTime;

        // Apply the offset only if the material is using a texture
        if (meshRenderer.material.mainTexture != null)
        {
            Vector2 offsetVector = new Vector2(offset, 0);
            meshRenderer.material.mainTextureOffset += offsetVector;
        }
        else
        {
            Debug.LogWarning("No main texture found on material of object: " + gameObject.name);
        }
    }
}
