using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public float damages;
    bool attaque;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (gameObject.active == false)
        //{
        //    attaque = false;
        //}
        //else attaque = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponentInParent<Player>().attackActivate == true && other.isTrigger == false)
        {
            if (other.gameObject.tag == "Enemy")
            {

                Enemy target = other.gameObject.GetComponentInParent<Enemy>();
                DealDamages(target);
            }
        }
    }

    private void DealDamages(Enemy target)
    {
        target.TakesDamages((int)damages, gameObject.transform.parent.gameObject);
    }
}
