using UnityEngine;

public class Manager1 : MonoBehaviour
{
    public static Manager1 Instance;

    [SerializeField] private GameObject instructions;

    private int _totalScore, _score;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OpenTutorial();

    }

    private void OpenTutorial()
    {
        GameManager.Instance.PauseGameAndBlock();
        instructions.SetActive(true);
    }

    public void CloseTutorial()
    {
        GameManager.Instance.UnlockGame();
        instructions.SetActive(false);
    }

    public void AddBalloon()
    {
        _totalScore++;
    }

    public void BalloonExplode()
    {
        _score++;

        if (_score >= _totalScore)
        {
            Win();
        }
    }
    
    private void Win()
    {
        GameManager.Instance.ShowScore();
    }

}
