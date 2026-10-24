using UnityEngine;

// Drops a random ingredient from above the screen at a steady interval
// Ingredients never spawn above the trash zone on the left or the delivery zone on the right
public class IngredientSpawner : MonoBehaviour
{
    // Ingredients to choose from and how often to drop them
    [SerializeField] GameObject[] ingredientPrefabs;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float edgePadding = 0.7f;

    // How much of the screen to keep clear on each side, in world units
    [SerializeField] float trashZoneWidth = 2.5f;
    [SerializeField] float deliveryZoneWidth = 2.5f;

    // How strongly gravity pulls on tagged ingredients and which tag to look for
    [SerializeField] float fallGravity = 0.5f;
    

    float timer;
    float minX, maxX, spawnY;

    // Calculates the spawn range when the game starts
    void Start()
    {
        CalculateSpawnRange();
    }

    // Counts up time and spawns an ingredient each time the interval passes
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    // Picks a random ingredient, drops it inside the allowed range, and sets how fast it falls
        // Picks a random ingredient, drops it inside the allowed range, and sets how fast it falls
    void Spawn()
    {
        if (ingredientPrefabs.Length == 0) return;

        GameObject prefab = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Length)];
        Vector2 position = new Vector2(Random.Range(minX, maxX), spawnY);
        GameObject ingredient = Instantiate(prefab, position, Quaternion.identity);

        Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = fallGravity;
    }
    

    // Finds the left and right spawn limits based on the camera and the zone widths
    void CalculateSpawnRange()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        minX = cam.transform.position.x - halfWidth + edgePadding + trashZoneWidth;
        maxX = cam.transform.position.x + halfWidth - edgePadding - deliveryZoneWidth;
        spawnY = cam.transform.position.y + halfHeight + 1f;
    }

    // Draws the spawn range as green lines in the Scene view so the zones are easy to tune
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