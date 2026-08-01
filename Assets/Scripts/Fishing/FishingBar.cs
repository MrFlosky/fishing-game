using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBar : MonoBehaviour
{
    [SerializeField] private float liftForce = 1200f;
    [SerializeField] private float gravity = 800f;
    [SerializeField] private float maxSpeed = 600f;
    
    [SerializeField] private float bounceDamping = 0.3f; 
    
    [SerializeField] private RectTransform containerBounds;

    private RectTransform rectTransform;
    private float velocity = 0f;
    private float minY;
    private float maxY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        CalculateBounds();
    }

    private void Update()
    {
        HandleInputAndPhysics();
        ClampPositionAndBounce();
    }

    private void HandleInputAndPhysics()
    {
        // Check for click/hold or Space key (adjust input method as needed)
        bool isPressing = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);

        if (isPressing)
        {
            // Apply upward force
            velocity += liftForce * Time.deltaTime;
        }
        else
        {
            // Apply gravity
            velocity -= gravity * Time.deltaTime;
        }

        // Clamp velocity within max speed limits
        velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);

        // Move the bar
        rectTransform.anchoredPosition += new Vector2(0f, velocity * Time.deltaTime);
    }

    private void ClampPositionAndBounce()
    {
        Vector2 pos = rectTransform.anchoredPosition;

        // Hit the top boundary
        if (pos.y > maxY)
        {
            pos.y = maxY;
            velocity = -velocity * bounceDamping; // Bounce downward
        }
        // Hit the bottom boundary
        else if (pos.y < minY)
        {
            pos.y = minY;
            velocity = -velocity * bounceDamping; // Bounce upward
        }

        rectTransform.anchoredPosition = pos;
    }

    private void CalculateBounds()
    {
        if (containerBounds == null)
        {
            Debug.LogError("Please assign the Container Bounds RectTransform in the Inspector!");
            return;
        }

        // Calculate top and bottom limits based on container and bar heights
        float containerHeight = containerBounds.rect.height;
        float barHeight = rectTransform.rect.height;

        // Assumes pivot points are centered (0.5, 0.5)
        minY = -(containerHeight / 2f) + (barHeight / 2f);
        maxY = (containerHeight / 2f) - (barHeight / 2f);
    }
}
