using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class PlaneBoundsUtilities
{
    public static Vector2[] GetPlaneCorners(GameObject plane)
    {
        Vector2[] corners = new Vector2[4];
        Transform transform = plane.transform;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 scale = new Vector2(transform.localScale.x, transform.localScale.y);
        Quaternion rotation = transform.rotation;

        // Calculate the half extents
        Vector2 halfExtents = new Vector2(scale.x / 2, scale.y / 2);

        // Calculate the corners
        corners[0] = position + (Vector2)(rotation * new Vector2(-halfExtents.x, -halfExtents.y)); // Bottom-left
        corners[1] = position + (Vector2)(rotation * new Vector2(halfExtents.x, -halfExtents.y));  // Bottom-right
        corners[2] = position + (Vector2)(rotation * new Vector2(halfExtents.x, halfExtents.y));   // Top-right
        corners[3] = position + (Vector2)(rotation * new Vector2(-halfExtents.x, halfExtents.y));  // Top-left

        return corners;
    }
}
