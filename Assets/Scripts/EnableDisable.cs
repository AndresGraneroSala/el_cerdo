using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    public void SwitchAct()
    {
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
