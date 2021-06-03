using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour, IDamageable
{
    private BoxCollider2D bodyCollider;
    private RaycastHit2D seeEnemy;
    private LayerMask detectLayer;
    private Animator animArcher;
    private bool attackReady = true;
    [SerializeField]
    public bool flipArcher;
    public bool isFlipped;
    [SerializeField]
    private float rayCastDistance;
    // Start is called before the first frame update
    void Start()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        detectLayer = 1 << LayerMask.NameToLayer("Player");
        animArcher = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animArcher.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            return;
        }
        FlipTheArcher();
        DetectEnemy();
    }

    private void DetectEnemy()
    {
        if (flipArcher == false)
        {
            seeEnemy = Physics2D.Raycast(transform.position, Vector2.right, rayCastDistance, detectLayer.value);
            //Debug.DrawRay(transform.position, Vector2.right * rayCastDistance, Color.red);
        }
        else {
            seeEnemy = Physics2D.Raycast(transform.position, Vector2.left, rayCastDistance, detectLayer.value);
            //Debug.DrawRay(transform.position, Vector2.left * rayCastDistance, Color.red);
        }
        
        if (seeEnemy.collider != null) {
            if (seeEnemy.collider.name == "NinjaSprite")
            {
                if (attackReady == true)
                {
                    animArcher.SetTrigger("attack");
                    StartCoroutine(ThrowLater(0.3f));
                    attackReady = false;
                    StartCoroutine(SetAttackTrue(3.5f));
                }

            }
        }
    }

    IEnumerator ThrowLater(float givenTime) {
        yield return new WaitForSeconds(givenTime);
        transform.GetChild(0).GetComponent<enemyArcherAttack>().ThrowArrow();
    }

    IEnumerator SetAttackTrue(float givenTime)
    {
        yield return new WaitForSeconds(givenTime);
        attackReady = true;
    }

    void IDamageable.Dead()
    {
        animArcher.SetTrigger("death");
        StartCoroutine(DestroyObject(2.5f));
    }

    IEnumerator DestroyObject(float givenTime)
    {
        yield return new WaitForSeconds(givenTime);
        Destroy(gameObject);
        Destroy(transform.GetChild(0));
    }

    private void FlipTheArcher() {
        if (flipArcher == true & isFlipped == false) {
            transform.RotateAround(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.up, 180);
            isFlipped = true;
        }
    }

}
