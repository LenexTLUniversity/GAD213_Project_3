using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public PowerUp powerUp; // Assign a PowerUp ScriptableObject in the Inspector


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
            if (collision.CompareTag("Player"))
            {
                HungerAndHPManager hungerAndHPManager = collision.GetComponent<HungerAndHPManager>();
                if (hungerAndHPManager != null)
                {
                    hungerAndHPManager.IncreaseHunger(); 
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Collision with a non-player object.");
        }
    }
}
