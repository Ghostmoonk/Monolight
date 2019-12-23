using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraRestriction : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.tag == "edge")
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
