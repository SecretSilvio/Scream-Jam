using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

public class MomIntro : MonoBehaviour
{
    class LightInfo
    {
        public Light light;
        public float initialIntensity;

        public LightInfo(Light light)
        {
            this.light = light;
            initialIntensity = light.intensity;
        }
    }

    public float fadeDuration = 5f;
    private List<LightInfo> lightInfos = new List<LightInfo>();
    public Light momLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Light[] sceneLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in sceneLights)
        {
            if (light == momLight)
            {
                continue;
            }
            LightInfo info = new LightInfo(light);
            light.intensity = 0f;
            lightInfos.Add(info);
        }

        StartCoroutine(FadeInLights());
        StartCoroutine(FadeOutLight());
    }

    public IEnumerator FadeInLights()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            foreach (LightInfo info in lightInfos)
            {
                info.light.intensity = Mathf.Lerp(0f, info.initialIntensity, t);
            }
            yield return null;
        }
        // Ensure all lights are set to their initial intensity at the end
        foreach (LightInfo info in lightInfos)
        {
            info.light.intensity = info.initialIntensity;
        }
    }

    public IEnumerator FadeOutLight()
    {
        float elapsedTime = 0f;
        float initialIntensity = momLight.intensity;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            momLight.intensity = Mathf.Lerp(initialIntensity, 0f, t);
            yield return null;
        }
        // Ensure the light is set to zero intensity at the end
        momLight.intensity = 0f;
    }
}
