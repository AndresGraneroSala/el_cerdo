using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEnableSelectUI : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}