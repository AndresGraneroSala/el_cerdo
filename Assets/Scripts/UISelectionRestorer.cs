using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISelectionRestorer : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference navigateAction;

    private GameObject _lastValidSelection;

    private void OnEnable()
    {
        if (navigateAction != null)
            navigateAction.action.Enable();
    }

    private void OnDisable()
    {
        if (navigateAction != null)
            navigateAction.action.Disable();
    }
    

    private void Update()
    {
        var eventSystem = EventSystem.current;
        if (!eventSystem)
            return;

        GameObject current = eventSystem.currentSelectedGameObject;

        if (current)
        {
            _lastValidSelection = current;
            return;
        }

        if (!navigateAction || !_lastValidSelection)
            return;

        Vector2 navigation = navigateAction.action.ReadValue<Vector2>();

        if (navigation.sqrMagnitude > 0.1f)
        {
            eventSystem.SetSelectedGameObject(_lastValidSelection);
        }
    }
}