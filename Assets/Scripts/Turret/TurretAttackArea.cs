using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackArea : MonoBehaviour
{
    [SerializeField] Turret turret;

    public List<GameObject> enemiesAtRange;

    private bool enemyInRange = false;

    void Start()
    {
        turret = gameObject.GetComponentInParent<Turret>();
        enemiesAtRange = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (turret.isBuild && other.transform.parent != null)
            {
                if (other.transform.parent.tag == "Enemy")
                {
                    if (!enemiesAtRange.Contains(other.transform.parent.gameObject))
                    {
                        enemiesAtRange.Add(other.transform.parent.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (turret.isBuild && other.transform.parent != null)
            {
                if (other.transform.parent.tag == "Enemy" && !turret.isAttacking)
                {
                    if (!enemiesAtRange.Contains(other.transform.parent.gameObject))
                    {
                        enemiesAtRange.Add(other.transform.parent.gameObject);
                    }
                    //StartCoroutine(DelayAttacking(enemiesAtRange, enemiesAtRange.Count));
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            if (enemiesAtRange.Contains(other.transform.parent.gameObject))
            {
                enemiesAtRange.Remove(other.transform.parent.gameObject);
            }
        }
    }

    private void Update()
    {
        // if (enemiesAtRange.Count > 0)
        // {
        //     //Debug.Log("Enemies at range : " + enemiesAtRange.Count);
        if (!turret.isAttacking && turret.isBuild && enemiesAtRange.Count > 0)
        {
            StartCoroutine(DelayAttacking(enemiesAtRange, enemiesAtRange.Count));
        }
        if (turret.buildTurret.GetComponent<TurretActions>().fireDamages)
        {
            turret.AttackEnemies(enemiesAtRange, enemiesAtRange.Count);
            turret.buildTurret.GetComponent<TurretActions>().fireDamages = false;
        }

        // }
    }

    public IEnumerator DelayAttacking(List<GameObject> enemiesAtRange, int enemyNumber)
    {
        _MGR_SoundDesign.Instance.PlaySound("Attack_Turret");
        if (enemyNumber > 0)
        {
            turret.isAttacking = true;
            if (!turret.buildedTurretAnimator.GetBool("IsAttacking"))
                turret.buildedTurretAnimator.SetBool("IsAttacking", true);

            yield return new WaitForSeconds(turret.attackCooldown);
            turret.isAttacking = false;
            if (turret.buildedTurretAnimator.GetBool("IsAttacking"))
                turret.buildedTurretAnimator.SetBool("IsAttacking", false);
        }
        //StopCoroutine(DelayAttacking(enemiesAtRange));
    }

}
