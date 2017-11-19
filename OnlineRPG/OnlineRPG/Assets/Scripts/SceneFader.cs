using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float fadeTimeMultiplier = 1f;

    [SerializeField] private bool fadeInThisScene = true;

    void Start()
    {
        if (fadeInThisScene)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime * fadeTimeMultiplier;
            Color colour = image.color;
            colour.a = curve.Evaluate(t);
            image.color = colour;

            yield return null;
        }
    }

    public void Network_FadeTo(string sceneName)
    {
        StartCoroutine(FadeToScene(sceneName, networkLoad: true));
    }

    public void Offline_FadeTo(string sceneName)
    {
        StartCoroutine(FadeToScene(sceneName));
    }

    IEnumerator FadeToScene(string sceneName, bool networkLoad = false)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeTimeMultiplier;
            Color colour = image.color;
            colour.a = curve.Evaluate(t);
            image.color = colour;

            yield return null;
        }

        if (networkLoad)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}