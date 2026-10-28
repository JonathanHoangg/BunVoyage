using UnityEngine;

// Keeps this UI element just below another UI element, lined up with its right edge
// Runs in the editor too, so you can see where it sits without pressing Play
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class StayBelow : MonoBehaviour
{
    // The UI element to stay under and how much space to leave between them
    [SerializeField] RectTransform target;
    [SerializeField] float gap = 10f;

    RectTransform rect;
    readonly Vector3[] corners = new Vector3[4];

    // Moves this element so its top right corner sits just under the target's bottom right corner
    void LateUpdate()
    {
        if (target == null) return;
        if (rect == null) rect = GetComponent<RectTransform>();

        // Corner 3 is the bottom right corner of the target
        target.GetWorldCorners(corners);
        Vector3 bottomRight = corners[3];

        rect.pivot = new Vector2(1f, 1f);
        float scale = rect.lossyScale.y;
        rect.position = bottomRight + new Vector3(0f, -gap * scale, 0f);
    }
}