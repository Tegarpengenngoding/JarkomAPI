// Assets/Scripts/GameOverUI.cs

using UnityEngine;
using UnityEngine.UI; // Diperlukan untuk Button
using TMPro;          // Diperlukan untuk TextMeshPro (Input Field dan Text)

public class GameOverUI : MonoBehaviour
{
    // ==========================================================
    // === REFERENSI (Harus di-drag-and-drop di Unity Inspector) ===
    // ==========================================================
    
    [Header("Core Services")]
    // Referensi ke GameObject 'GameManager' (yang punya script ApiService)
    public ApiService apiService; 
    
    // Referensi ke GameObject 'LogicManager' (yang punya script LogicManager)
    public LogicManager logicManager;

    [Header("UI Elements")]
    // Referensi ke UI Input Field untuk nama
    public TMP_InputField nameInputField;
    
    // Referensi ke UI Button untuk submit
    public Button submitButton;           

    [Header("Highscore List")]
    // Referensi ke Prefab 'ScoreEntryPrefab' (dari folder Assets)
    public GameObject scoreEntryPrefab;
    
    // Referensi ke 'HighscoreListContainer' (GameObject kosong di panel)
    public Transform highscoreListContainer;
    
    
    // ==========================================================
    // === Variabel Internal ===
    // ==========================================================
    private int finalScore; // Untuk menyimpan skor saat panel muncul

    // ==========================================================
    // === Fungsi Bawaan Unity ===
    // ==========================================================

    void Start()
    {
        // 1. Mendaftarkan fungsi 'SubmitClicked' agar berjalan saat
        //    'submitButton' di-klik.
        submitButton.onClick.AddListener(SubmitClicked);
        
        // 2. Sembunyikan panel ini saat game pertama kali dimulai
        gameObject.SetActive(false);
    }

    // ==========================================================
    // === Fungsi Publik (Dipanggil oleh script lain) ===
    // ==========================================================

    /// <summary>
    /// Ini adalah fungsi utama yang dipanggil oleh LogicManager.gameOver()
    /// </summary>
    /// <param name="score">Skor akhir pemain</param>
    public void ShowPanel(int score)
    {
        // 1. Simpan skor akhir
        finalScore = score;
        
        // 2. Tampilkan panel ini (GameOverPanel)
        gameObject.SetActive(true);
        
        // 3. (Opsional) Langsung fokus ke input field nama
        nameInputField.ActivateInputField();
        
        // 4. Minta daftar highscore terbaru dari server.
        //    Saat data siap, server akan menjalankan fungsi 'PopulateHighscoreUI'
        apiService.RequestHighscoreList(PopulateHighscoreUI);
    }

    // ==========================================================
    // === Fungsi Internal (Dipanggil oleh script ini) ===
    // ==========================================================

    /// <summary>
    /// Fungsi ini berjalan saat 'submitButton' di-klik.
    /// </summary>
    private void SubmitClicked()
    {
        // 1. Ambil nama dari input field
        string playerName = nameInputField.text;

        // 2. Validasi sederhana (jika kosong, beri nama "Guest")
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Guest";
        }

        // 3. Kirim skor ke API (ini berjalan di background)
        apiService.SubmitScore(playerName, finalScore);

        Debug.Log($"Submit diklik! Nama: {playerName}, Skor: {finalScore}");
        
        // 4. Sembunyikan panel UI
        gameObject.SetActive(false); 

        // 5. Restart game
        if (logicManager != null)
        {
            logicManager.restartGame();
        }
        else
        {
            Debug.LogError("LogicManager belum di-set di GameOverUI Inspector!");
        }
    }

    /// <summary>
    /// Fungsi ini berjalan sebagai 'callback' setelah data highscore
    /// berhasil diambil oleh ApiService.
    /// </summary>
    /// <param name="list">Array data highscore yang diterima dari server</param>
    private void PopulateHighscoreUI(HighscoreEntry[] list)
    {
        // 1. Bersihkan semua entri skor lama dari list
        foreach (Transform child in highscoreListContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Loop semua data baru yang diterima dari API
        int rank = 1;
        foreach (var entry in list)
        {
            // 3. Buat (Instantiate) 'Prefab' baru di dalam 'Container'
            GameObject entryGO = Instantiate(scoreEntryPrefab, highscoreListContainer);

            // 4. Cari komponen Text di dalam prefab tersebut
            //    PENTING: Nama "PlayerNameText" dan "ScoreText" harus SAMA
            //    dengan nama GameObject Text di dalam Prefab Anda.
            TMP_Text nameText = entryGO.transform.Find("PlayerNameText").GetComponent<TMP_Text>();
            TMP_Text scoreText = entryGO.transform.Find("ScoreText").GetComponent<TMP_Text>();

            // 5. Isi teks-nya dengan data skor
            if (nameText != null)
            {
                nameText.text = $"{rank}. {entry.playerName}";
            }

            if (scoreText != null)
            {
                scoreText.text = entry.score.ToString();
            }
            
            rank++;
        }
    }
}