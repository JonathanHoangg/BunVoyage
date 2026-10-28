using TMPro;
using UnityEngine;

// Tracks how much money the player earns from delivered burgers and remembers the best run ever
public class ScoreManager : MonoBehaviour
{
    // The text that shows this run's profit and the best profit ever
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI topProfitText;

    // How much a burger sells for, which goes up with each filling
    [SerializeField] int basePrice = 5;
    [SerializeField] int pricePerFilling = 2;

    const string TopProfitKey = "TopProfit";

    int profit;
    int topProfit;
    int topProfitAtStart;

    public int Profit => profit;
    public int TopProfit => topProfit;

    // True when this run earned more than the best run before it
    public bool BeatTopProfit => profit > topProfitAtStart;

    // Loads the saved top profit and shows both scores
    void Start()
    {
        topProfit = PlayerPrefs.GetInt(TopProfitKey, 0);
        topProfitAtStart = topProfit;
        UpdateText();
    }

    // Adds the price of a delivered burger and saves a new top profit if this run beats it
    public int AddSale(int fillingCount)
    {
        int price = basePrice + pricePerFilling * fillingCount;
        profit += price;

        if (profit > topProfit)
        {
            topProfit = profit;
            PlayerPrefs.SetInt(TopProfitKey, topProfit);
            PlayerPrefs.Save();
        }

        UpdateText();
        return price;
    }

    // Updates the on screen text with the current numbers
    void UpdateText()
    {
        if (profitText != null) profitText.text = "Profit: $" + profit;
        if (topProfitText != null) topProfitText.text = "Top Profit Score: $" + topProfit;
    }
}