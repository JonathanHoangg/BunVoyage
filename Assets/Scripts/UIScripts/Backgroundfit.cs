using UnityEngine;

// Scales the background just enough to cover the whole camera view at any screen size without squishing it
// Runs in the editor too, so you can see it fill the view without pressing Play
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFit : MonoBehaviour
{
    SpriteRenderer sr;

    // Centers the background on the camera and grows it until no edge of the screen is left uncovered
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null) return;

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;
        Vector2 spriteSize = sr.sprite.bounds.size;

        float scale = Mathf.Max(screenWidth / spriteSize.x, screenHeight / spriteSize.y);
        transform.localScale = new Vector3(scale, scale, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}