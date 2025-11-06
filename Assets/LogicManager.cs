// Assets/Scripts/LogicManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicManager : MonoBehaviour
{
    public int playerScore;
    public TextMeshProUGUI scoreText;
    // public GameObject gameOverScreen; // <-- GANTI INI
    
    // --- TAMBAHKAN DUA BARIS INI ---
    // 1. Referensi ke panel UI baru kita
    public GameOverUI gameOverUI; 
    // 2. Tambahkan bool agar game over tidak dipanggil berkali-kali
    private bool isGameOver = false;

    // ... (fungsi Start() dan [ContextMenu("Increase Score")]) ...

    public void addScore(int scoreToAdd)
    {
        // Pastikan game belum berakhir sebelum menambah skor
        if (!isGameOver)
        {
            playerScore = playerScore + scoreToAdd;
            scoreText.text = playerScore.ToString();
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        // --- GANTI FUNGSI INI ---

        // Cek jika game sudah berakhir, jangan lakukan apa-apa
        if (isGameOver)
        {
            return;
        }

        // Tandai game sudah berakhir
        isGameOver = true;

        // Nonaktifkan game over screen lama Anda (jika masih ada)
        // gameOverScreen.SetActive(true); // <-- HAPUS/KOMENTARI BARIS INI

        // Panggil panel UI baru dan kirimkan skor akhir
        if (gameOverUI != null)
        {
            gameOverUI.ShowPanel(playerScore);
        }
        else
        {
            Debug.LogError("GameOverUI belum di-set di LogicManager!");
        }
    }
}