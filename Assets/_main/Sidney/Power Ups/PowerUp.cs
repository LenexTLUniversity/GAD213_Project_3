using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUp", order = 1)]
public class PowerUp : ScriptableObject
{
    public string powerUpName; // Name of the powerup
    public float duration; // Duration of the effect in seconds
    public float effectValue; // Magnitude of the effect
}
