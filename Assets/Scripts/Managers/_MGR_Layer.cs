using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_Layer : MonoBehaviour
{
    private static _MGR_Layer p_instance = null;
    public static _MGR_Layer Instance { get { return p_instance; } }

    public List<GameObject> fixeContainersObjects;
    public List<GameObject> movingContainersObjects;

    // Update is called once per frame
    private void Awake()
    {
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        //On donne la liste de tous les groupes de GameObjects ne bougeant pas (Turrets, gisements..)
        UpdateOrderInLayer(fixeContainersObjects);
    }
    void Update()
    {
        //On donne la liste de tous les groupes de GameObjects bougeant (Joueur, Ennemis..)
        UpdateOrderInLayer(movingContainersObjects);
    }

    void UpdateOrderInLayer(List<GameObject> containersGroups)
    {

        //On boucle sur tous les container d'objets
        foreach (GameObject container in containersGroups)
        {
            //On boucle sur tous les objets de chaque container
            for (int i = 0; i < container.transform.childCount; i++)
            {
                //Si l'objet a des enfants qui ont un composant de rendu
                if (container.transform.GetChild(i).gameObject.GetComponentsInChildren<Renderer>() != null)
                {
                    //On prend tous les enfants
                    Renderer[] objectWithRenderers = container.transform.GetChild(i).gameObject.GetComponentsInChildren<Renderer>();
                    //Debug.Log("Tableau de renderer :" + objectWithRenderers.Length);
                    for (int k = 0; k < objectWithRenderers.Length; k++)
                    {
                        objectWithRenderers[k].sortingOrder = (int)Mathf.RoundToInt(-container.transform.GetChild(i).position.y * 100);
                    }
                }
            }
        }
    }

    public void SetOrderInLayer(Renderer renderer)
    {
        renderer.sortingOrder = (int)Mathf.RoundToInt(-renderer.transform.position.y * 100);
    }
}
