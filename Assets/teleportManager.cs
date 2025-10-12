using UnityEngine;

public class teleportManager : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float fallThresholdY = 0f;
    public float checkInterval = 0.1f;

    [Header("XR Origin Reference")]
    public Transform xrOrigin; // Assign XR Origin here in the inspector

    private Vector3 lastTeleportFromPosition;

    private void Start()
    {
        // Automatically find XR Origin if not assigned
        if (xrOrigin == null && transform.parent != null)
            xrOrigin = transform.parent;

        if (xrOrigin == null)
            Debug.LogError("XR Origin not assigned to teleportManager.");

        InvokeRepeating(nameof(CheckForFall), 0, checkInterval);
    }

    public void SaveCurrentPosition()
    {
        if (xrOrigin != null)
            lastTeleportFromPosition = xrOrigin.position;
    }

    public void ReturnToPreviousTeleportPosition()
    {
        if (xrOrigin != null)
            xrOrigin.position = lastTeleportFromPosition;
    }

    private void CheckForFall()
    {
        if (xrOrigin == null) return;

        if (xrOrigin.position.y < fallThresholdY)
            ReturnToPreviousTeleportPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (xrOrigin == null) return;

        if (other.gameObject.name == "Laser")
            ReturnToPreviousTeleportPosition();
    }
}
