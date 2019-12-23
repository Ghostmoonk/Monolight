using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestDeposit : MonoBehaviour
{
    Deposit deposit;
    bool tryHarvest;
    [SerializeField] private float harvestCooldown = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        deposit = gameObject.GetComponentInParent<Deposit>();
        tryHarvest = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown(deposit.player.harvestControl) && tryHarvest && deposit.playerAtRange)
        {
            //Si le gisement est plein
            if (deposit.harvestableDeposit)
            {
                //On récupère la ressource
                deposit.ConsumeDeposit();
            }
            else
            {
                Debug.Log("Gisement épuisé...");
            }
            StartCoroutine(HarvestCooldown());
        }
    }

    #region Entre dans la portée du gisement pour le récolter

    private void OnTriggerEnter2D(Collider2D other)
    {
        //On vérifie que le collider est bien celui du joueur
        if (!other.isTrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                deposit.playerAtRange = true;
                //On affiche un élément montrant qu'on peut récolter
                if (deposit.harvestableDeposit)
                {
                    deposit.indicationCanvas.gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion

    #region Sort de la portée du gisement

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                if (deposit.indicationCanvas.gameObject.activeSelf)
                    deposit.indicationCanvas.gameObject.SetActive(false);

                deposit.playerAtRange = false;
            }
        }
    }
    #endregion

    IEnumerator HarvestCooldown()
    {
        _MGR_SoundDesign.Instance.PlaySound("Harvest_Deposit");
        tryHarvest = !tryHarvest;
        yield return new WaitForSeconds(harvestCooldown);
        tryHarvest = !tryHarvest;
    }
}
