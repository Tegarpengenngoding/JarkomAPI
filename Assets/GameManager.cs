using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // kalau pakai TextMeshPro

public class GameManager : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public GameObject leaderboardPanel; // <-- TAMBAHKAN INI

    public GameObject gameOverUI; // panel atau teks Game Over
    public GameObject restartButton;

    // UI Skor Pop-up (gunakan TextMeshProUGUI)
    public TextMeshProUGUI teksScoreSaatIni;

    private bool isGameOver = false;

    void Awake()
    {
        // Pause game di awal dan tampilkan main menu jika ada
        Time.timeScale = 0f;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (leaderboardPanel != null) leaderboardPanel.SetActive(false); // <-- TAMBAHKAN INI
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Berhentikan score
        var scoreMgr = FindObjectOfType<ScoreManager>();
        if (scoreMgr != null)
        {
            scoreMgr.StopScore();
        }

        // Coba ambil skor akhir jika tersedia, jika tidak ada pakai 0
        int skorFinal = 0;
        if (scoreMgr != null)
        {
            // Jika ScoreManager punya properti/metode skor, ganti sesuai implementasi kamu
            // Contoh umum: skorFinal = scoreMgr.CurrentScore; atau scoreMgr.GetScore();
        }

        GameOver(skorFinal);
    }

    // Overload yang menerima skor akhir dan menampilkan ke UI
    public void GameOver(int skorFinal)
    {
        if (isGameOver == false)
        {
            isGameOver = true;
            var scoreMgr = FindObjectOfType<ScoreManager>();
            if (scoreMgr != null)
            {
                scoreMgr.StopScore();
            }
        }

        Debug.Log("Game Over! Skor akhir: " + skorFinal);

        Time.timeScale = 0f;

        // Tampilkan skor ke UI jika ada
        if (teksScoreSaatIni != null)
        {
            teksScoreSaatIni.text = skorFinal.ToString();
        }

        // Tampilkan panel Game Over (prioritas ke gameOverPanel jika diset)
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- FUNGSI BARU UNTUK TUKAR PANEL ---
    // Fungsi ini akan dipanggil oleh Tombol "Leaderboard"
    public void OpenLeaderboard()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (leaderboardPanel != null) leaderboardPanel.SetActive(true);
    }

    // Fungsi ini akan dipanggil oleh Tombol "Kembali"
    public void CloseLeaderboard()
    {
        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
