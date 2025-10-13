using UnityEngine;

public class CanGame : MonoBehaviour
{
    [Header("Settings")]
    public float fallThresholdY = 0f;       // Y position where the can counts as fallen
    public float rotationThreshold = 15f;   // Degrees off upright before counting as "tipped"

    [Header("Scoring")]
    public int scoreValue = 1;              // Points per fallen/tipped can
    public static int totalScore = 0;       // Shared score across all cans

    private bool hasScored = false;

    void Update()
    {
        // Check if the can has fallen below the Y threshold
        if (transform.position.y < fallThresholdY && !hasScored)
        {
            AddScoreAndDestroy();
            return;
        }

        // Check if can is rotated (no longer upright)
        float angleX = Mathf.Abs(transform.eulerAngles.x);
        float angleZ = Mathf.Abs(transform.eulerAngles.z);

        // Convert to -180..180 range for proper comparison
        if (angleX > 180f) angleX -= 360f;
        if (angleZ > 180f) angleZ -= 360f;

        if ((Mathf.Abs(angleX) > rotationThreshold || Mathf.Abs(angleZ) > rotationThreshold) && !hasScored)
        {
            AddScoreAndDestroy();
        }
    }

    void AddScoreAndDestroy()
    {
        hasScored = true;
        totalScore += scoreValue;
        Debug.Log("Can scored! Total Score: " + totalScore);

        // Destroy after a short delay to allow debug log to show
        Destroy(gameObject, 0.1f);
    }
}
