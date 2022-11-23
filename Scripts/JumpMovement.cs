using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    public float buttonTime = 0.5f;
    public float jumpHeight = 5;
    public float cancelRate = 100;
    float jumpTime;
    bool jumping;
    bool jumpCancelled;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float j = Input.GetAxisRaw("Jump");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if(animator.GetBool("isDead") == false)
        {
            if(h == -1.0f)
            {
                animator.SetBool("isWalking", true);
                transform.localScale = new Vector3(-0.5f,0.5f,1.0f);
                if(rigidbody2d.velocity.x > -5)
                {
                    rigidbody2d.AddForce(new Vector3(h*2,0,0));
                }
            } else if (h == 1.0f)
            {
                animator.SetBool("isWalking", true);
                transform.localScale = new Vector3(0.5f,0.5f,1.0f);
                if(rigidbody2d.velocity.x < 5)
                {
                    rigidbody2d.AddForce(new Vector3(h*2,0,0));
                }
            } else if( h == 0) {
                animator.SetBool("isWalking", false);
                if(rigidbody2d.velocity.y == 0)
                {
                    rigidbody2d.velocity = new Vector2(0.0f, rigidbody2d.velocity.y);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidbody2d.gravityScale));
                rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumping = true;
                jumpCancelled = false;
                jumpTime = 0;
            }
            if (jumping)
            {
                jumpTime += Time.deltaTime;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    jumpCancelled = true;
                }
                if (jumpTime > buttonTime)
                {
                    jumping = false;
                }
            }

            Attack(animator);

        }
    }

    private void FixedUpdate()
    {
        if(jumpCancelled && jumping && rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.AddForce(Vector2.down * cancelRate);
        }
    }

    private void Attack(Animator animator){
        float a = Input.GetAxisRaw("Fire3");

        if(a == 1.0f)
        {
            animator.SetBool("isAttacking", true);
        } else if (a == 0.0f)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "EnemyZombie"){
            animator.SetBool("isDead", true);
        }
    }

}
