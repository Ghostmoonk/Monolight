using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deposit : MonoBehaviour
{
    public GameObject harvestedDepositedObject;
    public GameObject fullDepositObject;
    [SerializeField] private Collider2D harvestArea;
    [HideInInspector] public Player player;

    public Canvas indicationCanvas;

    [HideInInspector] public bool harvestableDeposit;
    [HideInInspector] public bool playerAtRange;
    [Range(1, 5)]
    public int crystalsQuantity;

    void Start()
    {
        harvestableDeposit = true;
        indicationCanvas.gameObject.SetActive(false);
        _MGR_Ressource.Instance.list_deposits.Add(this);

        fullDepositObject.SetActive(true);
        harvestedDepositedObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (indicationCanvas.sortingOrder <= fullDepositObject.GetComponent<Renderer>().sortingOrder)
        {
            indicationCanvas.sortingOrder = fullDepositObject.GetComponent<Renderer>().sortingOrder + 1;
        }
    }

    public void ConsumeDeposit()
    {
        harvestableDeposit = false;
        _MGR_Ressource.Instance.AddCrystals(crystalsQuantity);
        harvestedDepositedObject.SetActive(true);
        fullDepositObject.SetActive(false);
        indicationCanvas.gameObject.SetActive(false);

        _MGR_Layer.Instance.SetOrderInLayer(harvestedDepositedObject.GetComponent<SpriteRenderer>());
    }

    public void RegenerateDeposit()
    {
        if (!harvestableDeposit)
        {
            harvestableDeposit = true;
            harvestedDepositedObject.SetActive(false);
            fullDepositObject.SetActive(true);
        }
    }

}
