using Mirror;
using UnityEngine;
using GameAnalyticsSDK;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpFactor;
    [SerializeField]
    private LayerMask groundLayer;
    private Rigidbody2D body;
    private Vector3 initialScale;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float initialGravityScale;
    private float horizontalAxis;

    private void Start()
    {
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraController>().SetPlayer(this);
            var back = GameObject.FindFirstObjectByType<DynamicBackground>();
            back.SetPlayerTransform(transform);

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level1");
        }

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level1");
    }

    private void Awake()
    {
        initialScale = new Vector3(transform.localScale[0], transform.localScale[1], transform.localScale[2]);

        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        initialGravityScale = body.gravityScale;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis == 0f)
        {
            horizontalAxis = SimpleInput.GetAxis("Horizontal");
        }

        if (horizontalAxis > 0.01f)
        {
            transform.localScale = initialScale;
        }
        else if (horizontalAxis < -0.01f)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalAxis * speed, body.velocity.y);

            if (IsOnWall() && !IsGrounded())
            {
                body.gravityScale = 0f;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = initialGravityScale;
            }

            if (Input.GetKey(KeyCode.Space) || SimpleInput.GetButton("Jump"))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        animator.SetBool("Run", horizontalAxis != 0);
        animator.SetBool("Grounded", IsGrounded());
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            animator.SetTrigger("Jump");
            body.velocity = new Vector3(body.velocity.x, jumpFactor);
        }
        else if (IsOnWall() && !IsGrounded())
        {
            if (horizontalAxis == 0f)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 4, 1);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 3);
            }

            wallJumpCooldown = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitResult = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);

        return hitResult.collider != null; ;
    }

    private bool IsOnWall()
    {
        RaycastHit2D hitResult = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0f), 0.05f, groundLayer); ;

        return hitResult.collider != null; ;
    }
}
