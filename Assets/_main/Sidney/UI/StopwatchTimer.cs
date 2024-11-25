using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopwatchTimer : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text timerText; // Reference to the UI Text element

    private float elapsedTime = 0f;
    private bool timerRunning = false;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Update the UI with the formatted time
        DisplayTime(elapsedTime);
    }

    private void DisplayTime(float timeToDisplay)
    {
        // Format time to show minutes, seconds, and milliseconds
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int milliseconds = Mathf.FloorToInt((timeToDisplay % 1) * 1000);

        // Display the time in the format MM:SS:MS
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
