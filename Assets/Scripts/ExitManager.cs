
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;

public class ExitManager : MonoBehaviour
{
    public TMP_Text resultsText;
    public TMP_Text topScoresText;

    private const string GetScoresURL =
        "http://104.129.130.168/getShooters.php";

    private const string UpdateScoresURL =
        "http://104.129.130.168/updateShooters.php";

    void Start()
    {
        DisplayPlayerResults();
        StartCoroutine(SubmitScoreAndLoadTopScores());
    }

    void DisplayPlayerResults()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        int level = PlayerPrefs.GetInt("Level", 1);
        int score = PlayerPrefs.GetInt("Score", 0);
        float survivalTime = PlayerPrefs.GetFloat("SurvivalTime", 0f);

        if (resultsText != null)
        {
            resultsText.text =
                "Player: " + playerName +
                "\nLevel: " + level +
                "\nScore: " + score +
                "\nSurvival Time: " +
                Mathf.RoundToInt(survivalTime) + " seconds";
        }
    }

    IEnumerator SubmitScoreAndLoadTopScores()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        int level = PlayerPrefs.GetInt("Level", 1);
        int score = PlayerPrefs.GetInt("Score", 0);
        float survivalTime = PlayerPrefs.GetFloat("SurvivalTime", 0f);

        if (topScoresText != null)
            topScoresText.text = "Submitting score...";

        string submitURL = UpdateScoresURL
            + "?name=" + UnityWebRequest.EscapeURL(playerName)
            + "&level=" + level
            + "&score=" + score
            + "&time=" + survivalTime.ToString(
                CultureInfo.InvariantCulture
            );

        using (UnityWebRequest request = UnityWebRequest.Get(submitURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "Score submission failed: " + request.error
                );
            }
            else
            {
                Debug.Log(
                    "Score submission response: " +
                    request.downloadHandler.text
                );
            }
        }

        yield return StartCoroutine(LoadTopScores());
    }

    IEnumerator LoadTopScores()
    {
        if (topScoresText == null)
            yield break;

        topScoresText.text = "Loading high scores...";

        using (UnityWebRequest request =
               UnityWebRequest.Get(GetScoresURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                topScoresText.text =
                    "Could not load high scores.\n" + request.error;
                yield break;
            }

            string[] lines = request.downloadHandler.text.Split('\n');
            topScoresText.text = "";

            int count = 0;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                string[] fields = trimmedLine.Split(
                    (char[])null,
                    System.StringSplitOptions.RemoveEmptyEntries
                );

                if (fields.Length < 3)
                    continue;

                count++;

                topScoresText.text +=
                    count + ". " + fields[0] +
                    " — Level: " + fields[1] +
                    " — Score: " + fields[2] + "\n";

                if (count >= 5)
                    break;
            }

            if (count == 0)
                topScoresText.text = "No scores found.";
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Preferences");
    }
}
