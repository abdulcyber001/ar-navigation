using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    [Header("UI")]
    public RectTransform minimapRoot;          // the panel that holds the minimap
    public RectTransform userIcon;             // small icon for the player
    public RectTransform destinationIcon;      // icon for destination
    public LineRenderer pathLine;              // or use UI lines / multiple images

    [Header("Settings")]
    public float mapScale = 2.0f;              // how many pixels per meter
    public Vector2 mapCenterOffset = Vector2.zero;

    private List<string> currentPath;
    private QRLocalizer localizer;

    void Start()
    {
        localizer = FindObjectOfType<QRLocalizer>();
    }

    public void SetPath(List<string> path)
    {
        currentPath = path;
        DrawPath();
    }

    void Update()
    {
        if (!QRLocalizer.IsLocalized || localizer == null) return;

        // Update user icon position on minimap
        if (userIcon != null && Camera.main != null)
        {
            Vector3 userWorld = Camera.main.transform.position;
            userIcon.anchoredPosition = WorldToMinimap(userWorld);
        }
    }

    void DrawPath()
    {
        if (currentPath == null || currentPath.Count < 2 || localizer == null)
            return;

        // Simple version: just place the destination icon
        string destId = currentPath[currentPath.Count - 1];
        Vector3 destPos = GetMarkerPosition(destId);

        if (destinationIcon != null)
        {
            destinationIcon.gameObject.SetActive(true);
            destinationIcon.anchoredPosition = WorldToMinimap(destPos);
        }

        // If you have a LineRenderer for the path
        if (pathLine != null)
        {
            pathLine.positionCount = currentPath.Count;
            for (int i = 0; i < currentPath.Count; i++)
            {
                Vector3 pos = GetMarkerPosition(currentPath[i]);
                Vector2 mapPos = WorldToMinimap(pos);
                pathLine.SetPosition(i, new Vector3(mapPos.x, mapPos.y, 0));
            }
        }
    }

    Vector2 WorldToMinimap(Vector3 worldPos)
    {
        // Simple top-down mapping (X and Z)
        float x = (worldPos.x * mapScale) + mapCenterOffset.x;
        float y = (worldPos.z * mapScale) + mapCenterOffset.y;
        return new Vector2(x, y);
    }

    Vector3 GetMarkerPosition(string id)
    {
        foreach (var m in localizer.markers)
        {
            if (m.qrId == id)
                return m.worldPosition;
        }
        return Vector3.zero;
    }
}