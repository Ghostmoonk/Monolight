using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour {

    public Image fadeColor;

    public AnimationCurve curve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }


    //Ecran noir => Scene
    IEnumerator FadeIn()
    {
        float t = 1f;
        while(t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = curve.Evaluate(t);
            fadeColor.color = new Color(0f, 0f , 0f , alpha);
            yield return 0;
        }
    }
    //Scene => Ecran noir
    IEnumerator FadeOut(string scene)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float alpha = curve.Evaluate(t);
            fadeColor.color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }

        //Se lit que quand ce qui est au dessus est fini
        SceneManager.LoadScene(scene);
    }

}
