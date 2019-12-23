using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActions : MonoBehaviour
{

    Player player;
    public bool followPlayer;
    public float cameraSlideSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        followPlayer = true;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            FollowTarget(player.gameObject);
        }
    }

    public void FollowTarget(GameObject target)
    {
        Vector3 finalPosition = new Vector3(target.gameObject.transform.position.x, target.gameObject.transform.position.y, transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, finalPosition, cameraSlideSpeed);
        transform.position = smoothPosition;
    }
}
