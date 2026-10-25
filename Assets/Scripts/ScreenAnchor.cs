using UnityEngine;

// Keeps this object pinned to the top left corner of the screen, even when the window size changes
// Runs in the editor too, so you can see where it will sit without pressing Play
[ExecuteAlways]
public class ScreenAnchor : MonoBehaviour
{
    // How far the object sits from the top and left edges of the screen, in world units
    [SerializeField] Vector2 padding = new Vector2(0.3f, 0.3f);

    SpriteRenderer sr;

    // Moves the object so its top left corner lines up with the screen corner, plus the padding
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 size = sr != null ? sr.bounds.extents : Vector3.zero;

        float x = cam.transform.position.x - halfWidth + padding.x + size.x;
        float y = cam.transform.position.y + halfHeight - padding.y - size.y;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}