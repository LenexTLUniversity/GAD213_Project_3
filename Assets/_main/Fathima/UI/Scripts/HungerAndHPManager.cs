using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HungerAndHPManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hungerBarSlider;
    public Slider hpBarSlider;

    [Header("Hunger & HP Settings")]
    public float maxHunger = 100f;
    public float maxHP = 100f;
    public float hungerDepletionRate = 10f;  
    public float foodRestoreAmount = 20f;

    private float currentHunger;
    private float currentHP;
    private bool isPlayerDead = false;

    void Start()
    {
        currentHunger = maxHunger;
        currentHP = maxHP;
        UpdateUI();
    }

    void Update()
    {
        if (isPlayerDead) return;

        // Smoothly deplete hunger over time
        currentHunger -= hungerDepletionRate * Time.deltaTime;

        if (currentHunger <= 0)
        {
            currentHunger = 0;
            ReduceHP();
        }

        UpdateUI();
    }

    private void ReduceHP()
    {
        if (currentHP > 0)
        {
            currentHP -= 10f;  // Reduce HP when hunger hits 0
            currentHunger = maxHunger;  // Reset hunger
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            isPlayerDead = true;
            ShowGameOverPanel();
        }
    }

    public void IncreaseHunger()
    {
        if (isPlayerDead) return;

        currentHunger = Mathf.Min(currentHunger + foodRestoreAmount, maxHunger);
        UpdateUI();
    }

    private void UpdateUI()
    {
        hungerBarSlider.value = currentHunger / maxHunger;
        hpBarSlider.value = currentHP / maxHP;
    }

    private void ShowGameOverPanel()
    {
        Debug.Log("Game Over! Display Game Over Panel Here.");
        // Implement game-over UI display logic
    }
}

