using UnityEngine;
using TMPro;
using System.Collections;

public class ScanFeedback : MonoBehaviour
{
    [Header("UI")]
    public GameObject successPanel;
    public TextMeshProUGUI successText;

    [Header("Settings")]
    public float displayTime = 3.5f;

    void OnEnable()
    {
        QRCodeScanner.OnQRCodeDetected += OnQRDetected;
    }

    void OnDisable()
    {
        QRCodeScanner.OnQRCodeDetected -= OnQRDetected;
    }

    void OnQRDetected(string qrText)
    {
        // Only show feedback if this is a known marker
        if (string.IsNullOrEmpty(qrText)) return;

        StopAllCoroutines();
        StartCoroutine(ShowSuccessMessage(qrText));
    }

    IEnumerator ShowSuccessMessage(string qrId)
    {
        if (successPanel != null)
        {
            successPanel.SetActive(true);

            if (successText != null)
            {
                successText.text = $"QR Scanned Successfully!\n({qrId})\nLook at the floor for the arrow";
            }
        }

        yield return new WaitForSeconds(displayTime);

        if (successPanel != null)
            successPanel.SetActive(false);
    }
}