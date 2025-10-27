using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject player {get; private set;}
    
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
}