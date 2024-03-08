using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5.0f;
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float dashDistance = 5.0f;

    private float moveX;
    // private float moveY;
    private Rigidbody2D rb;
    public Animator anim;
    private bool isGrounded;
    private bool isDashing;
    private float dashTime;
    private float dashCooldown = 2.0f;
    private float lastDash = -2.0f; // Initialize with -2 so that player can dash immediately.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // anim.SetTrigger("Jump");
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1) && Time.time > lastDash + dashCooldown) // Right mouse button
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        // moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, 0);

        // Flip the player's sprite when moving left or right
        if (moveX > 0.005f) // Moving right
        {
            transform.localScale = new Vector3(-1, 1, 1); // Or use your original scale if not (1,1,1)
        }
        else if (moveX < -0.005f) // Moving left
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip sprite horizontally
        }

        // Set animation parameters
        anim.SetBool("isMove", Mathf.Abs(moveX) > 0.005f);

    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        lastDash = startTime;
        Vector3 dashDirection = new Vector3(moveX, 0).normalized; // Dash in the direction of horizontal movement

        while (Time.time < startTime + dashTime)
        {
            rb.MovePosition(transform.position + dashDirection * dashDistance * Time.deltaTime);
            yield return null; // Wait until next frame
        }

        isDashing = false;
    }

    // Collision detection with the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}