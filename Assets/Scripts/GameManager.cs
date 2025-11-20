using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player { get; private set; }

    public GameObject pauseMenu;

    public bool IsPause { get; private set; }

    private bool _isBlockedPause;

    private float _timer;
    [SerializeField] private GameObject scoreUI;

    [SerializeField] private TextMeshProUGUI textTimer, textRank, textNextScore;
    [SerializeField] private int[] timesRanks;
    [SerializeField] private string phrasePlayerScoreObtained, phraseNextScore;

    [SerializeField] private string sceneNextLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        scoreUI.SetActive(false);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        int minutes = (int)(_timer / 60f);
        int seconds = (int)(_timer % 60f);
        textTimer.text = $"{minutes:00}:{seconds:00}";

    }

    public void ShowScore()
    {
        PauseGameAndBlock();
        textRank.text = GetRank().ToString();
        textNextScore.text = GetNextScore();
        scoreUI.SetActive(true);

    }

    private char GetRank()
    {
        if (timesRanks == null || timesRanks.Length == 0)
        {
            Debug.LogWarning("timesRanks está vacío, no se puede calcular rank.");
            return '?';
        }

        // A = 65 en ASCII
        char baseRank = 'A';

        for (int i = 0; i < timesRanks.Length; i++)
        {
            if (_timer <= timesRanks[i])
            {
                return (char)(baseRank + i);
            }
        }

        // Si no cumple ningún tiempo, asigna el siguiente rango inferior
        return (char)(baseRank + timesRanks.Length);
    }

    private String GetNextScore()
    {
        string phrase = "";

        if (timesRanks == null || timesRanks.Length == 0)
        {
            return "No scores defined";
        }

        int indexScore = 0;

        for (int i = 0; i < timesRanks.Length; i++)
        {
            if (!(_timer <= timesRanks[i])) continue;
            indexScore = i;
            break;
        }

        phrase += $"{phrasePlayerScoreObtained} {IntToTime((int)_timer)}. ";
        if (indexScore == 0)
        {
            return phrase;
        }

        int score = timesRanks[indexScore] - timesRanks[indexScore - 1];

        phrase += $"{phraseNextScore} {IntToTime(score)}";

        return phrase;
    }

    private string IntToTime(int totalSeconds)
    {
        var t = System.TimeSpan.FromSeconds(totalSeconds);
        return $"{t.Minutes:D2}:{t.Seconds:D2}";
    }


    public void SwitchPause()
    {
        if (_isBlockedPause)
        {
            return;
        }

        if (pauseMenu.activeInHierarchy)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        IsPause = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(!_isBlockedPause);
    }

    public void PauseGameAndBlock()
    {
        _isBlockedPause = true;
        PauseGame();
    }

    public void UnlockGame()
    {
        _isBlockedPause = false;
        ResumeGame();
    }

    public void ResumeGame()
    {
        IsPause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneNextLevel);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}