using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AI"))
        {
            playerMovement.FallOver(collision.gameObject.transform);
        }
    }
}
