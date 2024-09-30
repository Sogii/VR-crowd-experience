using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance { get; private set; }

    [SerializeField] private GameObject objGameOverScreen;
    [SerializeField] private GameObject objObstacleManager;
    [SerializeField] private GameObject objCoinSpawner;
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

    // Update is called once per frame
    private IEnumerator StartGame()
    {
        Debug.Log("Starting Game");
        yield return new WaitForSeconds(3);
        objPlayerShip.SetActive(true);
        objScoreManager.Resume();
        objGameTimer.Resume();
        ClearGame();
    }

    public void StopGame()
    {
        objGameOverScreen.SetActive(true);
        objPlayerShip.SetActive(false);
        objScoreManager.Pause();
        objGameTimer.Pause();
    }

    public void RestartGame()
    {
        objGameOverScreen.SetActive(false);
        objPlayerShip.transform.position = resetPlayerPos;
        objPlayerShip.transform.rotation = Quaternion.Euler(0, 0, 0);
        objScoreManager.ResetGameScore();
        objGameTimer.ResetTimer();
        StartCoroutine(StartGame());
    }

    private void ClearGame()
    {
        GameObject[] coinsToClear = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] enemiesToClear = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in coinsToClear)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in enemiesToClear)
        {
            Destroy(obj);
        }
    }
}
