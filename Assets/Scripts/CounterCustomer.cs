using UnityEngine;

// One customer's look in each of their three moods, plus the grunt they make when they get impatient
[System.Serializable]
public class CustomerLook
{
    public Sprite happy;
    public Sprite impatient;
    public Sprite angry;
    public AudioClip grunt;
}

// Shows a random customer behind the delivery counter with each new order and runs their impatience meter
// The customer and meter are created by this script and always stay lined up with the counter
// The customer's face changes from happy to impatient to angry, and if the meter runs out they storm off and cost a life
[RequireComponent(typeof(SpriteRenderer))]
public class CounterCustomer : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] OrderManager orderManager;
    [SerializeField] BurgerStack burgerStack;
    [SerializeField] LivesManager livesManager;
    [SerializeField] SoundManager soundManager;

    [Header("Customers")]
    [SerializeField] CustomerLook[] customers;

    // How long a customer waits: a base time plus extra time for each filling in their order
    [Header("Patience")]
    [SerializeField] float basePatience = 10f;
    [SerializeField] float patiencePerFilling = 4f;

    // How far the meter runs down before the customer looks impatient and then angry
    [SerializeField, Range(0f, 1f)] float impatientAt = 0.5f;
    [SerializeField, Range(0f, 1f)] float angryAt = 0.8f;

    // Where the customer stands on the counter image
    // X goes from 0 at the counter's left edge to 1 at its right edge
    // Y goes from 0 at the counter's bottom to 1 at the top of the image
    [Header("Customer Layout")]
    [SerializeField] Vector2 customerAnchor = new Vector2(0.7f, 0.15f);
    [SerializeField] float customerScale = 1f;
    [SerializeField] int customerSortingOrder = -3;

    // The size of the meter, how far it floats above the customer's head, and its colors
    [Header("Meter")]
    [SerializeField] float meterWidth = 2f;
    [SerializeField] float meterHeight = 0.25f;
    [SerializeField] float meterGap = 0.2f;
    [SerializeField] int meterSortingOrder = 5;
    [SerializeField] Color meterBackColor = new Color(0.15f, 0.15f, 0.15f, 0.8f);
    [SerializeField] Color happyColor = new Color(0.35f, 0.8f, 0.35f);
    [SerializeField] Color impatientColor = new Color(1f, 0.8f, 0.2f);
    [SerializeField] Color angryColor = new Color(0.9f, 0.25f, 0.2f);

    SpriteRenderer counterRenderer;
    SpriteRenderer customerRenderer;
    SpriteRenderer meterBack;
    SpriteRenderer meterFill;

    CustomerLook current;
    int lastIndex = -1;
    float patience;
    float waited;
    bool hasGrunted;

    // How impatient the customer is, from 0 when they arrive to 1 when they storm off
    public float Impatience => patience > 0f ? Mathf.Clamp01(waited / patience) : 0f;

    // Creates the customer and the meter as children of the counter
    void Awake()
    {
        counterRenderer = GetComponent<SpriteRenderer>();

        Texture2D white = Texture2D.whiteTexture;
        Sprite square = Sprite.Create(white, new Rect(0, 0, white.width, white.height), new Vector2(0.5f, 0.5f), white.width);

        customerRenderer = CreateChild("Customer", null, customerSortingOrder);
        meterBack = CreateChild("MeterBack", square, meterSortingOrder);
        meterFill = CreateChild("MeterFill", square, meterSortingOrder + 1);
        meterBack.color = meterBackColor;
    }

    // Starts listening for new orders before the first order is made
    void OnEnable()
    {
        orderManager.OrderChanged += NextCustomer;
    }

    // Stops listening when this is turned off
    void OnDisable()
    {
        orderManager.OrderChanged -= NextCustomer;
    }

    // Counts how long the customer has waited, grunts once when they get impatient, and sends them away if they run out of patience
    void Update()
    {
        if (current == null || livesManager.IsOutOfLives) return;

        waited += Time.deltaTime;
        UpdateMood();

        if (!hasGrunted && Impatience >= impatientAt)
        {
            hasGrunted = true;
            if (soundManager != null) soundManager.PlayEffect(current.grunt);
        }

        if (waited >= patience)
        {
            StormOff();
        }
    }

    // Keeps the customer and meter lined up with the counter, even if the counter moves
    void LateUpdate()
    {
        if (customerRenderer.sprite == null) return;

        // Finds the spot on the counter where the customer's feet go
        Bounds counter = counterRenderer.bounds;
        Vector3 standPoint = new Vector3(
            Mathf.Lerp(counter.min.x, counter.max.x, customerAnchor.x),
            Mathf.Lerp(counter.min.y, counter.max.y, customerAnchor.y),
            transform.position.z);

        // Places the bottom middle of the customer on that spot
        Bounds look = customerRenderer.sprite.bounds;
        customerRenderer.transform.localScale = Vector3.one * customerScale;
        Vector3 bottomMiddle = new Vector3(look.center.x, look.min.y, 0f) * customerScale;
        customerRenderer.transform.position = standPoint - bottomMiddle;

        // Floats the meter above the customer's head
        float meterY = standPoint.y + look.size.y * customerScale + meterGap + meterHeight / 2f;
        Vector3 meterCenter = new Vector3(standPoint.x, meterY, transform.position.z);
        meterBack.transform.position = meterCenter;
        meterBack.transform.localScale = new Vector3(meterWidth, meterHeight, 1f);

        // Shrinks the fill from the right as patience runs out and colors it to match the mood
        float remaining = 1f - Impatience;
        meterFill.transform.localScale = new Vector3(meterWidth * remaining, meterHeight, 1f);
        meterFill.transform.position = meterCenter + new Vector3(-meterWidth * (1f - remaining) / 2f, 0f, 0f);
        meterFill.color = MoodColor();
    }

    // Brings a new random customer to the counter with a full meter sized to their order and rings the bell
    void NextCustomer()
    {
        if (customers.Length == 0) return;

        int index = Random.Range(0, customers.Length);
        if (customers.Length > 1 && index == lastIndex)
        {
            index = (index + 1) % customers.Length;
        }
        lastIndex = index;
        current = customers[index];

        int fillings = orderManager.CurrentOrder.Count - 2;
        patience = basePatience + patiencePerFilling * fillings;
        waited = 0f;
        hasGrunted = false;

        UpdateMood();

        if (soundManager != null) soundManager.PlayCustomerDing();
    }

    // The customer leaves angry, which costs a life and brings the next customer
    void StormOff()
    {
        livesManager.LoseLife();
        burgerStack.ResetStack();
        orderManager.NewOrder();
    }

    // Shows the face that matches how impatient the customer is
    void UpdateMood()
    {
        float level = Impatience;
        if (level >= angryAt) customerRenderer.sprite = current.angry;
        else if (level >= impatientAt) customerRenderer.sprite = current.impatient;
        else customerRenderer.sprite = current.happy;
    }

    // Picks the meter color for the current mood
    Color MoodColor()
    {
        float level = Impatience;
        if (level >= angryAt) return angryColor;
        if (level >= impatientAt) return impatientColor;
        return happyColor;
    }

    // Makes a new sprite object inside the counter with the given sprite and drawing order
    SpriteRenderer CreateChild(string childName, Sprite sprite, int sortingOrder)
    {
        GameObject child = new GameObject(childName);
        child.transform.SetParent(transform, false);

        SpriteRenderer sr = child.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = sortingOrder;
        return sr;
    }
}