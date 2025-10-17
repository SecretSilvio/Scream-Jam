using UnityEngine;

public class FootstepsSound : MonoBehaviour
{

public AudioSource footstepsSource;
    public AudioClip[] footstepClips;
    public float stepInterval = 0.4f; // seconds between steps
    public float pitchVariation = 0.1f;
    public float volumeVariation = 0.1f;

    private float stepTimer = 0f;
    private bool isMoving;

    void Update()
    {
        // Check for movement input
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                   Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;

            // When enough time passes, play one step
            if (stepTimer <= 0f)
            {
                footstepsSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
                footstepsSource.volume = 1f - Random.Range(0f, volumeVariation);

                AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                footstepsSource.PlayOneShot(clip);

                // Reset timer for next step
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer when not moving
            stepTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        footstepsSource.Stop();
        }
    }
}