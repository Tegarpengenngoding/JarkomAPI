using UnityEngine;

// Nama class diubah menjadi BackgroundSpawner
public class BackgroundSpawner : MonoBehaviour
{
    // Variabel diganti namanya agar sesuai dengan "Background"
    public GameObject backgroundPrefab; // prefab background
    public Transform player;            // referensi ke player (masih sama)
    public float backgroundLength = 20f; // panjang background (biasanya lebih panjang dari ground)
    public int numberOfBackgrounds = 3;  // berapa potong aktif di layar

    private GameObject[] backgrounds;
    private int nextBackgroundIndex = 0;

    void Start()
    {
        // buat pool background
        backgrounds = new GameObject[numberOfBackgrounds];
        for (int i = 0; i < numberOfBackgrounds; i++)
        {
            // --- MODIFIKASI 1 ---
            // Saat pertama kali dibuat, ambil posisi Y player, bukan Y=0
            Vector3 pos = new Vector3(i * backgroundLength, transform.position.y, 0);

            // PENTING: Posisikan background di belakang (sumbu Z)
            // Ganti '10f' jika perlu (misal: 10f agar di belakang player/ground di Z=0)
            pos.z = 10f; 
            
            backgrounds[i] = Instantiate(backgroundPrefab, pos, Quaternion.identity);
            
            // (Opsional) Jadikan child dari spawner ini agar rapi di Hierarchy
            backgrounds[i].transform.parent = transform; 
        }
    }

    void Update()
    {
        // Cek apakah player sudah melewati batas background 'terdepan'
        // Kita gunakan 'backgroundLength / 2' sebagai buffer agar lebih mulus
        if (player.position.x > backgrounds[nextBackgroundIndex].transform.position.x + (backgroundLength / 2f) + 30)
        {
            // Ambil background yang akan didaur ulang (yang paling belakang)
            GameObject bgToMove = backgrounds[nextBackgroundIndex];

            // --- MODIFIKASI 2 (Inti Permintaan Kamu) ---
            
            // 1. Hitung posisi X baru (jauh di depan)
            float newX = bgToMove.transform.position.x + (backgroundLength * numberOfBackgrounds );

            // 3. Terapkan posisi baru
            // Sumbu Z diambil dari posisi Z yang sudah ada (bgToMove.transform.position.z)
            bgToMove.transform.position = new Vector3(newX, transform.position.y, bgToMove.transform.position.z);

            // ---------------------------------------------

            // update index untuk menunjuk ke background 'terdepan' berikutnya
            nextBackgroundIndex = (nextBackgroundIndex + 1) % numberOfBackgrounds;
        }
    }
}