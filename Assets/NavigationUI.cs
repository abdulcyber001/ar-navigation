using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NavigationUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown destinationDropdown;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI etaText;

    [Header("References")]
    public PathArrowController arrowController;
    public MinimapController minimapController;

    private List<string> destinationIds = new List<string>();
    private string selectedDestinationId;

    void Start()
    {
        PopulateDropdown();
        destinationDropdown.onValueChanged.AddListener(OnDestinationChanged);
    }

    void PopulateDropdown()
    {
        destinationDropdown.ClearOptions();
        destinationIds.Clear();

        QRLocalizer localizer = FindAnyObjectByType<QRLocalizer>();
        if (localizer == null) return;

        List<string> options = new List<string>();

        foreach (var m in localizer.markers)
        {
            options.Add(m.displayName);
            destinationIds.Add(m.qrId);
        }

        destinationDropdown.AddOptions(options);
    }

    void OnDestinationChanged(int index)
    {
        if (index < 0 || index >= destinationIds.Count) return;

        selectedDestinationId = destinationIds[index];
        StartNavigation();
    }

    public void OnStartHereClicked()
    {
        StartNavigation();
    }

    void StartNavigation()
    {
        if (!QRLocalizer.IsLocalized || string.IsNullOrEmpty(selectedDestinationId))
        {
            distanceText.text = "Scan a QR code first";
            etaText.text = "";
            return;
        }

        string startId = QRLocalizer.CurrentMarkerId;
        List<string> path = NavigationGraph.Instance.FindShortestPath(startId, selectedDestinationId);

        if (path == null || path.Count == 0)
        {
            distanceText.text = "No path found";
            etaText.text = "";
            return;
        }

        // Send path to the ground arrow
        if (arrowController != null)
            arrowController.SetPath(path);

        // Send path to the minimap
        if (minimapController != null)
            minimapController.SetPath(path);

        float dist = NavigationGraph.Instance.GetPathDistance(path);
        distanceText.text = "Distance: " + dist.ToString("F1") + " m";

        float minutes = dist / 1.2f / 60f; // average walking speed
        etaText.text = "ETA: ~" + Mathf.CeilToInt(minutes) + " min";
    }
}