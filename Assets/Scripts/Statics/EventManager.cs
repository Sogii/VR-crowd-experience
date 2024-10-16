using System;
using UnityEngine;

public static class EventManager
{
    // Define events
    public static event Action OnPlayerHit;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnCoinCountChanged;
    public static event Action<float> OnDifficultyChanged;
    public static event Action<float> OnMultiplierChanged;

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

    //Method that triggers coin count
    public static void CoinCountChanged(int amount)
    {
        OnCoinCountChanged?.Invoke(amount);
    }

    //Method that passes through the difficulty based on scoreMultiplier
    public static void DifficultyChanged(float scoreMultiplier)
    {
        OnDifficultyChanged?.Invoke(scoreMultiplier);
    }

    public static void MultiplierChanged(float multiplier)
    {
        OnMultiplierChanged?.Invoke(multiplier);
    }
}