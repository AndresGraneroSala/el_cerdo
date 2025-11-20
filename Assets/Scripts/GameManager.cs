using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject Player {get; private set;}
    
    public GameObject pauseMenu;

    public bool IsPause { get; private set; }

    [HideInInspector] public bool isBlockedPause;

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
        
    }

    public void SwitchPause()
    {
        if (isBlockedPause)
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

        pauseMenu.SetActive(!isBlockedPause);
    }

    public void ResumeGame()
    {
        IsPause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}