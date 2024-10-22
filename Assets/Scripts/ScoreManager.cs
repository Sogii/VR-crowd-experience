using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    //Score data
    public float score = 0;
    public float _multiplier { get; private set; } = 1;
    private float _multiplierGrowthRate = 0.01f;
    public int coinsCollected = 0;
    public float difficultyMultiplier = 1;

    //Data fetching data
    public int TimesHitByAsteroids = 0;
    public int TimeStepTimesHitByAsteroids = 0;
    public int TimesCoinCollected = 0;
    public int TimeStepCoinsCollected = 0;
    public int TimeStepAsteroidNearMiss = 0;
    public int TimeStepCoinNearMiss = 0;
    public TimestampedScore defaultScore;

    [SerializeField] private float timeIncrementInterval = 1f; // Time interval for incrementing score in seconds
    [SerializeField] private float timeScoreIncrement = 1f; // Amount to increase score by each interval
    [Tooltip("Drag in the corresponding UI element to show the score.")]
    [SerializeField] private TMP_Text coinTextObject;
    [SerializeField] private TMP_Text multiplierTextObject;

    private float timer = 0f;           // Timer to track the interval
    private bool isPaused = true;

    private void HandleScoreChange(int scoreAmount)
    {
        // Handle score reduced logic
        score += scoreAmount * _multiplier;
    }

    // Update is called once per frame
    void Awake()
    {
        //Add empty timestampscore to the queue
        defaultScore = new TimestampedScore(Time.time, 0, 1, 0, 0, 0, 0);
    }
    void Update()
    {

        if (!isPaused)
        {
            // Increment timer with the time passed since the last frame
            timer += Time.deltaTime;
            ManageQueues();

            // If the timer reaches or exceeds the increment interval
            if (timer >= timeIncrementInterval)
            {
                _multiplier += _multiplierGrowthRate;

                //Passes on the new multiplier to the event manager
                EventManager.DifficultyChanged(_multiplier);

                score += timeScoreIncrement * _multiplier;    // Increase score by the increment amount
                timer = 0f;                 // Reset the timer
            }
            coinTextObject.text = string.Format("{0:N0}", score);
            multiplierTextObject.text = string.Format("X{0:N1}", _multiplier);
        }
    }

    private Queue<TimestampedScore> scoreQueue = new Queue<TimestampedScore>();
    private float scoreRetentionTime = 60f; // Retain scores for the last 30 seconds
    private float lastEnqueueTime = 0f; // Time when the last score was enqueued
    private float enqueueInterval = 0.1f; // Minimum interval between enqueued scores

    private void ManageQueues()
    {
        float currentTime = Time.time;

        // Enqueue a new score if the interval has passed
        if (currentTime - lastEnqueueTime >= enqueueInterval)
        {
            scoreQueue.Enqueue(new TimestampedScore(currentTime, score, _multiplier, TimeStepTimesHitByAsteroids, TimeStepCoinsCollected, TimeStepAsteroidNearMiss, TimeStepCoinNearMiss));
            TimeStepTimesHitByAsteroids = 0;
            TimeStepCoinsCollected = 0;
            TimeStepAsteroidNearMiss = 0;
            TimeStepCoinNearMiss = 0;
            lastEnqueueTime = currentTime;
        }

        // Remove scores older than the retention time
        while (scoreQueue.Count > 0 && scoreQueue.Peek().Timestamp < currentTime - scoreRetentionTime)
        {
            scoreQueue.Dequeue();
        }
    }

    public TimestampedScore GetEntryFromTime(float secondsAgo)
    {
        if (scoreQueue.Count == 0)
        {
            return new TimestampedScore(Time.time, 0, 1, 0, 0, 0, 0);
        }

        float targetTime = Time.time - secondsAgo;
        TimestampedScore closestScore = null;
        TimestampedScore oldestScore = scoreQueue.Peek();
        TimestampedScore newestScore = scoreQueue.Last();

        // Check if the timestamps for the newest and oldest entries are further apart than the secondsAgo value
        if (newestScore.Timestamp - oldestScore.Timestamp < secondsAgo)
        {
            return new TimestampedScore(Time.time, 0, 1, 0, 0, 0, 0);
        }
        foreach (var score in scoreQueue)
        {
            if (oldestScore == null)
            {
                oldestScore = score;
            }

            if (score.Timestamp <= targetTime)
            {
                closestScore = score;
            }
            else
            {
                break;
            }
        }

        if (closestScore != null)
        {
            return closestScore;
        }
        else if (oldestScore != null)
        {
            return oldestScore;
        }
        else
        {
            return defaultScore;
        }
    }

    public List<TimestampedScore> GetAllFirstSecondsEntries(float secondAmount)
    {
        List<TimestampedScore> entries = new List<TimestampedScore>();
        float startTime;
        if (scoreQueue.Count > 0)
        {
            startTime = scoreQueue.Peek().Timestamp; // Assuming each entry has a Time property
        }
        else
        {
            startTime = 0;
        }
        float targetTime = startTime + secondAmount;

        foreach (var score in scoreQueue)
        {
            if (score.Timestamp <= targetTime)
            {
                entries.Add(score);
            }
            else
            {
                break;
            }
        }
        return entries;
    }

    private void OnEnable()
    {
        EventManager.OnPlayerHit += HandlePlayerHit;
        EventManager.OnScoreChanged += HandleScoreChange;
        EventManager.OnMultiplierChanged += HandleMultiplierChange;
        EventManager.OnCoinCollected += HandleCoinCollected;
        EventManager.OnPassThroughNearMiss += HandleNearMiss;
    }

    private void HandleNearMiss(int asteroidNearMisses, int coinNearMisses)
    {
        TimeStepAsteroidNearMiss += asteroidNearMisses;
        TimeStepCoinNearMiss += coinNearMisses;
    }

    private void HandleCoinCollected()
    {
        TimesCoinCollected++;
        TimeStepCoinsCollected++;
    }

    private void HandleMultiplierChange(float multiplierChange)
    {
        _multiplier += multiplierChange;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHit -= HandlePlayerHit;
        EventManager.OnScoreChanged -= HandleScoreChange;
        EventManager.OnMultiplierChanged -= HandleMultiplierChange;
        EventManager.OnCoinCollected -= HandleCoinCollected;
        EventManager.OnPassThroughNearMiss -= HandleNearMiss;
    }

    private void HandlePlayerHit()
    {
        DataFetcher.Instance.NotHitTimer = 0;
        TimesHitByAsteroids++;
        TimeStepTimesHitByAsteroids++;
        _multiplier = (_multiplier * 0.7f) - 1;
        if (_multiplier < 1)
        {
            _multiplier = 1;
        }
        score = (score * 0.8f) - 200;
        Debug.Log("Player was hit!");
    }

    public void ResetGameScore()
    {
        score = 0f;
        _multiplier = 1;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }
}

public class TimestampedScore
{
    public float Timestamp { get; set; }
    public float Score { get; set; }
    public float Multiplier { get; set; }
    public int TimeStepTimesHitByAsteroids { get; set; }
    public int TimeStepTimesCoinCollected { get; set; }
    public int TimeStepAsteroidNearMiss { get; set; }
    public int TimeStepCoinNearMiss { get; set; }

    public TimestampedScore(float timestamp, float score, float multiplier, int timeStepTimesHitByAsteroids, int timeStepTimesCoinCollected, int timeStepAsteroidNearMiss, int timeStepCoinNearMiss)
    {
        Timestamp = timestamp;
        Score = score;
        Multiplier = multiplier;
        TimeStepTimesHitByAsteroids = timeStepTimesHitByAsteroids;
        TimeStepTimesCoinCollected = timeStepTimesCoinCollected;
        TimeStepAsteroidNearMiss = timeStepAsteroidNearMiss;
        TimeStepCoinNearMiss = timeStepCoinNearMiss;
    }
}