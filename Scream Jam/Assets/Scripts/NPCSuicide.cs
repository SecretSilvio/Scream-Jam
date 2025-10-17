using UnityEngine;

public class NPCSuicide : MonoBehaviour
{
    public float checkInterval = 5f;
    public float minMoveDistance = 3f;
    public float stickTimeThreshold = 5f;

    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
        InvokeRepeating(nameof(CheckIfStuck), checkInterval, checkInterval);
    }

    void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        if (distanceMoved < minMoveDistance)
        {
            stuckTimer += checkInterval;
            if (stuckTimer >= stickTimeThreshold)
            {
                Debug.Log($"{gameObject.name} is stuck and will be destroyed.");
                Destroy(gameObject);
            }
        }
        else
        {
            stuckTimer = 0f; // Reset timer if moved enough
        }
        lastPosition = transform.position;
    }
}
