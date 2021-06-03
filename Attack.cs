using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Attack : MonoBehaviour
{

    Animator ninjaAnim;

    // Start is called before the first frame update
    void Start()
    {
        ninjaAnim = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackAnimation();
    }

    private void AttackAnimation() {
        if (CrossPlatformInputManager.GetButtonDown("AttackButton")) {
            if (transform.parent.GetComponent<Player>().grounded == true) {
                ninjaAnim.SetTrigger("Attack");
            }
            else{
                ninjaAnim.SetTrigger("jumpAttack");
            }           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable>() != null) {
            collision.GetComponent<IDamageable>().Dead();
            GameManager.totalKill++;
            if (ninjaAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                ninjaAnim.SetTrigger("DeathDance");
            }
        }
        
    }
}
