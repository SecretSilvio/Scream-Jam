using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class LostChecker : MonoBehaviour
{
    public GameObject mom;
    public GameObject player;

    private float currentDistance;
    public float LostThreshold = 20f;
    public float TimeToLose = 5f;

    public Volume volume;
    private Vignette vignette;

    public AudioSource heartbeatSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mom = FindFirstObjectByType<MomController>().gameObject;
        player = FindFirstObjectByType<PlayerMovement>().gameObject;

        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = Vector2.Distance(new Vector2(mom.transform.position.x, mom.transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
        if (currentDistance > LostThreshold)
        {
            StartCoroutine(LostCoroutine());
            if (!heartbeatSource.isPlaying)
            {
                heartbeatSource.Play();
            }
        }
        else
        {
            if (heartbeatSource.isPlaying)
            {
                heartbeatSource.Stop();
            }
        }
    }

    public IEnumerator LostCoroutine()
    {
        float timerElapsed = 0f;
        while (timerElapsed < TimeToLose)
        {
            if (currentDistance <= LostThreshold)
            {
                if (vignette != null)
                {
                    vignette.intensity.value = 0f;
                }
                yield break; // Exit the coroutine if the player is back within the threshold
            }
            float t = timerElapsed / TimeToLose;
            vignette.intensity.value = Mathf.Lerp(0, 1, t);

            timerElapsed += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = 1; // Ensure it ends exactly at target
        FindFirstObjectByType<SceneTransition>().LoadScene("GameOver");
    }
}
