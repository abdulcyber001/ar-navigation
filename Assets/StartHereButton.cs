using UnityEngine;
using UnityEngine.UI;

public class StartHereButton : MonoBehaviour
{
    public void OnStartHereClicked()
    {
        QRLocalizer localizer = FindAnyObjectByType<QRLocalizer>();

        if (localizer != null && QRLocalizer.IsLocalized)
        {
            Debug.Log("Start Here pressed. Current location: " + QRLocalizer.CurrentMarkerId);
        }
        else
        {
            Debug.Log("Please scan a QR code first.");
        }
    }
}