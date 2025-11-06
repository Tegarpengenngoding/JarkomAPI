// Assets/Scripts/ApiService.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System; // Diperlukan untuk 'Action'

// -----------------------------------------------------------------
// MODEL DATA UNTUK MENERIMA HIGHSCORE DARI SERVER
// Strukturnya harus cocok dengan Model 'Highscore.cs' di .NET API

// -----------------------------------------------------------------


public class ApiService : MonoBehaviour
{
    // !!! GANTI URL INI dengan URL API .NET Anda (cek port-nya) !!!
    private string apiUrl = "http://localhost:5123/api/Highscores"; 
    
    // Model data internal untuk MENGIRIM skor
    [System.Serializable]
    private class ScoreData
    {
        public string playerName;
        public int score;
    }

    // ==========================================================
    // BAGIAN 1: KIRIM SKOR BARU (POST) - (Ini sudah ada)
    // ==========================================================
    public void SubmitScore(string playerName, int score)
    {
        ScoreData data = new ScoreData
        {
            playerName = playerName,
            score = score
        };
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(PostScoreCoroutine(jsonData));
    }

    private IEnumerator PostScoreCoroutine(string jsonData)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Mengirim skor ke API: " + jsonData);
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Skor berhasil disimpan! Respon: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Gagal menyimpan skor. Error: " + request.error);
                Debug.LogError("Detail Error: " + request.downloadHandler.text);
            }
        }
    }


    // ==========================================================
    // BAGIAN 2: AMBIL DAFTAR HIGHSCORE (GET) - (Ini yang baru)
    // ==========================================================

    // Method publik yang dipanggil oleh GameOverUI
    public void RequestHighscoreList(Action<HighscoreEntry[]> onSuccess)
    {
        StartCoroutine(GetHighscoresCoroutine(onSuccess));
    }

    private IEnumerator GetHighscoresCoroutine(Action<HighscoreEntry[]> onSuccess)
    {
        // Kita pakai method GET
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            Debug.Log("Mengambil highscore dari API...");
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Sukses!
                string jsonString = request.downloadHandler.text;
                Debug.Log("Data highscore diterima: " + jsonString);

                // Gunakan JsonHelper untuk parse array
                HighscoreEntry[] highscoreList = JsonHelper.GetJsonArray<HighscoreEntry>(jsonString);
                
                // Panggil fungsi 'onSuccess' (yaitu PopulateHighscoreUI)
                // dan kirimkan datanya
                onSuccess?.Invoke(highscoreList);
            }
            else
            {
                // Gagal
                Debug.LogError("Gagal mengambil highscore. Error: " + request.error);
            }
        }
    }
}