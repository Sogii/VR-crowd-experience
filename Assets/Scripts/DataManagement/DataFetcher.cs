using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFetcher : MonoBehaviour
{

    public static DataFetcher Instance;
    public ScoreManager scoreManager;

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
        InnitiateArrays();
    }

    void Start()
    {
        GameObject gameManagers = GameObject.Find("GameManagers");
        if (gameManagers != null)
        {
            scoreManager = gameManagers.GetComponent<ScoreManager>();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    #region ObjectArrays
    //Objects In Scene
    public GameObject[] CoinsOnMap;
    public List<GameObject> AsteroidsOnMap { get; private set; } = new List<GameObject>();
    public GameObject PlayerShip;

    private void InnitiateArrays()
    {
        CoinsOnMap = new GameObject[5];
        PlayerShip = GameObject.FindGameObjectWithTag("Player");
    }
    public void PopulateCoinsOnMap(GameObject coin)
    {
        for (int i = 0; i < CoinsOnMap.Length; i++)
        {
            if (CoinsOnMap[i] == null)
            {
                CoinsOnMap[i] = coin;
                return;
            }
        }
    }
    public void RemoveCoinFromMap(GameObject coin)
    {
        for (int i = 0; i < CoinsOnMap.Length; i++)
        {
            if (CoinsOnMap[i] == coin)
            {
                CoinsOnMap[i] = null;
                return;
            }
        }
    }

    public void PopulateAsteroidOnMap(GameObject asteroid)
    {
        AsteroidsOnMap.Add(asteroid);
    }

    public void RemoveAsteroidFromMap(GameObject asteroid)
    {
        AsteroidsOnMap.Remove(asteroid);
    }
    #endregion


    //Data collection variables
    private string _sequenceID;
    private float _startTime;
    public int TimeStepCoinsCollected;
    public int TimeStepCoinNearMiss;
    public int TimeStepHitByAsteroids;
    public int TimeStepAsteroidNearMiss;


    private List<GameCaptureData> _gameCaptureData = new List<GameCaptureData>();

    public void StartDataCollection()
    {
        _startTime = Time.time;
    }
    public void CollectData()
    {
        GameCaptureData collectedData = new GameCaptureData();

        //Identifiers
        collectedData.SequenceID = _sequenceID;
        collectedData.TimeStamp = Time.time;
        collectedData.RelativeTime = Time.time - _startTime;


        //Game variables

        //Coins
        //Coin distances
        collectedData.CollectableCoin1Distance = CollectCoinDistance(0);
        collectedData.CollectableCoin2Distance = CollectCoinDistance(1);
        collectedData.CollectableCoin3Distance = CollectCoinDistance(2);
        collectedData.CollectableCoin4Distance = CollectCoinDistance(3);
        collectedData.CollectableCoin5Distance = CollectCoinDistance(4);
        //Coin collection count
        collectedData.TimeStepCoinsCollected = TimeStepCoinsCollected;
        TimeStepCoinsCollected = 0;
        collectedData.TimeStepCoinNearMiss = TimeStepCoinNearMiss;
        TimeStepCoinNearMiss = 0;

        //Asteroids
        //Asteroid distances
        List<float> asteroidDistances = Fetch5ClosestAsteroids();
        collectedData.Asteroid1Distance = asteroidDistances.Count > 0 ? asteroidDistances[0] : -1f;
        collectedData.Asteroid2Distance = asteroidDistances.Count > 1 ? asteroidDistances[1] : -1f;
        collectedData.Asteroid3Distance = asteroidDistances.Count > 2 ? asteroidDistances[2] : -1f;
        collectedData.Asteroid4Distance = asteroidDistances.Count > 3 ? asteroidDistances[3] : -1f;
        collectedData.Asteroid5Distance = asteroidDistances.Count > 4 ? asteroidDistances[4] : -1f;
        //Asteroids in range
        collectedData.AsteroidsInCloseRange = ListAsteroidsWithinRange(2.3f);
        collectedData.AsteroidsInMediumRange = ListAsteroidsWithinRange(4.8f);
        collectedData.AsteroidsOnScreen = AsteroidsOnMap.Count;
        collectedData.TimeStepHitByAsteroids = TimeStepHitByAsteroids;
        TimeStepHitByAsteroids = 0;

        //Score
        collectedData.ScoreCount = scoreManager.score;
        collectedData.MultiplierAmount = scoreManager._multiplier;

        //Ship data
        collectedData.ShipOrientation = PlayerShip.transform.rotation.eulerAngles.z;
        collectedData.PlayerInputCount = PlayerShip.GetComponent<SpaceshipController>().PlayerInputCount;
        PlayerShip.GetComponent<SpaceshipController>().PlayerInputCount = 0;
        







    }

    private void UpdateIdentifiers()
    {

    }

    #region DataCollectionUtilityFunctions

    private float CollectCoinDistance(int coinIndex)
    {
        if (CoinsOnMap[coinIndex] == null)
        {
            return -1;
        }
        else
        {
            return CoinsOnMap[coinIndex].GetComponent<CoinBehavior>().CalculateDistanceToPlayer(PlayerShip);
        }
    }

    private List<float> Fetch5ClosestAsteroids()
    {
        List<float> asteroidDistances = new List<float>();
        List<GameObject> closestAsteroids = new List<GameObject>();
        foreach (GameObject asteroid in AsteroidsOnMap)
        {
            asteroidDistances.Add(asteroid.GetComponent<AsteroidBehavior>().CalculateDistanceToPlayer(PlayerShip));
        }
        asteroidDistances.Sort();
        return asteroidDistances.GetRange(0, 5);

    }

    private int ListAsteroidsWithinRange(float Range)
    {
        int count = 0;
        foreach (GameObject asteroid in AsteroidsOnMap)
        {
            if (asteroid.GetComponent<AsteroidBehavior>().CalculateDistanceToPlayer(PlayerShip) < Range)
            {
                count++;
            }
        }
        return count;
    }

    #endregion
}
struct GameCaptureData
{
    public string SequenceID;
    public float TimeStamp;
    public float RelativeTime;
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
    public float ScoreCount;
    public float MultiplierAmount;
    public float ShipOrientation;
    public int PlayerInputCount;
    public float ShipDistanceTravelled;
    public float TimeWithoutHit;
    public float RecentScoreDifference;
    public float LongTermScoreDifference;
    public float RecentMultiplierDifference;
    public float LongTermMultiplierDifference;




}