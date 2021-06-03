using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArcherAttack : MonoBehaviour
{
    private BoxCollider2D hitBox;
    private Rigidbody2D arrowRigid;
    private SpriteRenderer arrowSprite;
    private Vector3 beginningPos;
    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        arrowRigid = GetComponent<Rigidbody2D>();
        arrowSprite = GetComponent<SpriteRenderer>();
        arrowRigid.gravityScale = 0.0f;
        if (transform.parent.GetComponent<EnemyArcher>().flipArcher == false) {
            beginningPos = new Vector3(1.35f, 0.2f, 0.0f);
        }
        else {
            beginningPos = new Vector3(-1.35f, 0.2f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowArrow() {
        arrowRigid.bodyType = RigidbodyType2D.Dynamic;
        arrowSprite.enabled = true;
        hitBox.enabled = true;
        if (transform.parent.GetComponent<EnemyArcher>().flipArcher == false)
        {
            arrowRigid.velocity = new Vector2(15.0f, 0);
        }
        else {
            arrowRigid.velocity = new Vector2(-15.0f, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().Dead();
        }
        arrowSprite.enabled = false;
        hitBox.enabled = false;
        arrowRigid.bodyType = RigidbodyType2D.Static;
        transform.localPosition = beginningPos;
        
        
    }

}
