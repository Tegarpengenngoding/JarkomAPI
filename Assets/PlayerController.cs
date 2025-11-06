using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check (Raycast)")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isGrounded;

    // --- SEMUA VARIABEL SKOR SUDAH DIHAPUS DARI SINI ---

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        // Auto-run ke kanan
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    void Update()
    {
        // Cek tanah
        CheckGround();
        
        // --- SEMUA LOGIKA SKOR SUDAH DIHAPUS DARI UPDATE ---
    }

    // Dipanggil oleh Player Input Component
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
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

        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(raycastOrigin, Vector2.down * (groundCheckDistance + 0.05f), rayColor);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("LONCAT! (via Input System)");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek jika menabrak rintangan
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Mati!");
        Time.timeScale = 0f; // Hentikan game

        // Panggil LogicManager (sudah benar)
        LogicManager lm = FindObjectOfType<LogicManager>();
        if (lm != null)
        {
            lm.gameOver();
        }
        else
        {
            Debug.LogError("Tidak bisa menemukan Logic Manager");
        }
    }
}