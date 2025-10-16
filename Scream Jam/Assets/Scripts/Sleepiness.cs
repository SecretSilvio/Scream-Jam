using UnityEngine;
using UnityEngine.UI;

public class Sleepiness : MonoBehaviour
{
    public float sleepinessLevel = 0f; // 0 to 1
    public float sleepinessIncreaseRate = 0.01f; // Rate at which sleepiness increases per second
    public float sleepinessDecreaseRate = 0.05f; // Rate at which sleepiness decreases when resting
    private float visualSleepiness = 0f; // Smoothed sleepiness for visual effects

    public AnimationCurve sleepinessCurve; // Curve to map sleepiness level to effects

    public Image sleepOverlay;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Decrease sleepiness when resting
            sleepinessLevel -= sleepinessDecreaseRate * Time.deltaTime;
        }
        else
        {
            // Increase sleepiness over time
            sleepinessLevel += sleepinessIncreaseRate * Time.deltaTime;
        }
        // Clamp sleepiness level between 0 and 1
        sleepinessLevel = Mathf.Clamp01(sleepinessLevel);

        // Use the curve to get multiplier (also between 0 and 1, shaped by your curve)
        visualSleepiness = sleepinessCurve.Evaluate(sleepinessLevel);;

        // Update overlay transparency based on sleepiness level
        if (sleepOverlay != null)
        {
            Color overlayColor = sleepOverlay.color;
            overlayColor.a = visualSleepiness;
            sleepOverlay.color = overlayColor;
        }
    }
}
