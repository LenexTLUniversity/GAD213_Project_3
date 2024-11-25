using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [Header("Timer Reference")]
    [SerializeField] private StopwatchTimer stopwatchTimer; // Reference to the timer script

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player"; // Tag used to identify the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the correct tag
        if (collision.CompareTag(playerTag))
        {
            // Stop the timer
            stopwatchTimer.StopTimer();

            // Optionally, log the final time
            Debug.Log("Player reached the end! Final time: " + FormatTime(stopwatchTimer.GetElapsedTime()));
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time % 1) * 1000);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
