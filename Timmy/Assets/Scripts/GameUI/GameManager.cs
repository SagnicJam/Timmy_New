using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Tweak Params")]
    public int allThreeStarsPoints;
    public int twoStarsPoints;
    public int oneStarsPoints;
    public PlayerController playerController;
    public int levelLength;
    public TimerUIData[] timerUIData;

    [Multiline]
    public string[] motivationalMessageArr;

    [Header("References")]
    public GameObject firstStar;
    public GameObject secondStar;
    public GameObject thirdStar;

    public Image timerCircleImage;
    public GameObject looseScreen;
    public GameObject winScreen;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public PostProcess grayScalePostProcess;

    public TextMeshProUGUI reasonMessageText;
    public TextMeshProUGUI attempText;
    public TextMeshProUGUI timeTakenText;
    public TextMeshProUGUI scoreText;

    [Header("Live Data")]
    public static int attempts;
    public static float timeTaken;
    public int livetimeCounterBeforeMatchStart;
    public float finalZ;
    public static int currentScore;

    [Header("UnityEvents")]
    public UnityEvent onStartCounterComplete;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        timeTaken += Time.deltaTime;
    }

    private void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        currentScoreText.text = currentScore.ToString();
        finalZ = playerController.transform.position.z+levelLength;

        StartCountTimer();
    }

    public bool CheckLevelEnd()
    {
        return finalZ <= playerController.transform.position.z;
    }

    public void OnCoinCollected()
    {
        currentScore++;
        currentScoreText.text = currentScore.ToString();
        if (currentScore> PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore",currentScore);
            highScoreText.text = currentScore.ToString();
        }
    }

    public void StartCountTimer()
    {
        StartCoroutine(TimerToStartGame());
    }

    IEnumerator TimerToStartGame()
    {
        Time.timeScale = 0;
        livetimeCounterBeforeMatchStart = 0;
        while (livetimeCounterBeforeMatchStart< timerUIData.Length)
        {
            timerCircleImage.color = timerUIData[livetimeCounterBeforeMatchStart].color;
            timerText.text = timerUIData[livetimeCounterBeforeMatchStart].message;
            livetimeCounterBeforeMatchStart++;
            yield return new WaitForSecondsRealtime(1);
        }
        //Start Match here...
        Time.timeScale = 1;
        onStartCounterComplete?.Invoke();
        yield break;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        grayScalePostProcess.enabled = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        grayScalePostProcess.enabled = false;
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        attempText.text = attempts.ToString();
        scoreText.text = currentScore.ToString();
        timeTakenText.text = FormatTime(timeTaken);

        if(currentScore>=allThreeStarsPoints)
        {
            firstStar.SetActive(true);
            secondStar.SetActive(true);
            thirdStar.SetActive(true);
        }
        else if(currentScore>=twoStarsPoints&&currentScore<allThreeStarsPoints)
        {
            firstStar.SetActive(true);
            secondStar.SetActive(true);
            thirdStar.SetActive(false);
        }
        else if (currentScore >= oneStarsPoints && currentScore < twoStarsPoints)
        {
            firstStar.SetActive(true);
            secondStar.SetActive(false);
            thirdStar.SetActive(false);
        }
        else
        {
            firstStar.SetActive(false);
            secondStar.SetActive(false);
            thirdStar.SetActive(false);
        }
        Time.timeScale = 0;
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowLooseScreen()
    {
        looseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReplaySameLevel()
    {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        attempts++;
        playerController.isInvulnerable = true;
        //Make player invunlerable for 3 seconds to obstacles
    }

    public void GoToGameStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    [Serializable]
    public struct TimerUIData
    {
        public string message;
        public Color color;
    }
}
