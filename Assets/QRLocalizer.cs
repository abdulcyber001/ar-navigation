using UnityEngine;
using System.Collections.Generic;

public class QRLocalizer : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin;

    [Header("Marker Positions")]
    public List<MarkerEntry> markers = new List<MarkerEntry>();

    [System.Serializable]
    public class MarkerEntry
    {
        public string qrId;
        public Vector3 worldPosition;
        public string displayName;
    }

    public static string CurrentMarkerId { get; private set; } = null;
    public static bool IsLocalized { get; private set; } = false;

    private Dictionary<string, Vector3> positionMap = new Dictionary<string, Vector3>();

    void OnEnable()
    {
        QRCodeScanner.OnQRCodeDetected += OnQRDetected;
    }

    void OnDisable()
    {
        QRCodeScanner.OnQRCodeDetected -= OnQRDetected;
    }

    void Start()
    {
        positionMap.Clear();
        foreach (var m in markers)
        {
            if (!string.IsNullOrEmpty(m.qrId))
                positionMap[m.qrId.Trim().ToUpper()] = m.worldPosition;
        }

        if (xrOrigin == null)
            xrOrigin = transform;
    }

    void OnQRDetected(string qrText)
    {
        SetStartFromId(qrText);
    }

    public void SetStartFromId(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        id = id.Trim().ToUpper();

        if (!positionMap.ContainsKey(id))
        {
            Debug.LogWarning("Unknown start: " + id);
            return;
        }

        Vector3 knownPosition = positionMap[id];
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 offset = knownPosition - cameraPosition;
        offset.y = knownPosition.y - cameraPosition.y;

        if (xrOrigin == null)
            xrOrigin = transform;

        xrOrigin.position += offset;

        CurrentMarkerId = id;
        IsLocalized = true;

        Debug.Log("Localized / start set to " + id + " at " + knownPosition);
    }
}