using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public CircleCollider2D attackZone;
    public Animator animator;
    public int damages;
    private float attackDelay = 0.2f;


    private void Awake()
    {
        attackZone = transform.GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        damages = gameObject.GetComponent<PlayerStats>().damages;
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (other.gameObject.tag == "Enemy")
            {
                Enemy target = other.gameObject.GetComponent<Enemy>();
                DealDamages(target);
            }
        }
    }

    private void Attack()
    {
        animator.SetBool("IsAttacking", true);
        StartCoroutine(nameof(CooldownAttack));
        Debug.Log("J'attaque : " + animator.GetBool("IsAttacking"));
    }

    private void DealDamages(Enemy target)
    {
        target.TakesDamages(damages, gameObject);
    }

    IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        animator.SetBool("IsAttacking", false);
        Debug.Log("J'ai fini mon attaque :" + animator.GetBool("IsAttacking"));
    }
}
