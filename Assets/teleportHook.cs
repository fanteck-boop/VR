using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class teleportHook : MonoBehaviour
{
    public teleportManager teleportManager;
    private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;

    private void Awake()
    {
        teleportationProvider = GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
    }

    private void OnEnable()
    {
        teleportationProvider.beginLocomotion += OnTeleportBegin;
    }

    private void OnDisable()
    {
        teleportationProvider.beginLocomotion -= OnTeleportBegin;
    }

    private void OnTeleportBegin(LocomotionSystem system)
    {
        // Player is about to teleport � save their current position
        teleportManager.SaveCurrentPosition();
    }
}