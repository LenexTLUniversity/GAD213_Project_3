using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private float defaultSpeed;
    private float defaultJumpPower;

    private Dictionary<string, Coroutine> activePowerUps = new Dictionary<string, Coroutine>();
    private int dashCount = 0;

    // UI elements for radial sliders
    public Slider speedBoostSlider;  // Slider for Speed Boost
    public Slider jumpBoostSlider;   // Slider for Jump Boost

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the player!");
            return;
        }

        // Save default values from PlayerMovement
        defaultSpeed = playerMovement.speed;
        defaultJumpPower = playerMovement.jumpingPower;

        // Initialize sliders to empty (0)
        if (speedBoostSlider != null) speedBoostSlider.value = 0f;
        if (jumpBoostSlider != null) jumpBoostSlider.value = 0f;
    }

    public void ApplyPowerUp(PowerUp powerUp)
    {
        Debug.Log($"Applying Power-Up: {powerUp.powerUpName} (Duration: {powerUp.duration}, Effect: {powerUp.effectValue})");

        switch (powerUp.powerUpName)
        {
            case "Milk":
                ResetOrStartPowerUp(
                    "Milk",
                    powerUp.duration,
                    () => playerMovement.jumpingPower = defaultJumpPower + powerUp.effectValue,
                    () => playerMovement.jumpingPower = defaultJumpPower,
                    jumpBoostSlider
                );
                break;

            case "Chilly Pepper":
                ResetOrStartPowerUp(
                    "Chilly Pepper",
                    powerUp.duration,
                    () => playerMovement.speed = defaultSpeed + powerUp.effectValue,
                    () => playerMovement.speed = defaultSpeed,
                    speedBoostSlider
                );
                break;

            case "Beans":
                dashCount++;
                Debug.Log($"Dash count increased! Current count: {dashCount}");
                break;

            default:
                Debug.LogWarning("Unknown power-up applied!");
                break;
        }
    }

    private void ResetOrStartPowerUp(string powerUpName, float duration, System.Action applyEffect, System.Action removeEffect, Slider timerSlider)
    {
        // Debug slider reference
        if (timerSlider == null)
        {
            Debug.LogWarning($"No slider assigned for {powerUpName}. Skipping UI update.");
        }
        else
        {
            timerSlider.value = 1f;  // Set slider to full
            Debug.Log($"Filling slider for {powerUpName}");
        }

        // If power-up is already active, reset its timer
        if (activePowerUps.ContainsKey(powerUpName))
        {
            StopCoroutine(activePowerUps[powerUpName]);
            activePowerUps.Remove(powerUpName);
        }

        applyEffect.Invoke(); // Apply the power-up effect
        activePowerUps[powerUpName] = StartCoroutine(PowerUpDuration(powerUpName, duration, removeEffect, timerSlider));
    }

    private IEnumerator PowerUpDuration(string powerUpName, float duration, System.Action removeEffect, Slider timerSlider)
    {
        float timer = duration;
        while (timer > 0)
        {
            if (timerSlider != null)
            {
                timerSlider.value = timer / duration;  // Update slider value
            }

            timer -= Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Revert the effect when the timer ends
        removeEffect.Invoke(); // Revert to default state
        activePowerUps.Remove(powerUpName);

        if (timerSlider != null)
        {
            timerSlider.value = 0f;  // Empty the slider
        }
    }

    // Method to get the current dash count
    public int GetDashCount()
    {
        return dashCount;
    }

    // Method to reduce the dash count when a dash is used
    public void ReduceDashCount()
    {
        if (dashCount > 0)
        {
            dashCount--;
            Debug.Log($"Dash used! Remaining dashes: {dashCount}");
        }
        else
        {
            Debug.Log("No dashes left!");
        }
    }
}
