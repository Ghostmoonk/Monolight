using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{

    #region Components
    public Animator buildedTurretAnimator;
    public GameObject slotTurret;
    public GameObject buildTurret;
    #endregion

    #region Stats
    public int maxLife;
    public int lifePoints;
    public int attackDamage;
    public int aggressionAmount;
    public int crystalCost;
    public float attackCooldown;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBuild;
    #endregion

    #region UI
    [SerializeField] Image healthBar;
    [HideInInspector] public Player player;
    #endregion

    [HideInInspector] public bool playerAtRangeToBuild;

    void Start()
    {

        // turretSprite = Resources.Load<Sprite>("Sprites/Turret/placeholder_tourelle");
        // slotSprite = Resources.Load<Sprite>("Sprites/Ressources/placeholder_minerai_1");
        playerAtRangeToBuild = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lifePoints = maxLife;
        isBuild = false;
        isAttacking = false;
        buildTurret.SetActive(false);

        healthBar.fillAmount = (float)lifePoints / maxLife;

        //StartCoroutine(AdaptTurretLayer());

    }
    private void Update()
    {
        if (slotTurret.GetComponent<Renderer>().sortingOrder >= buildTurret.GetComponent<Renderer>().sortingOrder)
        {
            buildTurret.GetComponent<Renderer>().sortingOrder = slotTurret.GetComponent<Renderer>().sortingOrder + 1;
        }
    }

    // IEnumerator AdaptTurretLayer()
    // {
    //     yield return new WaitForSeconds(0.2f);
    //     buildTurret.GetComponent<Renderer>().sortingOrder = slotTurret.GetComponent<Renderer>().sortingOrder + 1;
    // }

    public void TakesDamages(int amount, Enemy source)
    {
        _MGR_SoundDesign.Instance.PlaySound("Turret_Damage");
        lifePoints -= amount;
        healthBar.fillAmount = (float)lifePoints / maxLife;

        if (lifePoints <= 0)
        {
            TurretDown();
            source.targetsAtRangeList.Remove(gameObject);
            if (source.countArrayTarget < source.arr_target.Length - 1)
            {
                source.countArrayTarget++;
            }
            source.ChangeTarget(source.arr_target[source.countArrayTarget]);
        }
    }

    public void BuildTurret()
    {
        //Construction d'une tour
        Debug.Log("Construction de la tour");
        _MGR_SoundDesign.Instance.PlaySound("Construction");
        isBuild = true;
        lifePoints = maxLife;
        healthBar.fillAmount = (float)lifePoints / maxLife;
        buildTurret.SetActive(true);

        _MGR_Layer.Instance.SetOrderInLayer(buildTurret.GetComponent<Renderer>());
    }

    public void TurretDown()
    {
        isBuild = false;
        buildedTurretAnimator.SetBool("TurretDestroyed", true);
        _MGR_SoundDesign.Instance.PlaySound("Turret_Destruction");
    }

    public void AttackEnemies(List<GameObject> enemiesList, int enemiesInRangeCount)
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i].GetComponent<Enemy>().pointLife - attackDamage <= 0)
            {
                GameObject enemy = enemiesList[i];
                enemiesList.Remove(enemiesList[i]);
                enemy.GetComponent<Enemy>().TakesDamages(attackDamage, gameObject);
                i--;
            }
            else
            {
                enemiesList[i].GetComponent<Enemy>().TakesDamages(attackDamage, gameObject);
            }
        }
    }

}
