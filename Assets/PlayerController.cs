using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 5f;      // Kecepatan lari
    public float jumpForce = 8f;     // Kekuatan loncat

    [Header("Ground Check (Raycast)")]
    public LayerMask groundLayer;     // Layer untuk mendeteksi tanah
    public float groundCheckDistance = 0.1f; // Jarak raycast ke bawah

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isGrounded;         // Status menapak tanah (dari raycast)

	private int currentScore = 0;      // Skor saat ini (sudah ada)
	private float scoreAccumulator = 0f; // Penampung skor (desimal)
	public float scoreMultiplier = 10f;    // Poin yang didapat per detik

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 1. CEK TANAH (RAYCAST)
        // Kita lakukan ini di Update agar status 'isGrounded' selalu terbaru.
        CheckGround();

        // 2. AUTO-RUN ke kanan
        // Kita atur kecepatan horizontal, tapi biarkan fisika mengatur kecepatan vertikal (Y).
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        // 3. CEK INPUT LOMPAT (Mouse Klik Kiri)
        // Input.GetMouseButtonDown(0) berarti "Klik Kiri Mouse"
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            Jump();
        }

		// --- 4. TAMBAHKAN LOGIKA SKOR INI ---
		// Cek apakah game sedang berjalan (tidak di-pause/di main menu)
		if (Time.timeScale > 0f)
		{
			// Tambah skor berdasarkan waktu (Time.deltaTime) dikali pengali
			scoreAccumulator += Time.deltaTime * scoreMultiplier;

			// Bulatkan skor ke 'int' agar jadi angka bulat
			currentScore = Mathf.FloorToInt(scoreAccumulator);
		}
		// ------------------------------------
    }

    void CheckGround()
    {
        // Hitung titik awal raycast (tepat di tengah-bawah collider)
        Vector2 raycastOrigin = new Vector2(coll.bounds.center.x, coll.bounds.min.y);

        // Tembakkan "laser" (raycast) lurus ke bawah
        // 'coll.bounds.extents.x' dipakai agar raycast-nya selebar collider
        RaycastHit2D hit = Physics2D.BoxCast(
            raycastOrigin, // Titik awal
            new Vector2(coll.bounds.size.x * 0.9f, 0.1f), // Ukuran box (sedikit lebih sempit)
            0f, // Rotasi
            Vector2.down, // Arah ke bawah
            groundCheckDistance, // Jarak tembak
            groundLayer // Hanya deteksi layer ini
        );

        // Jika 'hit' mengenai sesuatu di 'groundLayer', kita menapak tanah
        isGrounded = hit.collider != null;

        // --- (Opsional) Untuk melihat raycast di Scene view ---
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(raycastOrigin, Vector2.down * (groundCheckDistance + 0.05f), rayColor);
        // ---
    }

    void Jump()
    {
        // Menggunakan AddForce (Impulse) lebih baik untuk lompatan instan
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("LONCAT! (via Mouse Klik)");
    }

    // Kita masih pakai ini untuk deteksi KEMATIAN
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Mati!");
        Time.timeScale = 0f; // Menghentikan game
        
        // Coba cari GameManager untuk panggil GameOver
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver();
        }
    }
}