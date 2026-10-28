using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ends the run when the last life is lost: saves the results and opens the Shop Closed scene
public class RunEnder : MonoBehaviour
{
    // The scripts this reads from, the scene to open, and how long to pause before switching
    [SerializeField] LivesManager livesManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] string resultsSceneName = "ResultsScene";
    [SerializeField] float delay = 1.5f;

    // Starts listening for the last life being lost
    void OnEnable()
    {
        livesManager.OutOfLives += EndRun;
    }

    // Stops listening when this is turned off
    void OnDisable()
    {
        livesManager.OutOfLives -= EndRun;
    }

    // Saves this run's numbers for the Shop Closed scene, then switches after a short pause
    void EndRun()
    {
        RunResults.Profit = scoreManager.Profit;
        RunResults.NewTopProfit = scoreManager.BeatTopProfit;
        StartCoroutine(LoadResults());
    }

    // Waits in real time because the game is frozen, then opens the Shop Closed scene
    IEnumerator LoadResults()
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(resultsSceneName);
    }
}