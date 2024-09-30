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
        RestartGame();
    }

    // Update is called once per frame
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        objObstacleManager.SetActive(true);
        objCoinSpawner.SetActive(true);
        objPlayerShip.SetActive(true);
    }

    public void RestartGame()
    {
        objGameOverScreen.SetActive(false);
        objObstacleManager.SetActive(false);
        objCoinSpawner.SetActive(false);
        objPlayerShip.SetActive(false);
        objPlayerShip.transform.position = resetPlayerPos;
        objPlayerShip.transform.rotation = Quaternion.Euler(0, 0, 0);
        objScoreManager.ResetGameScore();
        StartCoroutine(StartGame());
    }
}