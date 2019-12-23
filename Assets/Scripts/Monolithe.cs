using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monolithe : MonoBehaviour
{
    public int lifePoints;
    private int lifeMax;
    [SerializeField] private Image lifePointsBar;
    Animator animator;
    [SerializeField] CameraActions mainCameraActions;
    // Start is called before the first frame update

    void Start()
    {
        lifeMax = lifePoints;
        animator = gameObject.GetComponent<Animator>();
        _MGR_Layer.Instance.SetOrderInLayer(gameObject.GetComponent<Renderer>());

        mainCameraActions = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraActions>();
    }

    private void FixedUpdate()
    {
        if (lifePoints <= 0)
        {
            mainCameraActions.followPlayer = false;
            mainCameraActions.FollowTarget(gameObject);
        }
    }

    public void MonolitheEnd()
    {
        _MGR_SoundDesign.Instance.PlaySound("NexusDestruction");
        if (lifePoints <= 0)
        {
            _MGR_SceneManager.Instance.victory = false;
            animator.SetBool("IsDestroyed", true);
            mainCameraActions.followPlayer = false;
            StartCoroutine(WaitToSeeRudysAnimation());
            //StartCoroutine(SlideCameraToNexus());
        }
    }

    public void TakesDamages(float damageAmount)
    {
        lifePoints -= (int)damageAmount;
        lifePointsBar.fillAmount -= damageAmount / 100;
        if (lifePoints <= 0)
        {
            MonolitheEnd();
        }
    }

    IEnumerator WaitToSeeRudysAnimation()
    {
        yield return new WaitForSeconds(3f);
        _MGR_SceneManager.Instance.LoadScene("End");
    }

    IEnumerator SlideCameraToNexus()
    {
        yield return new WaitForSeconds(0.05f);
    }
}
