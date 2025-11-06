// Assets/Scripts/LogicManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicManager : MonoBehaviour
{
    // === Variabel Inti ===
    public int playerScore; // Skor ini yang akan dikirim
    public TextMeshProUGUI scoreText; // UI Teks untuk skor
    public GameOverUI gameOverUI; // Panel Game Over
    private bool isGameOver = false;

    // === Variabel Skor Baru (Dipindah dari PlayerController) ===
    [Header("Score Settings")]
    public float scoreMultiplier = 10f; // Pastikan ini tidak 0 di Inspector
    private float scoreAccumulator = 0f;

    
    // FUNGSI UPDATE BARU: untuk menghitung skor
    void Update()
    {
        // Hanya hitung skor jika game sedang berjalan
        if (!isGameOver)
        {
            // 1. Akumulasi skor berdasarkan waktu
            scoreAccumulator += Time.deltaTime * scoreMultiplier;
            playerScore = Mathf.FloorToInt(scoreAccumulator);
            
            // 2. Update teks UI
            scoreText.text = playerScore.ToString();
        }
    }

    // Fungsi ini TIDAK DIPAKAI LAGI, karena 'Update' sudah menanganinya
    // public void addScore(int scoreToAdd)
    // { ... }


    public void restartGame()
    {
        Time.timeScale = 1f; // Pastikan game berjalan lagi saat restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        // Cek jika game sudah berakhir, jangan lakukan apa-apa
        if (isGameOver)
        {
            return;
        }

        // Tandai game sudah berakhir
        isGameOver = true;

        // Panggil panel UI baru dan KIRIMKAN SKOR AKHIR
        if (gameOverUI != null)
        {
            // 'playerScore' sekarang punya nilai yang benar (misal: 10)
            gameOverUI.ShowPanel(playerScore);
        }
        else
        {
            Debug.LogError("GameOverUI belum di-set di LogicManager!");
        }
    }
}