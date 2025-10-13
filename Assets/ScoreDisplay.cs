using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText; // Use TMP_Text instead of Text

    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + CanGame.totalScore;
    }
}
