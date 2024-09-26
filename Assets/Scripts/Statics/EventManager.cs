using System;
using UnityEngine;

public static class EventManager
{
    // Define events
    public static event Action OnPlayerHit;
    public static event Action<int> OnScoreChanged;

    // Method to trigger player hit event
    public static void PlayerHit()
    {
        OnPlayerHit?.Invoke();
    }

    // Method to trigger score reduced event
    public static void ScoreChanged(int amount)
    {
        OnScoreChanged?.Invoke(amount);
    }
}