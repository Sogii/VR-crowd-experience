using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class CSVParser
{
    public static void ParseGameDataIntoCSV(List<GameCaptureData> gameCaptureData, string filePath)
    {
        StringBuilder csvContent = new StringBuilder();

        // Add CSV header
        csvContent.AppendLine("SequenceID,TimeStamp,RelativeTime,CollectableCoin1Distance,CollectableCoin2Distance,CollectableCoin3Distance,CollectableCoin4Distance,CollectableCoin5Distance,TimeStepCoinsCollected,TimeStepCoinNearMiss,Asteroid1Distance,Asteroid2Distance,Asteroid3Distance,Asteroid4Distance,Asteroid5Distance,AsteroidsInCloseRange,AsteroidsInMediumRange,AsteroidsOnScreen,TimeStepHitByAsteroids,TimeStepAsteroidNearMiss,ScoreCount,MultiplierAmount,ShipOrientation,PlayerInputCount,ShipDistanceTravelled,TimeWithoutHit,VeryRecentScoreDifference,RecentScoreDifference,LongTermScoreDifference,VeryRecentMultiplierDifference,RecentMultiplierDifference,LongTermMultiplierDifference,TotalAsteroidNearMisses,TotalAsteroidHits,TotalCoinCollected,First30SecondsAsteroidHits,First30SecondsCoinsCollected,TotalCoinNearmisses");

        // Add CSV rows
        foreach (var data in gameCaptureData)
        {
            csvContent.AppendLine($"{data.SequenceID},{data.TimeStamp},{data.RelativeTime},{data.CollectableCoin1Distance},{data.CollectableCoin2Distance},{data.CollectableCoin3Distance},{data.CollectableCoin4Distance},{data.CollectableCoin5Distance},{data.TimeStepCoinsCollected},{data.TimeStepCoinNearMiss},{data.Asteroid1Distance},{data.Asteroid2Distance},{data.Asteroid3Distance},{data.Asteroid4Distance},{data.Asteroid5Distance},{data.AsteroidsInCloseRange},{data.AsteroidsInMediumRange},{data.AsteroidsOnScreen},{data.TimeStepHitByAsteroids},{data.TimeStepAsteroidNearMiss},{data.ScoreCount},{data.MultiplierAmount},{data.ShipOrientation},{data.PlayerInputCount},{data.ShipDistanceTravelled},{data.TimeWithoutHit},{data.VeryRecentScoreDifference},{data.RecentScoreDifference},{data.LongTermScoreDifference},{data.VeryRecentMultiplierDifference},{data.RecentMultiplierDifference},{data.LongTermMultiplierDifference},{data.TotalAsteroidNearMisses},{data.TotalAsteroidHits},{data.TotalCoinCollected},{data.First30SecondsAsteroidHits},{data.First30SecondsCoinsCollected},{data.TotalCoinNearmisses}");
        }

        // Write to file
        File.WriteAllText(filePath, csvContent.ToString());
    }
}
