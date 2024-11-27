using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public PowerUp powerUp; // Assign a PowerUp ScriptableObject in the Inspector
    public HungerAndHPManager hungerAndHPManager;  // Reference to the HungerAndHPManager script


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision detected with {collision.name}");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collision with Player confirmed.");
            PowerUpHandler playerPowerUpHandler = collision.GetComponent<PowerUpHandler>();
            if (playerPowerUpHandler != null)
            {
                Debug.Log("PowerUpHandler found, applying power-up...");
                playerPowerUpHandler.ApplyPowerUp(powerUp);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PowerUpHandler not found on Player.");
            }
            // Increase the hunger bar when the player eats the food
            if (hungerAndHPManager != null)
            {
                Debug.Log("HungerAndHPManager found, increasing hunger...");
                hungerAndHPManager.IncreaseHunger();  // Increase the hunger bar

                Destroy(gameObject);  // Destroy the food object after consumption
            }
            else
            {
                Debug.LogWarning("HungerAndHPManager not assigned.");
            }
        }
        else
        {
            Debug.Log("Collision with a non-player object.");
        }
    }
}
