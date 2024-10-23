using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public static class TSVParser
{
    public static void ParseGameDataIntoTSV(List<GameCaptureData> gameCaptureData, string filePath)
    {
        StringBuilder tsvContent = new StringBuilder();

        // Add TSV header
        tsvContent.AppendLine("SequenceID\tTimeStamp\tRelativeTime\tCollectableCoin1Distance\tCollectableCoin2Distance\tCollectableCoin3Distance\tCollectableCoin4Distance\tCollectableCoin5Distance\tTimeStepCoinsCollected\tTimeStepCoinNearMiss\tAsteroid1Distance\tAsteroid2Distance\tAsteroid3Distance\tAsteroid4Distance\tAsteroid5Distance\tAsteroidsInCloseRange\tAsteroidsInMediumRange\tAsteroidsOnScreen\tTimeStepHitByAsteroids\tTimeStepAsteroidNearMiss\tScoreCount\tMultiplierAmount\tShipOrientation\tPlayerInputCount\tShipDistanceTravelled\tTimeWithoutHit\tVeryRecentScoreDifference\tRecentScoreDifference\tLongTermScoreDifference\tVeryRecentMultiplierDifference\tRecentMultiplierDifference\tLongTermMultiplierDifference\tTotalAsteroidNearMisses\tTotalAsteroidHits\tTotalCoinCollected\tFirst30SecondsAsteroidHits\tFirst30SecondsCoinsCollected\tTotalCoinNearmisses");

        // Add TSV rows
        foreach (var data in gameCaptureData)
        {
            tsvContent.AppendLine($"{data.SequenceID}\t{data.TimeStamp.ToString("F1", CultureInfo.InvariantCulture)}\t{data.RelativeTime.ToString("F1", CultureInfo.InvariantCulture)}\t{data.CollectableCoin1Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.CollectableCoin2Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.CollectableCoin3Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.CollectableCoin4Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.CollectableCoin5Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.TimeStepCoinsCollected}\t{data.TimeStepCoinNearMiss}\t{data.Asteroid1Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.Asteroid2Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.Asteroid3Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.Asteroid4Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.Asteroid5Distance.ToString("F1", CultureInfo.InvariantCulture)}\t{data.AsteroidsInCloseRange}\t{data.AsteroidsInMediumRange}\t{data.AsteroidsOnScreen}\t{data.TimeStepHitByAsteroids}\t{data.TimeStepAsteroidNearMiss}\t{data.ScoreCount.ToString("F1", CultureInfo.InvariantCulture)}\t{data.MultiplierAmount.ToString("F1", CultureInfo.InvariantCulture)}\t{data.ShipOrientation.ToString("F1", CultureInfo.InvariantCulture)}\t{data.PlayerInputCount}\t{data.ShipDistanceTravelled.ToString("F1", CultureInfo.InvariantCulture)}\t{data.TimeWithoutHit.ToString("F1", CultureInfo.InvariantCulture)}\t{data.VeryRecentScoreDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.RecentScoreDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.LongTermScoreDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.VeryRecentMultiplierDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.RecentMultiplierDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.LongTermMultiplierDifference.ToString("F1", CultureInfo.InvariantCulture)}\t{data.TotalAsteroidNearMisses}\t{data.TotalAsteroidHits}\t{data.TotalCoinCollected}\t{data.First30SecondsAsteroidHits}\t{data.First30SecondsCoinsCollected}\t{data.TotalCoinNearmisses}");
        }

        // Write to file
        File.WriteAllText(filePath, tsvContent.ToString());
    }
}