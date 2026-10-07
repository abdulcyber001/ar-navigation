using UnityEngine;

public class UIToggle : MonoBehaviour
{
    public GameObject destinationPanel;

    public void ShowHidePanel()
    {
        destinationPanel.SetActive(!destinationPanel.activeSelf);
    }
}