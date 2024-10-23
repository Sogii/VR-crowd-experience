using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataFetcher : MonoBehaviour
{

    public static DataFetcher Instance;
    public ScoreManager scoreManager;
    public GameTimer gameTimer;
    public float CollectionInterval = 0.5f;

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
        InnitiateObjectScructures();
    }

    void Start()
    {

    }

    #region ObjectArrays
    //Objects In Scene
    public GameObject[] CoinsOnMap;
    public List<GameObject> AsteroidsOnMap { get; private set; } = new List<GameObject>();
    public GameObject PlayerShip;
    private void InnitiateObjectScructures()
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
    private List<GameCaptureData> _gameCaptureData = new List<GameCaptureData>();

    public void StartDataCollection()
    {
        _startTime = Time.time;
        NotHitTimer = 0.0f;
        _gameCaptureData.Add(CollectData());
        StartCoroutine(DataCollectionRoutine());
    }

    public void StopDataCollection()
    {
        Debug.Log("Data Collection Stopped");
        StopCoroutine(DataCollectionRoutine());
        string folderPath = Path.Combine(Application.dataPath, "Resources", "CSVFiles");
        //combine path with filename
        folderPath = Path.Combine(folderPath, _sequenceID.ToString() + ".csv");
        CSVParser.ParseGameDataIntoCSV(_gameCaptureData, folderPath);
    }

    IEnumerator DataCollectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CollectionInterval);
            _gameCaptureData.Add(CollectData());
        }
    }

    #region DataCollectionVariables
    private string _sequenceID;
    private float _startTime;
    public int TimeStepCoinsCollected;
    public int TimeStepCoinNearMiss;
    public int TimeStepHitByAsteroids;
    public int TotalHitByAsteroids;
    public int TimeStepAsteroidNearMiss;
    public int TotalAsteroidNearMiss;
    public int TotalCoinNearmisses;
    public int TotalCoinCollected;
    #endregion
    public GameCaptureData CollectData()
    {
        GameCaptureData collectedData = new GameCaptureData();

        //Identifiers
        collectedData.SequenceID = _sequenceID;
        collectedData.TimeStamp = Time.time;
        collectedData.RelativeTime = gameTimer.timeStamp;
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
        TotalCoinCollected += TimeStepCoinsCollected;
        collectedData.TotalCoinCollected = TotalCoinCollected;
        TimeStepCoinsCollected = 0;
        collectedData.TimeStepCoinNearMiss = TimeStepCoinNearMiss;
        TotalCoinNearmisses += TimeStepCoinNearMiss;
        collectedData.TotalCoinNearmisses = TotalCoinNearmisses;
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
        TotalHitByAsteroids += TimeStepHitByAsteroids;                          //Adjust total hit by asteroids
        TimeStepHitByAsteroids = 0;
        collectedData.TimeStepAsteroidNearMiss = TimeStepAsteroidNearMiss;
        TotalAsteroidNearMiss += TimeStepAsteroidNearMiss;                      //Adjust total asteroid near miss
        TimeStepAsteroidNearMiss = 0;
        //Score
        collectedData.ScoreCount = scoreManager.score;
        collectedData.MultiplierAmount = scoreManager._multiplier;

        //Ship data
        collectedData.ShipOrientation = PlayerShip.transform.rotation.eulerAngles.z;
        collectedData.PlayerInputCount = PlayerShip.GetComponent<SpaceshipController>().PlayerInputCount;
        PlayerShip.GetComponent<SpaceshipController>().PlayerInputCount = 0;
        collectedData.ShipDistanceTravelled = PlayerShip.GetComponent<SpaceshipController>().TotalDistanceTraveled;
        PlayerShip.GetComponent<SpaceshipController>().TotalDistanceTraveled = 0;

        //Time based data
        collectedData.TimeWithoutHit = NotHitTimer;
        //Score
        collectedData.VeryRecentScoreDifference = scoreManager.score - scoreManager.GetEntryFromTime(5).Score;
        collectedData.RecentScoreDifference = scoreManager.score - scoreManager.GetEntryFromTime(12).Score;
        collectedData.LongTermScoreDifference = scoreManager.score - scoreManager.GetEntryFromTime(30).Score;
        //Multiplier
        collectedData.VeryRecentMultiplierDifference = scoreManager._multiplier - scoreManager.GetEntryFromTime(5).Multiplier;
        collectedData.RecentMultiplierDifference = scoreManager._multiplier - scoreManager.GetEntryFromTime(12).Multiplier;
        collectedData.LongTermMultiplierDifference = scoreManager._multiplier - scoreManager.GetEntryFromTime(30).Multiplier;
        //Total Amounts
        collectedData.TotalAsteroidNearMisses = TotalAsteroidNearMiss;
        collectedData.TotalAsteroidHits = TotalHitByAsteroids;

        //First 30 seconds
        //Asteroid hits
        List<TimestampedScore> entries = scoreManager.GetAllFirstSecondsEntries(30);
        int hits = 0;
        foreach (var entry in entries)
        {
            hits += entry.TimeStepTimesHitByAsteroids;
        }
        collectedData.First30SecondsAsteroidHits = hits;
        //Asteroid near misses
        int nearMisses = 0;
        foreach (var entry in entries)
        {
            nearMisses += entry.TimeStepAsteroidNearMiss;
        }
        collectedData.First30SecondsAsteroidHits = nearMisses;
        //Coin collection
        int coinsCollected = 0;
        foreach (var entry in entries)
        {
            coinsCollected += entry.TimeStepTimesCoinCollected;
        }
        collectedData.First30SecondsCoinsCollected = coinsCollected;
        //Coin near misses
        int coinNearMisses = 0;
        foreach (var entry in entries)
        {
            coinNearMisses += entry.TimeStepCoinNearMiss;
        }
        // collectedData.First30SecondsCoinsCollected = coinNearMisses;

        return collectedData;
    }

    #region DataCollectionUtilityFunctions

    void Update()
    {
        NotHitTimer += Time.deltaTime;
    }

    public float NotHitTimer = 0.0f;

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
        foreach (GameObject asteroid in AsteroidsOnMap)
        {
            asteroidDistances.Add(asteroid.GetComponent<AsteroidBehavior>().CalculateDistanceToPlayer(PlayerShip));
        }
        asteroidDistances.Sort();

        if (asteroidDistances.Count < 5)
        {
            return asteroidDistances;
        }
        else
        {
            return asteroidDistances.GetRange(0, 5);
        }


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
public struct GameCaptureData
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
    public float VeryRecentScoreDifference;
    public float RecentScoreDifference;
    public float LongTermScoreDifference;
    public float VeryRecentMultiplierDifference;
    public float RecentMultiplierDifference;
    public float LongTermMultiplierDifference;
    public int TotalAsteroidNearMisses;
    public int TotalAsteroidHits;
    public int TotalCoinCollected;
    public int First30SecondsAsteroidHits;
    public int First30SecondsCoinsCollected;
    public int TotalCoinNearmisses;
}