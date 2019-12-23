using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMenu : MonoBehaviour
{
    // lance la scene menu
    void Start()        // est appelé apres les awake d'initialisation 
    {
        _MGR_SceneManager.Instance.LoadScene("Menu");
    }
}
