using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public float score = 0;
    public int coinsCollected = 0;
    public float difficultyMultiplier = 1;

    [SerializeField] private float timeIncrementInterval = 1f; // Time interval for incrementing score in seconds
    [SerializeField] private float timeScoreIncrement = 1f; // Amount to increase score by each interval
    [Tooltip("Drag in the corresponding UI element to show the score.")]
    [SerializeField] private TMP_Text coinTextObject;
    [SerializeField] private float coinValue = 50;  // How much your score will increase when grabbing a coin

    private float timer = 0f;           // Timer to track the interval

    private void HandleScoreChange(int scoreAmount)
    {
        // Handle score reduced logic
        score += scoreAmount; 
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer with the time passed since the last frame
        timer += Time.deltaTime;

        // If the timer reaches or exceeds the increment interval
        if (timer >= timeIncrementInterval)
        {
            score += timeScoreIncrement;    // Increase score by the increment amount
            timer = 0f;                 // Reset the timer
        }
        coinTextObject.text = string.Format("{0:N0}", score);
    }

    public void CollectCoin()
    {
        score += coinValue;
        coinsCollected++;
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
        // Handle player hit logic
        Debug.Log("Player was hit!");
    }
}
