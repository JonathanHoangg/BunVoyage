using UnityEngine;
using UnityEngine.InputSystem;

// Moves the plate left and right with A/D or the arrow keys and keeps it on screen
[RequireComponent(typeof(Rigidbody2D))]
public class PlateController : MonoBehaviour
{
    // Settings and values the plate uses while moving
    [SerializeField] float speed = 8f;

    Rigidbody2D rb;
    float input;
    float minX, maxX;

    // Sets up the Rigidbody and calculates how far the plate can move before hitting the screen edges
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Camera cam = Camera.main;
        float halfScreen = cam.orthographicSize * cam.aspect;
        float halfPlate = GetComponent<SpriteRenderer>().bounds.extents.x;
        minX = cam.transform.position.x - halfScreen + halfPlate;
        maxX = cam.transform.position.x + halfScreen - halfPlate;
    }

    // Reads the keyboard each frame to decide which direction the plate should move
    void Update()
    {
        input = 0f;
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input += 1f;
    }

    // Moves the plate through the physics system and stops it at the screen edges
    void FixedUpdate()
    {
        float x = rb.position.x + input * speed * Time.fixedDeltaTime;
        x = Mathf.Clamp(x, minX, maxX);
        rb.MovePosition(new Vector2(x, rb.position.y));
    }
}