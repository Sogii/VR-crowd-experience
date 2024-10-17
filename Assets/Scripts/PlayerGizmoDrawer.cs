using UnityEngine;

public class PlayerGizmoDrawer : MonoBehaviour
{
    [SerializeField] private float range = 5.0f;
    [SerializeField] private Color gizmoColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}