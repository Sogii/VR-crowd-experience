using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance { get; private set; }

    [SerializeField] private GameObject objGameOverScreen;
    [SerializeField] private ObstacleManager objObstacleManager;
    [SerializeField] private CoinSpawner objCoinSpawner;
    [SerializeField] private ScoreManager objScoreManager;
    [SerializeField] private GameTimer objGameTimer;
    [SerializeField] private GameObject objPlayerShip;

    private Vector3 resetPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // check for issues
        if (objGameOverScreen == null || objObstacleManager == null || objGameOverScreen == null || objScoreManager == null)
        {
            Debug.LogWarning("GameState is missing an initialised gameobject");
        }
        resetPlayerPos = objPlayerShip.transform.position;

        // start the game
        StopGame();
        objGameOverScreen.SetActive(false);
        RestartGame();
    }

    private IEnumerator StartGame()
    {
        Debug.Log("Starting Game");
        ClearGame();
        yield return new WaitForSeconds(2);
        objPlayerShip.SetActive(true);
        objScoreManager.Resume();
        objGameTimer.Resume();
        objCoinSpawner.Restart();
        objObstacleManager.Restart();
        //Start logging data:
        DataFetcher.Instance.StartDataCollection();
    }

    public void StopGame()
    {
        DataFetcher.Instance.StopDataCollection();
        objGameOverScreen.SetActive(true);
        objPlayerShip.SetActive(false);
        objScoreManager.Pause();
        objGameTimer.Pause();

        //Data logging:

    }

    public void RestartGame()
    {
        // first clear the screen
        objGameOverScreen.SetActive(false);
        objPlayerShip.transform.position = resetPlayerPos;
        objPlayerShip.transform.rotation = Quaternion.Euler(0, 0, 0);
        objScoreManager.ResetGameScore();
        objGameTimer.ResetTimer();
        // after three seconds a new game starts
        StartCoroutine(StartGame());
    }

    private void ClearGame()
    {
        objCoinSpawner.Stop();
        objObstacleManager.Stop();
    }
}
