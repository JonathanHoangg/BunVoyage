using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Runs the Shop Closed scene: shows the last run's results and lets the player play again or go back to the title
public class ResultsMenu : MonoBehaviour
{
    // The scenes to open and the text that shows the results
    [SerializeField] string gameSceneName = "SampleScene";
    [SerializeField] string titleSceneName = "TitleScene";
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI topProfitText;
    [SerializeField] GameObject newTopMessage;

    // Fills in the results from the run that just ended
    void Start()
    {
        Time.timeScale = 1f;
        profitText.text = "Profit: $" + RunResults.Profit;
        topProfitText.text = "Top Profit Score: $" + PlayerPrefs.GetInt("TopProfit", 0);
        newTopMessage.SetActive(RunResults.NewTopProfit);
    }

    // Lets the player press Enter to play again
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            PlayAgain();
        }
    }

    // Starts a fresh run
    public void PlayAgain()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Goes back to the title screen
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}