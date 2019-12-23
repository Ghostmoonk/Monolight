using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_Enemies : MonoBehaviour
{

    private static _MGR_Enemies p_instance = null;
    public static _MGR_Enemies Instance { get { return p_instance; } }

    public List<Enemy> list_enemies = new List<Enemy>();


    void Awake()
    {
        // ===>> Singleton Manager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }
}
