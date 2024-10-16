using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFetcher : MonoBehaviour
{

    public static DataFetcher Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        _sequenceID = System.Guid.NewGuid().ToString();

    }

    private string _sequenceID;


    private List<GameCaptureData> _gameCaptureData = new List<GameCaptureData>();

    public void CollectData()
    {
        GameCaptureData collectedData = new GameCaptureData();

        //Identifiers
        collectedData.SequenceID = _sequenceID;
        collectedData.TimeStamp = Time.time;

        //Game variables
        //Coins
        


    }

    private void UpdateIdentifiers()
    {

    }
}

struct GameCaptureData
{
    public string SequenceID;
    public float TimeStamp;
    public float CollectableCoin1Distance;
    public float CollectableCoin2Distance;
    public float CollectableCoin3Distance;
    public float CollectableCoin4Distance;
    public float CollectableCoin5Distance;
    public int TimeStepCoinsCollected;
    public int TimeStepCoinNearMiss;
    public float Asteroid1Distance;
    public float Asteroid2Distance;
    public float Asteroid3Distance;
    public float Asteroid4Distance;
    public float Asteroid5Distance;
    public int AsteroidsInCloseRange;
    public int AsteroidsInMediumRange;
    public int AsteroidsOnScreen;
    public int TimeStepHitByAsteroids;
    public int TimeStepAsteroidNearMiss;
    public int ScoreCount;
    public int MultiplierAmount;
    public int ShipOrientation;




}
