using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance { get; private set; }

    [SerializeField] private float intTimerResetTime = 60f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text playingTimeText;

    private float intTimer;
    private bool isPaused = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        intTimer = intTimerResetTime;
    }

    // Update is called once per frame
    public float timeStamp = 0;
    void Update()
    {
        //Debug.Log(intTimer + "  |  " + isPaused);
        if (!isPaused)
        {
            
            timeStamp += Time.deltaTime;
            playingTimeText.text = timeStamp.ToString("F1");
            if (intTimer > 0)
            {
                intTimer -= Time.deltaTime;
                timerText.text = intTimer.ToString("F1") + " sec";
            }
            else
            {
                GameState.instance.StopGame();
                Debug.Log("Time ran out. Game stopped.");
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        Debug.Log("resume");
        isPaused = false;
    }

    public void ResetTimer()
    {
        intTimer = intTimerResetTime;
    }
}
