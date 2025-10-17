using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LostChecker : MonoBehaviour
{
    public GameObject mom;
    public GameObject player;
    private Light spotLight;

    private float currentDistance;
    public float LostThreshold = 20f;
    public float TimeToLose = 5f;

    public Volume volume;
    private Vignette vignette;

    public AudioSource heartbeatSource;

    private Coroutine lostCoroutine;
    private bool isLosing = false;

    void Start()
    {
        mom = FindFirstObjectByType<MomController>().gameObject;
        player = FindFirstObjectByType<PlayerMovement>().gameObject;

        spotLight = mom.GetComponentInChildren<Light>(); // Use UnityEngine.Light not SpotLight (SpotLight is a LightType)

        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    void Update()
    {
        currentDistance = Vector2.Distance(new Vector2(mom.transform.position.x, mom.transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

        if (currentDistance > LostThreshold)
        {
            if (!isLosing)
            {
                lostCoroutine = StartCoroutine(LostCoroutine());
                isLosing = true;
            }

            if (spotLight != null)
                spotLight.intensity = 1000f;

            if (!heartbeatSource.isPlaying)
                heartbeatSource.Play();
        }
        else
        {
            if (isLosing)
            {
                if (lostCoroutine != null)
                    StopCoroutine(lostCoroutine);
                if (vignette != null)
                    vignette.intensity.value = 0f;

                isLosing = false;
            }

            if (spotLight != null)
                spotLight.intensity = 0f;

            if (heartbeatSource.isPlaying)
                heartbeatSource.Stop();
        }
    }

    IEnumerator LostCoroutine()
    {
        float timerElapsed = 0f;

        while (timerElapsed < TimeToLose)
        {
            if (currentDistance <= LostThreshold)
            {
                if (vignette != null)
                    vignette.intensity.value = 0f;

                yield break;
            }

            float t = timerElapsed / TimeToLose;
            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(0f, 1f, t);

            timerElapsed += Time.deltaTime;
            yield return null;
        }

        if (vignette != null)
            vignette.intensity.value = 1f;

        FindFirstObjectByType<SceneTransition>().LoadScene("GameOver");
    }
}
