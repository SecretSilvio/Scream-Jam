using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RubEyes : MonoBehaviour
{
    public RectTransform HandsPanel;
    public GameObject RHandUp;
    public GameObject LHandUp;
    public GameObject RHandDown;
    public GameObject LHandDown;

    private float toggleInterval = 0.5f;
    private float timer = 0f;
    private float lerpSpeed = 0.25f;
    private Vector2 hiddenPos = new Vector2(0, -1000);
    private Vector2 visiblePos = new Vector2(0, 0);
    private Vector2 targetPos;
    private bool handsUp = true;

    void Start()
    {
        targetPos = hiddenPos;
        UpdateHandPositions();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            targetPos = visiblePos;
            timer += Time.deltaTime;

            if (timer >= toggleInterval)
            {
                timer = 0f;
                handsUp = !handsUp;
                UpdateHandPositions();
            }
        }
        else
        {
            targetPos = hiddenPos;
            // Reset timer and hands to default when Space is released
            timer = 0f;
            handsUp = true;
            UpdateHandPositions();
        }

        HandsPanel.anchoredPosition = Vector2.Lerp(HandsPanel.anchoredPosition, targetPos, lerpSpeed);
    }

    private void UpdateHandPositions()
    {
        RHandUp.SetActive(handsUp);
        LHandUp.SetActive(handsUp);
        RHandDown.SetActive(!handsUp);
        LHandDown.SetActive(!handsUp);
    }
}
