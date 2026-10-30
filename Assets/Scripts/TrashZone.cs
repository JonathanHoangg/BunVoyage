using UnityEngine;

// Clears the burger when the plate slides into the trash area and plays a trash sound
// Always places itself in the bottom left corner of the screen
[ExecuteAlways]
public class TrashZone : MonoBehaviour
{
    // The burger on the plate, the sound player, and how wide the trash area is, in world units
    [SerializeField] BurgerStack burgerStack;
    [SerializeField] SoundManager soundManager;
    [SerializeField] float zoneWidth = 2.5f;

    // The line the plate has to cross
    public float RightEdge { get; private set; }

    SpriteRenderer sr;

    // Places the zone as soon as the game starts
    void Awake()
    {
        PlaceOnScreen();
    }

    // In the editor this keeps the zone in place, and during play it throws away the burger when the plate crosses the line
    void Update()
    {
        if (!Application.isPlaying)
        {
            PlaceOnScreen();
            return;
        }

        if (burgerStack == null) return;

        float plateX = burgerStack.transform.position.x;
        if (plateX <= RightEdge && burgerStack.HasFillings)
        {
            burgerStack.ResetStack();
            if (soundManager != null) soundManager.PlayTrash();
        }
    }

    // Lines the zone up with the left and bottom edges of the screen
    void PlaceOnScreen()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        float halfWidth = cam.orthographicSize * cam.aspect;
        float leftEdge = cam.transform.position.x - halfWidth;
        float bottomEdge = cam.transform.position.y - cam.orthographicSize;
        float halfHeight = sr != null ? sr.bounds.extents.y : 0f;

        RightEdge = leftEdge + zoneWidth;
        transform.position = new Vector3(leftEdge + zoneWidth / 2f, bottomEdge + halfHeight, transform.position.z);
    }
}