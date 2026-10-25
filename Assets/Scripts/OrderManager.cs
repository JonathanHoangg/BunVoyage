using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Creates random burger orders and draws the current one on the ticket as an evenly spaced list of ingredients
public class OrderManager : MonoBehaviour
{
    // Where the picture is drawn and the prefabs that provide each ingredient's sprite
    // Scale the Ticket Stack object to make the ingredients bigger or smaller
    [SerializeField] Transform ticketStack;
    [SerializeField] Ingredient[] ingredientPrefabs;

    // How much vertical space the ingredients can use and the largest gap allowed between them
    [SerializeField] float ticketHeight = 2.3f;
    [SerializeField] float maxSpacing = 0.6f;

    // Which sorting order the picture starts at and how many fillings an order can have
    [SerializeField] int ticketSortingOrder = 21;
    [SerializeField] int minFillings = 1;
    [SerializeField] int maxFillings = 6;

    // Fillings that can appear between the buns
    static readonly IngredientType[] fillings =
    {
        IngredientType.Patty,
        IngredientType.Cheese,
        IngredientType.Lettuce,
        IngredientType.Tomato
    };

    readonly List<IngredientType> currentOrder = new List<IngredientType>();
    readonly Dictionary<IngredientType, Ingredient> prefabLookup = new Dictionary<IngredientType, Ingredient>();

    // Lets other scripts read the order, which burger checking will use next
    public IReadOnlyList<IngredientType> CurrentOrder => currentOrder;

    // Matches each ingredient type to its prefab and creates the first order
    void Start()
    {
        foreach (Ingredient prefab in ingredientPrefabs)
        {
            if (prefab != null) prefabLookup[prefab.Type] = prefab;
        }
        NewOrder();
    }

    // Press N to make a new order while testing (remove before submitting)
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            NewOrder();
        }
    }

    // Builds an order with a bottom bun, random fillings, and a top bun
    public void NewOrder()
    {
        currentOrder.Clear();
        currentOrder.Add(IngredientType.BottomBun);

        // Random.Range leaves out the max for whole numbers, so 1 is added to include it
        int count = Random.Range(minFillings, maxFillings + 1);
        for (int i = 0; i < count; i++)
        {
            currentOrder.Add(fillings[Random.Range(0, fillings.Length)]);
        }

        currentOrder.Add(IngredientType.TopBun);
        DrawTicket();
    }

    // Clears the old picture and places each ingredient evenly spaced from the bottom up
    void DrawTicket()
    {
        if (ticketStack == null) return;

        foreach (Transform child in ticketStack)
        {
            Destroy(child.gameObject);
        }

        // Spreads the ingredients across the ticket, but never farther apart than Max Spacing
        int count = currentOrder.Count;
        float spacing = count > 1 ? Mathf.Min(maxSpacing, ticketHeight / (count - 1)) : 0f;

        for (int i = 0; i < count; i++)
        {
            if (!prefabLookup.TryGetValue(currentOrder[i], out Ingredient prefab)) continue;

            Sprite sprite = prefab.GetComponent<SpriteRenderer>().sprite;

            GameObject icon = new GameObject(currentOrder[i].ToString());
            icon.transform.SetParent(ticketStack, false);
            icon.transform.localPosition = new Vector3(0f, i * spacing - sprite.bounds.center.y, 0f);

            SpriteRenderer sr = icon.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = ticketSortingOrder + i;
        }
    }

    // Draws a yellow line in the Scene view showing the space the ingredients can use on the ticket
    void OnDrawGizmosSelected()
    {
        if (ticketStack == null) return;

        Vector3 bottom = ticketStack.position;
        Vector3 top = ticketStack.TransformPoint(new Vector3(0f, ticketHeight, 0f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottom, top);
        Gizmos.DrawWireSphere(bottom, 0.05f);
        Gizmos.DrawWireSphere(top, 0.05f);
    }
}