using UnityEngine;
using UnityEngine.UI;

public class HungerAndHPManager : MonoBehaviour
{
    public Slider hungerBarSlider;  // Reference to the hunger bar slider
    public Slider hpBarSlider;      // Reference to the HP bar slider
    public float playerSpeed; // The player's speed (set initially)

    private float maxHunger = 1f;  // Full hunger bar (can be changed if needed)
    private float currentHunger = 1f;  // Start full
    private float maxHP = 1f;  // Full HP
    private float currentHP = 1f;  // Start full HP
    private float hungerDepletionRate = 0.1f; // Rate at which hunger bar depletes
    private float timeSinceLastMeal = 0f;  // Tracks time since last food consumption
    private int hungerDepletionCount = 0;  // Tracks number of hunger depletions
    private int foodEatenCount = 0;  // Tracks the number of food items eaten

    private float speedDecreasePerHPDrop = 0.1f;  // How much speed decreases when HP drops
    private float speedIncreasePerHPGain = 0.1f; // How much speed increases when HP gains

    void Update()
    {
        HandleHungerDepletion();
        UpdateUI();
    }

    void HandleHungerDepletion()
    {
        // Track time since last food consumption
        timeSinceLastMeal += Time.deltaTime;

        // Debug log to check if time is passing correctly
        Debug.Log("Time since last meal: " + timeSinceLastMeal);

        // Deplete hunger if more than 8 seconds without food
        if (timeSinceLastMeal >= 8f)
        {
            Debug.Log("Decreasing hunger after 8 seconds.");
            DecreaseHunger();
            timeSinceLastMeal = 0f;  // Reset time after hunger depletion
        }
    }



    void DecreaseHunger()
    {
        if (currentHunger > 0f)
        {
            currentHunger -= hungerDepletionRate;  // Deplete hunger
            hungerDepletionCount++;

            // Every 2 depletions, reduce HP
            if (hungerDepletionCount >= 2)
            {
                hungerDepletionCount = 0;
                DecreaseHP();
            }
        }

        // Update the hunger bar UI (Ensure this is happening)
        UpdateUI();
    }


    public void IncreaseHunger()
    {
        if (currentHunger < maxHunger)
        {
            currentHunger += 0.5f;  // Each food item increases hunger by 0.5 (adjust as needed)
            foodEatenCount++;

            if (foodEatenCount >= 2) // Every two increases of hunger
            {
                foodEatenCount = 0;
                IncreaseHP();
            }
        }

        // Reset the timer since the player ate
        timeSinceLastMeal = 0f;
    }

    void DecreaseHP()
    {
        if (currentHP > 0f)
        {
            currentHP -= 0.1f;  // Decrease HP when hunger depletes

            // Apply speed reduction based on the HP drop
            playerSpeed = Mathf.Max(playerSpeed - speedDecreasePerHPDrop, 0f); // Ensures speed doesn't go negative

            Debug.Log("HP decreased. Current HP: " + currentHP);
            Debug.Log("Player speed decreased. Current speed: " + playerSpeed);
        }
    }

    void IncreaseHP()
    {
        if (currentHP < maxHP)
        {
            currentHP += 0.1f;  // Adjust the amount of HP gain
            AdjustSpeed();
        }
    }

    void AdjustSpeed()
    {
        // Adjust player speed based on HP
        float speedChange = currentHP * speedIncreasePerHPGain;
        playerSpeed = Mathf.Clamp(playerSpeed + speedChange, playerSpeed, playerSpeed); // Ensure it doesn't exceed the initial speed
    }

    void UpdateUI()
    {
        // Update hunger slider based on current hunger
        hungerBarSlider.value = currentHunger / maxHunger;

        // Update HP slider based on current HP
        hpBarSlider.value = currentHP / maxHP;
    }


}
