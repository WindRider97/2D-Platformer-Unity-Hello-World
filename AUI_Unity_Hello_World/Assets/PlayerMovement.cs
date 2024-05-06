using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private BoxCollider2D boxCollider;

    private float dirHorizontal;

    private enum MvmtState
    {
        idle,
        running,
        jumping,
        falling,
    }


    [SerializeField]
    float jumpSpeed = 14f;
    [SerializeField]
    float runSpeed = 7f;
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        rb.velocity = new Vector2(dirHorizontal * runSpeed, rb.velocity.y);

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MvmtState state;
        if (dirHorizontal > 0f)
        {
            state = MvmtState.running;

            spriteRenderer.flipX = false;
        }
        else if (dirHorizontal < 0f)
        {
            state = MvmtState.running;
            spriteRenderer.flipX = true;
        }
        else
        {
            state = MvmtState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MvmtState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MvmtState.falling;
        }


        animator.SetInteger("MvmtState", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        dirHorizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
    }
}
