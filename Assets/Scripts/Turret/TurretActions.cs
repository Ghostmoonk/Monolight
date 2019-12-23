using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretActions : MonoBehaviour
{

    [SerializeField] private Turret turret;
    public SpriteRenderer spriteEffects;
    [SerializeField] private Animator turretAnimator;
    [SerializeField] private GameObject turretEffectObject;
    public bool fireDamages;

    private void Start()
    {
        turretAnimator.Rebind();
        fireDamages = false;
    }

    public void EndDestroyed()
    {
        turretAnimator.SetBool("HasSpawn", false);
        gameObject.SetActive(false);
    }

    public void FireAttacks()
    {
        fireDamages = true;
    }

    public void SpawnOver()
    {
        turretAnimator.SetBool("HasSpawn", true);
    }

    public void TriggerGroundCrush()
    {
        turretEffectObject.SetActive(true);
        turretEffectObject.GetComponent<Animator>().SetTrigger("TurretGroundEffect");
    }
}
