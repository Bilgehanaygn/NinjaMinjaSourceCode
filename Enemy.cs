using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;
    [SerializeField]
    private float speed;
    Transform currentTarget;
    SpriteRenderer heroRender;
    Animator heroAnim;
    private bool currentFlipX;
    private BoxCollider2D heroCollider;
    private Rigidbody2D heroRigidB;
    private RaycastHit2D raycastTop;
    private RaycastHit2D raycastBottom;
    private RaycastHit2D nearRaycast;
    [SerializeField]
    private bool onlyTopRaycast;
    private bool attackMode;
    private LayerMask detectLayer;
    [SerializeField]
    private float addDistanceRaycast;
    // Start is called before the first frame update
    void Start()
    {
        heroRender = GetComponent<SpriteRenderer>();
        currentTarget =  pointB;
        heroAnim = GetComponent<Animator>();
        heroCollider = GetComponent<BoxCollider2D>();
        heroRigidB = GetComponent<Rigidbody2D>();
        detectLayer = 1 << LayerMask.NameToLayer("Player");
        speed = 2.0f;
        heroAnim.SetTrigger("Walk");
        currentFlipX = false;
        attackMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (heroAnim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            return;
        }
        AttackMode();
        if (attackMode == true) {          
            return;
        }
        DetectEnemy();
        DetectPositionTarget();
        Move();
              
    }

    private void DetectPositionTarget() {
        if (transform.position.x == pointA.position.x) {
            if (currentFlipX == true) {
                heroAnim.SetTrigger("Idle");
                StartCoroutine(WaitIdleFlip(2.0f, false));
                currentTarget = pointB;
            }
        }
        else if (transform.position.x == pointB.position.x) {
            if (currentFlipX == false) {
                heroAnim.SetTrigger("Idle");
                StartCoroutine(WaitIdleFlip(2.0f, true));
                currentTarget = pointA;
            }
        }
    }

    IEnumerator WaitIdleFlip(float waitTime, bool flip) {
        heroCollider.offset = new Vector2(heroCollider.offset.x + 5.5f, heroCollider.offset.y);
        yield return new WaitForSeconds(waitTime);
        if (flip == true)
        {
            transform.RotateAround(new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z), transform.up, 180);
        }
        else {
            transform.RotateAround(new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z), transform.up, 180);
        }
        currentFlipX = flip;
        heroAnim.SetTrigger("Walk");

    }


    private void Move() {
        if (!heroAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
        }
    }

    private void DetectEnemy() {
        if (currentFlipX == false) {
            //Debug.DrawRay(transform.position + new Vector3(-0.3f, 0.6f, 0.0f), Vector2.right + new Vector2(pointB.position.x - transform.position.x, pointB.position.y - transform.position.y), Color.red);
            //Debug.DrawRay(transform.position + new Vector3(-0.3f, -0.5f, 0.0f), Vector2.right + new Vector2(pointB.position.x - transform.position.x, pointB.position.y - transform.position.y), Color.red);
            if (onlyTopRaycast == true)
            {
                raycastTop = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0.6f, 0.0f), Vector2.right, Mathf.Abs(pointB.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
            }
            else {
                raycastTop = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0.6f, 0.0f), Vector2.right, Mathf.Abs(pointB.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
                raycastBottom = Physics2D.Raycast(transform.position + new Vector3(-0.3f, -0.5f, 0.0f), Vector2.right, Mathf.Abs(pointB.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
            }
        }
        else{
            //Debug.DrawRay(transform.position + new Vector3(0.3f, 0.6f, 0.0f), Vector2.left + new Vector2(pointA.position.x - transform.position.x, pointA.position.y - transform.position.y), Color.red);
            //Debug.DrawRay(transform.position + new Vector3(0.3f, -0.5f, 0.0f),Vector2.left + new Vector2(pointA.position.x - transform.position.x, pointA.position.y - transform.position.y), Color.red);
            if (onlyTopRaycast == true)
            {
                raycastTop = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0.6f, 0.0f), Vector2.left, Mathf.Abs(pointA.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
            }
            else {
                raycastTop = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0.6f, 0.0f), Vector2.left, Mathf.Abs(pointA.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
                raycastBottom = Physics2D.Raycast(transform.position + new Vector3(0.3f, -0.5f, 0.0f), Vector2.left, Mathf.Abs(pointB.position.x - transform.position.x) + addDistanceRaycast, detectLayer.value);
            }
        }

        if (raycastTop.collider != null || raycastBottom.collider != null) {
            if (raycastTop.collider ==  GameObject.Find("NinjaSprite").GetComponent<CapsuleCollider2D>() || raycastBottom.collider == GameObject.Find("NinjaSprite").GetComponent<CapsuleCollider2D>()) {
                attackMode = true;
                heroAnim.SetTrigger("attackMode");
                speed = 13.0f;
            }
            
        }
        
    }

    private void AttackMode() {
        if (attackMode == true) {
            if (GameObject.Find("NinjaSprite") != null) {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("NinjaSprite").transform.position, speed * Time.deltaTime);
                if (currentFlipX == false)
                {
                    nearRaycast = Physics2D.Raycast(transform.position, Vector2.right, 4.0f, detectLayer.value);
                    //Debug.DrawRay(transform.position, Vector2.left, Color.blue, 4.0f);
                    if (nearRaycast.collider == GameObject.Find("NinjaSprite").GetComponent<CapsuleCollider2D>()) {
                        heroAnim.SetTrigger("shieldAttack");
                    }
                }
                else {
                    nearRaycast = Physics2D.Raycast(transform.position, Vector2.left, 4.0f, detectLayer.value);
                    //Debug.DrawRay(transform.position, Vector2.left, Color.blue, 4.0f);
                    if (nearRaycast.collider == GameObject.Find("NinjaSprite").GetComponent<CapsuleCollider2D>()) {
                        heroAnim.SetTrigger("shieldAttack");
                    }
                } 
            }
        }
    }


    void IDamageable.Dead()
    {
        heroAnim.SetTrigger("Die");
        if (currentFlipX == false)
        {
            transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);
        }
        else {
            transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
        }
        heroRigidB.bodyType = RigidbodyType2D.Static;
        Destroy(heroCollider);
        StartCoroutine(WaitDestroy(4.0f));
    }

    IEnumerator WaitDestroy(float givenTime) {
        yield return new WaitForSeconds(givenTime);
        Destroy(gameObject);
    }
}
