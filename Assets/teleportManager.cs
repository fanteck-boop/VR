using UnityEngine;

public class teleportManager : MonoBehaviour
{
    private Vector3 lastTeleportFromPosition;
    public float fallThresholdY = 0f;
    public float checkInterval = 0.1f;

    private void Start()
    {
        InvokeRepeating(nameof(CheckForFall), 0, checkInterval);
    }

    public void SaveCurrentPosition()
    {
        lastTeleportFromPosition = transform.position;
    }

    public void ReturnToPreviousTeleportPosition()
    {
        transform.position = lastTeleportFromPosition;
    }

    private void CheckForFall()
    {
        if (transform.position.y < fallThresholdY)
            ReturnToPreviousTeleportPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Laser")
            ReturnToPreviousTeleportPosition();
    }
}