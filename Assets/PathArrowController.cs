using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PathArrowController : MonoBehaviour
{
    [Header("References")]
    public GameObject arrowPrefab;
    public ARRaycastManager raycastManager;

    [Header("Settings")]
    public float heightOffset = 0.05f;
    public float updateRate = 0.15f;
    public float reachDistance = 2.5f;

    private GameObject currentArrow;
    private List<string> currentPath;
    private int nextIndex = 1;
    private float timer;

    public void SetPath(List<string> path)
    {
        currentPath = path;
        nextIndex = 1;

        if (currentArrow == null && arrowPrefab != null)
        {
            currentArrow = Instantiate(arrowPrefab);
        }
    }

    void Update()
    {
        if (currentPath == null || currentPath.Count < 2 || !QRLocalizer.IsLocalized)
            return;

        timer += Time.deltaTime;
        if (timer < updateRate) return;
        timer = 0f;

        string nextId = currentPath[Mathf.Min(nextIndex, currentPath.Count - 1)];
        Vector3 targetPos = GetMarkerPosition(nextId);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            Vector3 arrowPos = hitPose.position + Vector3.up * heightOffset;

            Vector3 direction = targetPos - arrowPos;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f && currentArrow != null)
            {
                currentArrow.transform.position = arrowPos;
                currentArrow.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        Vector3 userPos = Camera.main.transform.position;
        userPos.y = 0;
        Vector3 wp = targetPos;
        wp.y = 0;

        if (Vector3.Distance(userPos, wp) < reachDistance && nextIndex < currentPath.Count - 1)
        {
            nextIndex++;
        }
    }

    Vector3 GetMarkerPosition(string id)
    {
        QRLocalizer localizer = FindAnyObjectByType<QRLocalizer>();
        if (localizer != null)
        {
            foreach (var m in localizer.markers)
            {
                if (m.qrId == id)
                    return m.worldPosition;
            }
        }
        return Vector3.zero;
    }
}