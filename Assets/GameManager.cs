using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // kalau pakai TextMeshPro

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // panel atau teks Game Over
    public GameObject restartButton;

    private bool isGameOver = false;

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Berhentikan score
        FindObjectOfType<ScoreManager>().StopScore();

        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        restartButton.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
