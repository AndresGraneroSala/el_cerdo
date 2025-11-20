using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    public void SwitchAct()
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}
