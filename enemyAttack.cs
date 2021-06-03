using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    private BoxCollider2D shieldCollider;
    // Start is called before the first frame update
    void Start()
    {
        shieldCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "NinjaSprite" & collision.GetComponent<IDamageable>() != null) {
            collision.GetComponent<IDamageable>().Dead();
            //gameover
        }
    }
}
