using TMPro;
using UnityEngine;
using System.Collections;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private float fadeTime = 0.5f;

    CanvasGroup canvasGroup;

    public void SetupMessage(string message, bool useAnim = true)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        messageText.text = message;
        if (useAnim)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < 1)
        {
            canvasGroup.alpha = t;
            t += Time.deltaTime * fadeTime;
            yield return null;
        }

        //StartCoroutine(CountdownAndFadeOut());
    }

    IEnumerator CountdownAndFadeOut()
    {
        yield return new WaitForSeconds(destroyTime);

        float t = 1;
        while (t > 0)
        {
            canvasGroup.alpha = t;
            t -= Time.deltaTime * fadeTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}