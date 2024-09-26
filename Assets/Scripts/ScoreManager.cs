using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public float score = 0;
    private float _multiplier = 1;
    private float _multiplierGrowthRate = 0.05f;
    public int coinsCollected = 0;
    public float difficultyMultiplier = 1;

    [SerializeField] private float timeIncrementInterval = 1f; // Time interval for incrementing score in seconds
    [SerializeField] private float timeScoreIncrement = 1f; // Amount to increase score by each interval
    [Tooltip("Drag in the corresponding UI element to show the score.")]
    [SerializeField] private TMP_Text coinTextObject;
    [SerializeField] private TMP_Text multiplierTextObject;

    private float timer = 0f;           // Timer to track the interval

    private void HandleScoreChange(int scoreAmount)
    {
        // Handle score reduced logic
        score += scoreAmount * _multiplier;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer with the time passed since the last frame
        timer += Time.deltaTime;

        // If the timer reaches or exceeds the increment interval
        if (timer >= timeIncrementInterval)
        {
            _multiplier += _multiplierGrowthRate;
            score += timeScoreIncrement * _multiplier;    // Increase score by the increment amount
            timer = 0f;                 // Reset the timer
        }
        coinTextObject.text = string.Format("{0:N0}", score);
        multiplierTextObject.text = string.Format("X{0:N1}", _multiplier);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerHit += HandlePlayerHit;
        EventManager.OnScoreChanged += HandleScoreChange;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHit -= HandlePlayerHit;
        EventManager.OnScoreChanged -= HandleScoreChange;
    }

    private void HandlePlayerHit()
    {
        _multiplier = 1;
        Debug.Log("Player was hit!");
    }
}
