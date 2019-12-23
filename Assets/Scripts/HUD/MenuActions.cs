using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public void LaunchGame()
    {
        _MGR_SoundDesign.Instance.PlaySound("Menu_Check");
        _MGR_SceneManager.Instance.LoadScene("Main");
        Debug.Log("Game has been launched");
    }
    public void ExitGame()
    {
        _MGR_SoundDesign.Instance.PlaySound("Menu_Check");
        Application.Quit();
    }
}
