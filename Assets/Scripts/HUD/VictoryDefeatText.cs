using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryDefeatText : MonoBehaviour
{
    [SerializeField] Text victory;
    [SerializeField] Text defeat;
    // Start is called before the first frame update
    void Start()
    {
        if (_MGR_SceneManager.Instance.victory == true)
        {
            defeat.gameObject.SetActive(false);
        }
        else victory.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
