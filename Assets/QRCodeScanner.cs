using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using System;

public class QRCodeScanner : MonoBehaviour
{
    [Header("References")]
    public ARCameraManager cameraManager;

    [Header("Settings")]
    public float scanInterval = 0.4f;

    private IBarcodeReader barcodeReader;
    private float timer = 0f;
    private bool isScanning = true;

    public static event Action<string> OnQRCodeDetected;

    void Start()
    {
        barcodeReader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                PossibleFormats = new[] { BarcodeFormat.QR_CODE },
                TryHarder = true,
                TryInverted = true
            }
        };

        if (cameraManager == null)
            cameraManager = FindObjectOfType<ARCameraManager>();

        if (cameraManager == null)
            Debug.LogError("QRCodeScanner: No ARCameraManager found! Please assign it in the Inspector.");
        else
            Debug.Log("QRCodeScanner: Camera Manager assigned successfully.");
    }

    void Update()
    {
        if (!isScanning)
        {
            // Uncomment the next line only if you want constant spam
            // Debug.Log("QR Scanner is DISABLED");
            return;
        }

        if (cameraManager == null)
        {
            Debug.LogError("cameraManager is NULL – assign it in the Inspector!");
            return;
        }

        timer += Time.deltaTime;
        if (timer < scanInterval) return;
        timer = 0f;

        Debug.Log("Attempting QR scan...");
        Scan();
    }

    void Scan()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            Debug.LogWarning("Could not acquire CPU image from camera");
            return;
        }

        using (image)
        {
            // Using half resolution for better performance on newer phones
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
                outputFormat = TextureFormat.RGBA32,
                transformation = XRCpuImage.Transformation.MirrorY
            };

            int size = image.GetConvertedDataSize(conversionParams);
            var buffer = new Unity.Collections.NativeArray<byte>(size, Unity.Collections.Allocator.Temp);

            try
            {
                image.Convert(conversionParams, buffer);

                int width = conversionParams.outputDimensions.x;
                int height = conversionParams.outputDimensions.y;

                Color32[] pixels = new Color32[width * height];
                for (int i = 0; i < pixels.Length; i++)
                {
                    int idx = i * 4;
                    pixels[i] = new Color32(buffer[idx], buffer[idx + 1], buffer[idx + 2], 255);
                }

                Result result = barcodeReader.Decode(pixels, width, height);

                if (result != null && !string.IsNullOrEmpty(result.Text))
                {
                    string qrText = result.Text.Trim();
                    Debug.Log("QR DETECTED → " + qrText);
                    OnQRCodeDetected?.Invoke(qrText);
                }
                else
                {
                    Debug.Log($"QR scan {width}x{height} – no code found");
                }
            }
            finally
            {
                buffer.Dispose();
            }
        }
    }

    public void EnableScanning(bool enable)
    {
        isScanning = enable;
        Debug.Log("QR Scanning enabled: " + enable);
    }
}