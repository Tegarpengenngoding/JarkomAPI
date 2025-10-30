using UnityEngine;
using TMPro; // gunakan TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public float score;          // nilai score
    public float scoreRate = 1f; // seberapa cepat score naik
    private bool isGameOver = false;

    void Start()
    {
        score = 0f;
    }

    void Update()
    {
        if (!isGameOver)
        {
            score += scoreRate * Time.deltaTime;
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }

    public void StopScore()
    {
        isGameOver = true;
    }
}
