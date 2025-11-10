using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string nameScene;

    public void Load()
    {
        SceneManager.LoadScene(nameScene);
    }
}
