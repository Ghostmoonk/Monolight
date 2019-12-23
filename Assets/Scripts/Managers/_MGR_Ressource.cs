using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _MGR_Ressource : MonoBehaviour
{
    private static _MGR_Ressource p_instance = null;
    public static _MGR_Ressource Instance { get { return p_instance; } }

    public List<Deposit> list_deposits;

    public int crystalsStock;


    #region UI
    [SerializeField] private Text crystalText;

    #endregion
    void Awake()
    {        // ===>> Singleton Manager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);   par nécessaire ici car déja fait par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr
    }

    private void Start()
    {
        crystalText.text = crystalsStock.ToString();
    }

    public void AddCrystals(int amount)
    {
        crystalsStock += amount;
        crystalText.text = crystalsStock.ToString();
    }

    public void RemoveCrystals(int amount)
    {
        crystalsStock -= amount;
        crystalText.text = crystalsStock.ToString();
    }
}
