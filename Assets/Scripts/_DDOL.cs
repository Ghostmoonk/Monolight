using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DDOL : MonoBehaviour
{
    //Petit code pour que le manager ne se détruise pas
    void Awake()
    {
        DontDestroyOnLoad(this);
        Debug.developerConsoleVisible = true;
    }
}
