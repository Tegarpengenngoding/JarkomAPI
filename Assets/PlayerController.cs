using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 5f;      // Kecepatan lari
    public float jumpForce = 8f;     // Kekuatan loncat

    [Header("Ground Check (Raycast)")]
    public LayerMask groundLayer;      // Layer untuk mendeteksi tanah
    public float groundCheckDistance = 0.1f; // Jarak raycast ke bawah

    [Header("Scoring")]
    public float scoreMultiplier = 10f; // Poin yang didapat per detik

    // --- Variabel Internal ---
    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isGrounded;         // Status menapak tanah (dari raycast)
    
    // Variabel untuk skor
    private float scoreAccumulator = 0f; // Penampung skor (desimal)
    private int currentScore = 0;      // Skor bulat saat ini

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 1. CEK TANAH (RAYCAST)
        CheckGround();

        // 2. AUTO-RUN ke kanan
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        // 3. CEK INPUT LOMPAT (Mouse Klik Kiri)
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            Jump();
        }
        
        // 4. HITUNG SKOR
        // Cek apakah game sedang berjalan (tidak di-pause)
        if (Time.timeScale > 0f) 
        {
            // Tambah skor berdasarkan waktu
            scoreAccumulator += Time.deltaTime * scoreMultiplier;
            // Bulatkan ke 'int'
            currentScore = Mathf.FloorToInt(scoreAccumulator);
        }
    }

    void CheckGround()
    {
        Vector2 raycastOrigin = new Vector2(coll.bounds.center.x, coll.bounds.min.y);

        RaycastHit2D hit = Physics2D.BoxCast(
            raycastOrigin, 
            new Vector2(coll.bounds.size.x * 0.9f, 0.1f), 
            0f, 
            Vector2.down, 
            groundCheckDistance, 
            groundLayer 
        );

        isGrounded = hit.collider != null;

        // Opsional: Untuk debugging, melihat raycast
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(raycastOrigin, Vector2.down * (groundCheckDistance + 0.05f), rayColor);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("LONCAT! (via Mouse Klik)");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek tabrakan dengan rintangan
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Mati! Mengirim skor: " + currentScore);

        // 1. Cari GameManager
        GameManager gm = FindObjectOfType<GameManager>();

        // 2. Jika ketemu, kirim skornya
        if (gm != null)
        {
            gm.GameOver(currentScore);
        }

        // 3. Matikan Player
        gameObject.SetActive(false);
    }
}