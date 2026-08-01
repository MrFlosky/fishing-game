using UnityEngine;

public abstract class FishBehaviour : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected RectTransform containerBounds;
    [SerializeField] protected float smoothSpeed = 3f;

    protected RectTransform rectTransform;
    protected float minY;
    protected float maxY;
    protected float targetY;
    
    private float stateTimer;
    private float currentStateDuration;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        CalculateBounds();
        OnStateReset();
    }

    protected virtual void Update()
    {
        // Smoothly move towards the current target Y
        Vector2 pos = rectTransform.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * smoothSpeed);
        rectTransform.anchoredPosition = pos;

        // Handle decision timer
        stateTimer += Time.deltaTime;
        if (stateTimer >= currentStateDuration)
        {
            stateTimer = 0f;
            OnStateReset();
        }
    }

    /// <summary>
    /// Called whenever it is time for the fish to choose a new target position or action.
    /// Override this in subclasses to change movement behavior.
    /// </summary>
    protected virtual void OnStateReset()
    {
        // Default target pick: random spot within bounds
        targetY = GetRandomYPosition();
        currentStateDuration = GetNextStateDuration();
    }

    /// <summary>
    /// Override to change how long the fish stays in its current movement state.
    /// </summary>
    protected virtual float GetNextStateDuration()
    {
        return Random.Range(0.5f, 2.0f);
    }

    /// <summary>
    /// Helper method to return a valid random Y position within the container track.
    /// </summary>
    protected float GetRandomYPosition()
    {
        return Random.Range(minY, maxY);
    }

    private void CalculateBounds()
    {
        if (containerBounds == null)
        {
            Debug.LogError($"Please assign Container Bounds on {gameObject.name}!");
            return;
        }

        float containerHeight = containerBounds.rect.height;
        float fishHeight = rectTransform.rect.height;

        minY = -(containerHeight / 2f) + (fishHeight / 2f);
        maxY = (containerHeight / 2f) - (fishHeight / 2f);
    }
}
