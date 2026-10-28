using System.Collections.Generic;
using UnityEngine;

// Drops random ingredients from above the screen at a steady interval, sometimes two at once
// Ingredients the current order still needs are more likely to fall, and the next one needed is the most likely
// Ingredients never spawn above the trash area on the left or the delivery area on the right
public class IngredientSpawner : MonoBehaviour
{
    // Ingredients to choose from and how often to drop them
    [SerializeField] GameObject[] ingredientPrefabs;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float edgePadding = 0.7f;

    // The chance each spawn drops two ingredients instead of one, and how far apart a pair must be
    [SerializeField, Range(0f, 1f)] float doubleSpawnChance = 0.3f;
    [SerializeField] float minPairGap = 2f;

    // How much of the screen to keep clear on each side, in world units
    [SerializeField] float trashZoneWidth = 2.5f;
    [SerializeField] float deliveryZoneWidth = 2.5f;

    // How strongly gravity pulls on the ingredients
    [SerializeField] float fallGravity = 0.5f;

    // The order and the burger, used to figure out which ingredients are needed
    [SerializeField] OrderManager orderManager;
    [SerializeField] BurgerStack burgerStack;

    // Every ingredient starts with the base chance, and needed ones get the bonuses added on top
    [SerializeField] float baseWeight = 1f;
    [SerializeField] float stillNeededBonus = 1f;
    [SerializeField] float nextNeededBonus = 2f;

    Ingredient[] prefabIngredients;
    float timer;
    float minX, maxX, spawnY;

    // Reads each prefab's ingredient type and calculates the spawn range
    void Start()
    {
        prefabIngredients = new Ingredient[ingredientPrefabs.Length];
        for (int i = 0; i < ingredientPrefabs.Length; i++)
        {
            prefabIngredients[i] = ingredientPrefabs[i].GetComponent<Ingredient>();
        }

        CalculateSpawnRange();
    }

    // Counts up time and spawns each time the interval passes
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    // Drops one ingredient, and sometimes a second one at the same time
    void Spawn()
    {
        if (ingredientPrefabs.Length == 0) return;

        float firstX = Random.Range(minX, maxX);
        SpawnAt(firstX);

        if (Random.value < doubleSpawnChance)
        {
            SpawnAt(PickSecondX(firstX));
        }
    }

    // Creates a picked ingredient above the screen at this X position and sets how fast it falls
    void SpawnAt(float x)
    {
        GameObject prefab = PickIngredient();
        GameObject ingredient = Instantiate(prefab, new Vector2(x, spawnY), Quaternion.identity);

        Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = fallGravity;
    }

    // Finds a spot for the second ingredient that is far enough from the first one
    float PickSecondX(float firstX)
    {
        for (int tries = 0; tries < 10; tries++)
        {
            float x = Random.Range(minX, maxX);
            if (Mathf.Abs(x - firstX) >= minPairGap) return x;
        }

        // If no good spot was found, use the far side of the spawn range
        return firstX < (minX + maxX) / 2f ? maxX : minX;
    }

    // Gives each ingredient a weight based on whether the order needs it, then picks one at random using those weights
    GameObject PickIngredient()
    {
        float[] weights = new float[ingredientPrefabs.Length];
        float total = 0f;

        for (int i = 0; i < ingredientPrefabs.Length; i++)
        {
            float weight = baseWeight;
            Ingredient ingredient = prefabIngredients[i];

            if (ingredient != null)
            {
                if (IsStillNeeded(ingredient.Type)) weight += stillNeededBonus;
                if (IsNextNeeded(ingredient.Type)) weight += nextNeededBonus;
            }

            weights[i] = weight;
            total += weight;
        }

        // Rolls a number and walks through the weights until it lands on one
        float roll = Random.Range(0f, total);
        for (int i = 0; i < weights.Length; i++)
        {
            if (roll < weights[i]) return ingredientPrefabs[i];
            roll -= weights[i];
        }
        return ingredientPrefabs[ingredientPrefabs.Length - 1];
    }

    // Checks whether this ingredient appears anywhere in the part of the order not built yet
    bool IsStillNeeded(IngredientType type)
    {
        if (orderManager == null || burgerStack == null || burgerStack.IsComplete) return false;

        IReadOnlyList<IngredientType> order = orderManager.CurrentOrder;
        for (int i = burgerStack.Layers.Count; i < order.Count; i++)
        {
            if (order[i] == type) return true;
        }
        return false;
    }

    // Checks whether this ingredient is the very next one the order needs
    bool IsNextNeeded(IngredientType type)
    {
        if (orderManager == null || burgerStack == null || burgerStack.IsComplete) return false;

        IReadOnlyList<IngredientType> order = orderManager.CurrentOrder;
        int nextSpot = burgerStack.Layers.Count;
        return nextSpot < order.Count && order[nextSpot] == type;
    }

    // Finds the left and right spawn limits from the screen edges and the zone widths
    void CalculateSpawnRange()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float leftEdge = cam.transform.position.x - halfWidth;
        float rightEdge = cam.transform.position.x + halfWidth;

        minX = leftEdge + edgePadding + trashZoneWidth;
        maxX = rightEdge - edgePadding - deliveryZoneWidth;
        spawnY = cam.transform.position.y + halfHeight + 1f;
    }

    // Draws the spawn range as green lines in the Scene view so the zones are easy to check
    void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;
        CalculateSpawnRange();

        float bottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, spawnY, 0), new Vector3(minX, bottom, 0));
        Gizmos.DrawLine(new Vector3(maxX, spawnY, 0), new Vector3(maxX, bottom, 0));
    }
}