using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using System.Collections;
public class MobileCamera : MonoBehaviour
{
    public RawImage rawImage; // Assign the RawImage in the Inspector
    private WebCamTexture webcamTexture;
    public WebCamDevice webCamDevice;
    private bool isCameraInitialized = false; // Track camera initialization state

    void Init()
    {
        // Initialize the camera but don't start it yet
        foreach (var cam in WebCamTexture.devices)
        {
            if (cam.isFrontFacing) // Choose front camera (if available)
            {
                webCamDevice = cam;
                break;
            }
        }

        webcamTexture = new WebCamTexture(webCamDevice.name);
        rawImage.texture = webcamTexture;

        // Adjust the camera feed (aspect ratio and rotation handling)
        AdjustCameraFeed();

        // Apply mirroring to the RawImage (mirror front-facing camera)
        MirrorCamera();
    }

    void TakePermission()
    {
        // Check if the camera permission has been granted
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Init();
        }
        else
        {
            // Request camera permission at runtime
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    // Called after permission request result is received
    private void Update()
    {
        // If permission is granted and camera is not initialized, initialize it
        if (Permission.HasUserAuthorizedPermission(Permission.Camera) && !isCameraInitialized)
        {
            Init();
            isCameraInitialized = true; // Mark camera as initialized
        }
    }

    // Handle button click for toggling the camera
    public void CameraClick()
    {
        TakePermission();
        ToggleCamera();
    }

    // Toggle the camera on/off
    public void ToggleCamera()
    {
        if (webcamTexture == null) return;

        if (webcamTexture.isPlaying)
        {
            // Turn off the camera
            webcamTexture.Stop();
            rawImage.gameObject.SetActive(false);
        }
        else
        {
            // Turn on the camera
            rawImage.gameObject.SetActive(true);
            webcamTexture.Play();
        }
    }

    // Adjust the camera feed (aspect ratio and rotation handling)
    private void AdjustCameraFeed()
    {
        StartCoroutine(WaitForCameraInitialization());
    }

    private IEnumerator WaitForCameraInitialization()
    {
        // Wait until the camera feed is initialized
        while (webcamTexture.width <= 16)
        {
            yield return null;
        }

        // Get the aspect ratio of the camera feed
        float cameraAspectRatio = (float)webcamTexture.width / webcamTexture.height;
        float rawImageAspectRatio = rawImage.rectTransform.rect.width / rawImage.rectTransform.rect.height;

        // Adjust the RawImage's size to match the camera's aspect ratio
        if (cameraAspectRatio > rawImageAspectRatio)
        {
            // Adjust height to match the aspect ratio
            rawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                rawImage.rectTransform.rect.width / cameraAspectRatio);
        }
        else
        {
            // Adjust width to match the aspect ratio
            rawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                rawImage.rectTransform.rect.height * cameraAspectRatio);
        }
    }
    // Apply mirroring to the front-facing camera
    private void MirrorCamera()
    {
        if (webCamDevice.isFrontFacing)
        {
            // Flip the camera horizontally (mirror effect)
            rawImage.rectTransform.localScale = new Vector3(-1, 1, 1);  // Mirror horizontally by flipping the X scale
        }
    }

    // Ensure the camera stops when the object is destroyed
    void OnDestroy()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}