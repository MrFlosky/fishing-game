using UnityEngine;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform playerBar;
    [SerializeField] private RectTransform fishBar;
    [SerializeField] private Image progressBarUI; // Optional visual progress bar

    [Header("Catch Settings")]
    [Tooltip("Time in seconds the player needs to stay on the fish to catch it.")]
    [SerializeField] private float timeToCatch = 3.0f;
    
    [Tooltip("Rate at which progress drops when NOT on the fish.")]
    [SerializeField] private float progressLossMultiplier = 0.5f;

    [Header("Current Status")]
    private float currentProgress = 0f;
    private bool isFishingActive = true;

    private void Update()
    {
        if (!isFishingActive) return;

        // Check if the player bar and fish bar overlap
        if (IsOverlapping(playerBar, fishBar))
        {
            // Increase progress
            currentProgress += Time.deltaTime;
        }
        else
        {
            // Decrease progress slowly when missing
            currentProgress -= Time.deltaTime * progressLossMultiplier;
        }

        // Clamp progress between 0 and the max time needed
        currentProgress = Mathf.Clamp(currentProgress, 0f, timeToCatch);

        // Update UI Progress Bar fill if assigned
        if (progressBarUI != null)
        {
            progressBarUI.fillAmount = currentProgress / timeToCatch;
        }

        // Check Win/Loss conditions
        if (currentProgress >= timeToCatch)
        {
            OnFishCaught();
        }
    }

    /// <summary>
    /// Checks if two UI RectTransforms overlap along the vertical (Y) axis.
    /// </summary>
    private bool IsOverlapping(RectTransform rectA, RectTransform rectB)
    {
        // Get world corners for both RectTransforms
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];
        
        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);

        // Min and Max Y for rectA
        float minYA = cornersA[0].y;
        float maxYA = cornersA[1].y;

        // Min and Max Y for rectB
        float minYB = cornersB[0].y;
        float maxYB = cornersB[1].y;

        // Overlap condition for Y-axis
        return maxYA >= minYB && minYA <= maxYB;
    }

    private void OnFishCaught()
    {
        isFishingActive = false;
        Debug.Log("Fish Caught!");
        // Add your reward logic or end-minigame sequence here
    }

    public void ResetFishing()
    {
        currentProgress = 0f;
        isFishingActive = true;
    }
}