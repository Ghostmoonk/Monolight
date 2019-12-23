using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTurretArea : MonoBehaviour
{
    [SerializeField] private Turret turret;

    [SerializeField] Canvas buildIndicationCanvas;
    [SerializeField] Text buildIndicationText;
    [SerializeField] Text buildIndicationTextBehind;
    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponentInParent<Turret>();
        buildIndicationCanvas.gameObject.SetActive(false);
        buildIndicationText.text = turret.crystalCost.ToString();
        buildIndicationTextBehind.text = turret.crystalCost.ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown(turret.player.buildControl))
        {
            if (turret.playerAtRangeToBuild && _MGR_Ressource.Instance.crystalsStock >= turret.crystalCost && !turret.isBuild)
            {
                _MGR_Ressource.Instance.RemoveCrystals(turret.crystalCost);
                turret.BuildTurret();
                buildIndicationCanvas.gameObject.SetActive(false);
            }
        }
    }

    //Trigger : si le joueur entre dans la zone
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            if (!turret.isBuild)
            {
                //Afficher un indicateur de build possible, et éviter de le spam
                if (!buildIndicationCanvas.gameObject.activeSelf)
                {
                    buildIndicationCanvas.gameObject.SetActive(true);
                    turret.playerAtRangeToBuild = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                buildIndicationCanvas.gameObject.SetActive(false);
                turret.playerAtRangeToBuild = false;
            }
        }
    }
}

