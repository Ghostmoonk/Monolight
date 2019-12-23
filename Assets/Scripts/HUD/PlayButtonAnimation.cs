using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text playText;


    public void OnPointerEnter(PointerEventData eventData)
    {
        playText.color = Color.black; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playText.color = Color.white; //Or however you do your color
    }
}
