using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DestinationButtonController : MonoBehaviour
{
    public Button selectButton;
    public TMP_Dropdown destinationDropdown;
    public NavigationUI navigationUI; // optional reference

    void Start()
    {
        // Hide dropdown at start
        if (destinationDropdown != null)
            destinationDropdown.gameObject.SetActive(false);

        selectButton.onClick.AddListener(OnSelectButtonClicked);
        destinationDropdown.onValueChanged.AddListener(OnDestinationChosen);
    }

    void OnSelectButtonClicked()
    {
        // Toggle the dropdown
        bool isActive = destinationDropdown.gameObject.activeSelf;
        destinationDropdown.gameObject.SetActive(!isActive);
    }

    void OnDestinationChosen(int index)
    {
        // Hide dropdown after selection
        destinationDropdown.gameObject.SetActive(false);
    }
}