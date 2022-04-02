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
    public float clearance;

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
    public int currentLevelScore;

    [Header("UnityEvents")]
    public UnityEvent onStartCounterComplete;
    public bool started;
    private void Awake()
    {
        instance = this;
    }

    public GameObject expoVFX;
    public void SpawnExplosion(Vector3 pos)
    {
        GameObject g = Instantiate(expoVFX);
        g.transform.position = pos;
    }

    private void Update()
    {
        timeTaken += Time.deltaTime;
    }

    public void PulsateCurrentScore()
    {
        StopCoroutine("PulsateCS");
        StartCoroutine("PulsateCS");
    }

    public float maxScale = 1.2f;
    public float scaleSpeed = 1f;
    IEnumerator PulsateCS()
    {
        Vector3 currentScale = currentScoreText.transform.localScale;
        Vector3 finalScale = Vector3.one * maxScale;

        float t1 = 0;
        while(finalScale.x - currentScoreText.transform.localScale.x > 0.01f)
        {
            currentScoreText.transform.localScale = Mathf.Lerp(currentScale.x, finalScale.x, t1) * Vector3.one;
            t1 += Time.unscaledDeltaTime* scaleSpeed;
            yield return null;
        }

        currentScoreText.transform.localScale = finalScale;
        Vector3 currentScale2 = currentScoreText.transform.localScale;
        Vector3 finalScale2 = Vector3.one;
        float t2 = 0;

        while (currentScoreText.transform.localScale.x - finalScale2.x > 0.01f)
        {
            currentScoreText.transform.localScale = Mathf.Lerp(currentScale2.x, finalScale2.x, t2) * Vector3.one;
            t2 += Time.unscaledDeltaTime * scaleSpeed;
            yield return null;
        }

        currentScoreText.transform.localScale = finalScale2;
        yield break;
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

    public bool CheckObstacleSpawnEnd(float zValue)
    {
        bool result = (finalZ - clearance <= zValue) && started;
        Debug.Log("RESYKT " + result);
        return result;
    }

    public void OnCoinCollected()
    {
        currentScore++;
        currentLevelScore++;
        PulsateCurrentScore();
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
            JukeBoxUnit.instance.PlaySFX(0);
            yield return new WaitForSecondsRealtime(1);
        }
        //Start Match here...
        Time.timeScale = 1;
        onStartCounterComplete?.Invoke();
        started = true;
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
        JukeBoxUnit.instance.PlayVFX(0, PlayerController.instance.playerCentre.position);
        PlayerController.instance.HidePlayer();
        looseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReplaySameLevel()
    {
        Time.timeScale = 1;
        currentScore -= currentLevelScore;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        PlayerController.instance.ShowPlayer();
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        attempts++;
        playerController.isInvulnerable = true;
        playerController.invulfx.SetActive(true);
        PlayerController.instance.ShowPlayer();
        //Make player invunlerable for 3 seconds to obstacles
    }

    public void FirePlayer()
    {
        PlayerController.instance.FireLaserProjectile();
    }

    public void ShieldPlayer()
    {
        PlayerController.instance.ShieldPressed();
    }

    public GameObject fireButton;
    public GameObject shieldButton;
    public Image fireTimer;
    public Image shieldTimer;
    public TextMeshProUGUI fireText;
    public void GoToGameStart()
    {
        currentScore = 0;
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
