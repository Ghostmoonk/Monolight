using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _MGR_SceneManager : MonoBehaviour
{
    public string[] arr_SceneName;      // tableau des scenes

    private static _MGR_SceneManager p_instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static _MGR_SceneManager Instance { get { return p_instance; } }

    public bool victory = false;

    void Awake()
    {
        // ===>> SingletonMAnager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);   par nécessaire ici car déja fait  par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //fonction appelant la scene
    public void LoadScene(string __nom_scene)
    {
        SceneManager.LoadScene(__nom_scene);
    }
}
