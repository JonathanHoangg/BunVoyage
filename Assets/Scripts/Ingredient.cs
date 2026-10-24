using UnityEngine;

// Every kind of ingredient the game uses
public enum IngredientType { BottomBun, Patty, Cheese, Lettuce, Tomato, TopBun }

// Stores what kind of ingredient this is and how thick it is on the stack
// Deletes the ingredient if it falls below the bottom of the screen without being caught
public class Ingredient : MonoBehaviour
{
    // What this ingredient is and how much height it adds to the burger
    // Leave Stack Height at 0 to use the full height of the sprite
    [SerializeField] IngredientType type;
    [SerializeField] float stackHeight = 0f;

    public IngredientType Type => type;
    public bool IsStacked { get; set; }

    float killY;

    // Finds the Y position just below the bottom of the camera view
    void Start()
    {
        Camera cam = Camera.main;
        killY = cam.transform.position.y - cam.orthographicSize - 1f;
    }

    // Deletes the ingredient once it falls off screen, unless it is part of the burger
    void Update()
    {
        if (!IsStacked && transform.position.y < killY)
        {
            Destroy(gameObject);
        }
    }

    // Returns how tall this layer is when stacked on the burger
    public float GetStackHeight()
    {
        if (stackHeight > 0f) return stackHeight;
        return GetComponent<SpriteRenderer>().sprite.bounds.size.y;
    }
}