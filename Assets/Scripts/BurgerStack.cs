using System.Collections.Generic;
using UnityEngine;

// Holds the burger on the plate
// Starts every burger with a bottom bun, snaps caught ingredients into a straight stack,
// and stops accepting ingredients once the top bun is placed
[RequireComponent(typeof(Rigidbody2D))]
public class BurgerStack : MonoBehaviour
{
    // The bottom bun prefab and settings for how layers sit on each other
    [SerializeField] GameObject bottomBunPrefab;
    [SerializeField] float plateSurfaceY = 0.2f;
    [SerializeField] float layerOverlap = 0.05f;

    readonly List<Ingredient> layers = new List<Ingredient>();
    float stackTopY;
    bool isComplete;

    // Lets the trash and delivery zones check the burger
    public bool IsComplete => isComplete;
    public bool HasFillings => layers.Count > 1;
    public IReadOnlyList<Ingredient> Layers => layers;

    // Starts the first burger when the game begins
    void Start()
    {
        ResetStack();
    }

    // Clears the plate and places a fresh bottom bun on it
    public void ResetStack()
    {
        foreach (Ingredient layer in layers)
        {
            if (layer != null) Destroy(layer.gameObject);
        }
        layers.Clear();
        stackTopY = plateSurfaceY;
        isComplete = false;

        GameObject bun = Instantiate(bottomBunPrefab, transform.position, Quaternion.identity);
        AddToStack(bun.GetComponent<Ingredient>());
    }

    // Catches falling ingredients, or lets them fall through once the burger is finished
    void OnCollisionEnter2D(Collision2D collision)
    {
        Ingredient ingredient = collision.gameObject.GetComponent<Ingredient>();
        if (ingredient == null || ingredient.IsStacked) return;

        if (isComplete)
        {
            LetFallThrough(collision.collider);
            return;
        }

        AddToStack(ingredient);
    }

    // Turns off the ingredient's physics, attaches it to the plate, and centers it on top of the stack
    void AddToStack(Ingredient item)
    {
        item.IsStacked = true;

        Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.simulated = false;
            Destroy(itemRb);
        }

        if (item.GetComponent<Collider2D>() == null)
        {
            item.gameObject.AddComponent<BoxCollider2D>();
        }

        SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
        float height = item.GetStackHeight();
        float spriteCenter = sr.sprite.bounds.center.y;

        item.transform.SetParent(transform);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = new Vector3(0f, stackTopY + height / 2f - spriteCenter, 0f);
        sr.sortingOrder = layers.Count + 1;

        stackTopY += height - layerOverlap;
        layers.Add(item);

        if (item.Type == IngredientType.TopBun)
        {
            isComplete = true;
        }
    }

    // Makes an ingredient pass through the finished burger and the plate instead of landing on them
    void LetFallThrough(Collider2D other)
    {
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(other, col);
        }
    }
}