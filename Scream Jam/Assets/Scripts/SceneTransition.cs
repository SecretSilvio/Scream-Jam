using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    public IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = fadeImage.color;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
    }

    public IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float time = 0f;
        Color color = fadeImage.color;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneName);
    }
}
