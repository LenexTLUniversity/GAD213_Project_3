using UnityEngine;
using UnityEngine.UI;

public class HungerAndHPManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hungerBarSlider;   // Reference to the hunger bar slider
    public Slider hpBarSlider;       // Reference to the HP bar slider
    public GameObject gameOverPanel; // Reference to the Game Over Panel

    [Header("Hunger and HP Settings")]
    public float maxHunger = 1f;       // Maximum hunger value
    public float maxHP = 1f;           // Maximum HP value
    public float hungerDepletionRate = 0.1f;  // Speed of hunger depletion (per second)
    public float foodRestoreAmount = 0.2f;    // Amount of hunger restored by food
    public float hpDropAmount = 0.1f;         // Amount of HP lost when hunger depletes fully

    [Header("Player Speed Settings")]
    public float baseSpeed = 15f;       // Starting speed of the player
    public float minSpeed = 2f;        // Minimum speed of the player when HP is low
    private float currentSpeed;        // Current speed of the player

    private float currentHunger;       // Current hunger level
    private float currentHP;           // Current HP level
    private bool isPlayerDead = false; // Flag to check if the player is dead

    void Start()
    {
        // Initialize hunger, HP, and speed
        currentHunger = maxHunger;
        currentHP = maxHP;
        currentSpeed = baseSpeed;

        // Ensure the game-over panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (isPlayerDead) return;

        // Deplete hunger over time
        currentHunger -= hungerDepletionRate * Time.deltaTime;

        if (currentHunger <= 0)
        {
            currentHunger = maxHunger; // Reset hunger bar
            DecreaseHP();             // Decrease HP
        }

        UpdateUI();

        if (currentHP <= 0 && !isPlayerDead)
        {
            PlayerDies();
        }
    }

    public void IncreaseHunger()
    {
        if (isPlayerDead) return;

        currentHunger = Mathf.Min(currentHunger + foodRestoreAmount, maxHunger);
        UpdateUI();
    }

    private void DecreaseHP()
    {
        // Decrease HP and adjust speed
        currentHP -= hpDropAmount;
        AdjustPlayerSpeed();
        Debug.Log("HP decreased. Current HP: " + currentHP);
    }

    private void AdjustPlayerSpeed()
    {
        // Calculate speed based on current HP
        currentSpeed = Mathf.Lerp(minSpeed, baseSpeed, currentHP / maxHP);
        Debug.Log("Player speed adjusted to: " + currentSpeed);

        // Apply the new speed to the player's movement script (if applicable)
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetSpeed(currentSpeed);
        }
    }

    private void PlayerDies()
    {
        isPlayerDead = true;

        // Display the Game Over Panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("Player has died! Game Over!");
    }

    private void UpdateUI()
    {
        hungerBarSlider.value = currentHunger / maxHunger;
        hpBarSlider.value = currentHP / maxHP;
    }
}
