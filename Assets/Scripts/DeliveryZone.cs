using UnityEngine;

// Serves the burger when the plate slides into the delivery area
// A matching burger earns money, a wrong one costs a life, and either way a new order starts
// Always places itself in the bottom right corner of the screen
[ExecuteAlways]
public class DeliveryZone : MonoBehaviour
{
    // The burger on the plate, the current order, the player's lives, and the money earned
    [SerializeField] BurgerStack burgerStack;
    [SerializeField] OrderManager orderManager;
    [SerializeField] LivesManager livesManager;
    [SerializeField] ScoreManager scoreManager;

    // How far in from the right edge the plate has to slide to deliver, in world units
    [SerializeField] float zoneWidth = 2.5f;

    // The line the plate has to cross and how many burgers have been delivered
    public float LeftEdge { get; private set; }
    public int BurgersDelivered => burgersDelivered;

    SpriteRenderer sr;
    int burgersDelivered;

    // Places the counter as soon as the game starts
    void Awake()
    {
        PlaceOnScreen();
    }

    // In the editor this keeps the counter in place, and during play it serves the burger when the plate crosses the line
    void Update()
    {
        if (!Application.isPlaying)
        {
            PlaceOnScreen();
            return;
        }

        if (burgerStack == null || livesManager.IsOutOfLives) return;

        float plateX = burgerStack.transform.position.x;
        if (plateX >= LeftEdge && burgerStack.IsComplete)
        {
            Serve();
        }
    }

    // Compares the burger to the order, pays for a correct one, then clears the plate and brings a new order
    void Serve()
    {
        if (orderManager.Matches(burgerStack.Layers))
        {
            burgersDelivered++;

            // The fillings are every layer except the two buns
            int fillingCount = burgerStack.Layers.Count - 2;
            scoreManager.AddSale(fillingCount);
        }
        else
        {
            livesManager.LoseLife();
        }

        burgerStack.ResetStack();
        orderManager.NewOrder();
    }

    // Lines the counter art up with the right and bottom edges of the screen
    void PlaceOnScreen()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        float halfWidth = cam.orthographicSize * cam.aspect;
        float rightEdge = cam.transform.position.x + halfWidth;
        float bottomEdge = cam.transform.position.y - cam.orthographicSize;

        LeftEdge = rightEdge - zoneWidth;

        if (sr != null)
        {
            float rightOffset = sr.bounds.max.x - transform.position.x;
            float bottomOffset = transform.position.y - sr.bounds.min.y;
            transform.position = new Vector3(rightEdge - rightOffset, bottomEdge + bottomOffset, transform.position.z);
        }
    }
}