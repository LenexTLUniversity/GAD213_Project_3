using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private float defaultSpeed;
    private float defaultJumpPower;

    private Dictionary<string, Coroutine> activePowerUps = new Dictionary<string, Coroutine>();
    private int dashCount = 0;

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
    }

    public void ApplyPowerUp(PowerUp powerUp)
    {
        switch (powerUp.powerUpName)
        {
            case "Milk":
                ResetOrStartPowerUp("Milk", powerUp.duration, () => playerMovement.jumpingPower = defaultJumpPower + powerUp.effectValue, () => playerMovement.jumpingPower = defaultJumpPower);
                break;

            case "Chilly Pepper":
                ResetOrStartPowerUp("Chilly Pepper", powerUp.duration, () => playerMovement.speed = defaultSpeed + powerUp.effectValue, () => playerMovement.speed = defaultSpeed);
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

    private void ResetOrStartPowerUp(string powerUpName, float duration, System.Action applyEffect, System.Action removeEffect)
    {
        // If power-up is already active, reset its timer
        if (activePowerUps.ContainsKey(powerUpName))
        {
            StopCoroutine(activePowerUps[powerUpName]);
            activePowerUps.Remove(powerUpName);
        }

        applyEffect.Invoke(); // Apply the power-up effect
        activePowerUps[powerUpName] = StartCoroutine(PowerUpDuration(powerUpName, duration, removeEffect));
    }

    private IEnumerator PowerUpDuration(string powerUpName, float duration, System.Action removeEffect)
    {
        yield return new WaitForSeconds(duration);

        removeEffect.Invoke(); // Revert to default state
        activePowerUps.Remove(powerUpName);
    }

    public void UseDash()
    {
        if (dashCount > 0)
        {
            dashCount--;
            Debug.Log("Dash used! Remaining dashes: " + dashCount);

            // Determine the dash direction based on the player's facing direction
            float dashForce = 10f;

            // Get the direction the player is facing
            float dashDirection = playerMovement.transform.localScale.x > 0 ? 1f : -1f; // Facing right is positive, left is negative

            Debug.Log($"Dash direction: {dashDirection} (1 = right, -1 = left)");

            // Apply dash velocity
            Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();

            // Ensure dash only modifies horizontal movement (keep current vertical velocity)
            float currentYVelocity = rb.velocity.y;  // Save current vertical velocity
            rb.velocity = new Vector2(dashForce * dashDirection, currentYVelocity);  // Modify horizontal velocity only

            Debug.Log($"Player Velocity after Dash: {rb.velocity}");
        }
        else
        {
            Debug.Log("No dashes left!");
        }
    }
}
