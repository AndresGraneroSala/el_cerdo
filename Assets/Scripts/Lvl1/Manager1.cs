using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager1 : MonoBehaviour
{
    public static Manager1 instance;
    
    [SerializeField] private string nextLevel = "Lvl2";

    [SerializeField] private GameObject instructions;
    
    private int _totalScore, _score;

    private void Awake()
    {
        OpenTutorial();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OpenTutorial()
    {
        Time.timeScale = 0;
        instructions.SetActive(true);
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1;
        instructions.SetActive(false);
    }

    public void AddBalloon()
    {
        _totalScore++;
    }

    public void BalloonExplode()
    {
        _score++;

        if (_score>=_totalScore)
        {
            Win();
        }
        
    }
    

    private void Win()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
}
