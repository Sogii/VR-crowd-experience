using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityUtlities
{
    public static float GenerateNormalRandomValue(float min, float max, float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNormal = mean + stdDev * randStdNormal;

        // Clamp the value to ensure it is within the specified range
        return Mathf.Clamp(randNormal, min, max);
    }

    public static float GenerateRightHalfNormalRandomValue(float min, float max, float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // Ensure the value is on the right side of the mean
        if (randStdNormal < 0)
        {
            randStdNormal = -randStdNormal;
        }

        float randNormal = mean + stdDev * randStdNormal;

        // Clamp the value to ensure it is within the specified range
        return Mathf.Clamp(randNormal, min, max);
    }
}


