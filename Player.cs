using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D rigidB;
    private CapsuleCollider2D ninjaCollider;
    private SpriteRenderer ninjaRender;
    private Animator anim;
    private bool currentFlipX;
    [SerializeField]
    private float speed;
    [SerializeField]
    public float jumpForce;
    [SerializeField]
    public bool grounded;
    [SerializeField]
    private bool resetJump;
    private bool currentGroundedState = true;
    private RaycastHit2D hitInfoRight;
    private RaycastHit2D hitInfoLeft;
    private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        ninjaCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        ninjaRender = GetComponent<SpriteRenderer>();
        speed = 3.0f;
        grounded = true;
        resetJump = true;
        groundLayer = 1 << LayerMask.NameToLayer("groundLayer");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ThroughWalls();
        CheckGrounded();
        FallingToTheGround();
    }

    private void Movement()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") & !anim.GetCurrentAnimatorStateInfo(0).IsName("DeathDance") &
            !anim.GetCurrentAnimatorStateInfo(0).IsName("croud"))
        {

            float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            if (horizontalInput < 0)
            {
                if (currentFlipX == false)
                {
                    transform.RotateAround(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.up, 180);
                    currentFlipX = true;
                }
            }
            else if (horizontalInput > 0)
            {
                if (currentFlipX == true)
                {
                    transform.RotateAround(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.up, 180);
                    currentFlipX = false;
                }
            }
            anim.SetFloat("horiInput", Mathf.Abs(horizontalInput));
            rigidB.velocity = new Vector2(horizontalInput * speed, rigidB.velocity.y);

            if (CrossPlatformInputManager.GetButtonDown("JumpButton") && grounded == true)
            {
                anim.SetTrigger("Jump");
                rigidB.velocity = new Vector2(rigidB.velocity.x, jumpForce);
                grounded = false;
                resetJump = false;
                StartCoroutine(WaitTime(0.3f));

            }
        }

        if (CrossPlatformInputManager.GetButtonDown("CroudButton")) {
            anim.SetBool("croud", true);
            rigidB.velocity = new Vector2(0, rigidB.velocity.y);
        }
        if (CrossPlatformInputManager.GetButtonUp("CroudButton")) {
            anim.SetBool("croud", false);
        }
             
    }

    IEnumerator WaitTime(float givenTime)
    {
        yield return new WaitForSeconds(givenTime);
        resetJump = true;
    }

    void CheckGrounded()
    {

        if (currentFlipX == false)
        {
            //Debug.DrawRay(transform.position + new Vector3(0.12f, -0.7f, 0), Vector2.down * (0.4f), Color.red);
            //Debug.DrawRay(transform.position + new Vector3(-0.25f, -0.7f, 0), Vector2.down * (0.4f), Color.red);
            hitInfoRight = Physics2D.Raycast(transform.position + new Vector3(0.12f, -0.7f, 0), Vector2.down, 0.4f, groundLayer.value);
            hitInfoLeft = Physics2D.Raycast(transform.position + new Vector3(-0.25f, -0.7f, 0), Vector2.down, 0.4f, groundLayer.value);
            if ((hitInfoRight.collider != null || hitInfoLeft.collider != null) & resetJump == true)
            {
                grounded = true;
                if (currentGroundedState == false)
                {
                    anim.SetTrigger("isGrounded");
                    currentGroundedState = true;
                }
            }
            else if (hitInfoRight.collider == null & hitInfoLeft.collider == null)
            {
                grounded = false;
                currentGroundedState = false;
            }
        }
        else
        {
            hitInfoRight = Physics2D.Raycast(transform.position + new Vector3(+0.25f, -0.7f, 0), Vector2.down, 0.4f, groundLayer.value);
            hitInfoLeft = Physics2D.Raycast(transform.position + new Vector3(-0.12f, -0.7f, 0), Vector2.down, 0.4f, groundLayer.value);
            //Debug.DrawRay(transform.position + new Vector3(+0.25f, -0.7f, 0), Vector2.down * (0.4f), Color.red);
            //Debug.DrawRay(transform.position + new Vector3(-0.12f, -0.7f, 0), Vector2.down * (0.4f), Color.red);
            if ((hitInfoRight.collider != null || hitInfoLeft.collider != null) & resetJump == true)
            {
                grounded = true;
                if (currentGroundedState == false)
                {
                    anim.SetTrigger("isGrounded");
                    currentGroundedState = true;
                }
            }
            else if (hitInfoLeft.collider == null & hitInfoRight.collider == null)
            {
                grounded = false;
                currentGroundedState = false;
            }
        }
    }

    private void FallingToTheGround()
    {
        if (grounded == false & !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            anim.SetTrigger("Jump");
        }
    }

    void IDamageable.Dead()
    {
        anim.SetTrigger("die");
        Destroy(ninjaCollider);       
        StartCoroutine(WaitToDestroy(1.0f));
    }

    IEnumerator WaitToDestroy(float givenTime) {
        yield return new WaitForSeconds(givenTime);
        Destroy(this.gameObject);
        GameManager.instance.GameOver();
    }

    private void ThroughWalls() {
        if (rigidB.velocity.y > 0.0f)
        {
            ninjaCollider.isTrigger = true;
        }
        else {
            ninjaCollider.isTrigger = false;
        }
    }

}
