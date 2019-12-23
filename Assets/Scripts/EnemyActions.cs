using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    Enemy enemy;
    Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AttackEnds()
    {
        enemyAnimator.SetBool("IsAttacking", false);
    }

    void AttackStarts()
    {
        enemyAnimator.SetBool("IsAttacking", true);
        _MGR_SoundDesign.Instance.PlaySound("Monster_Attack");
    }
}
