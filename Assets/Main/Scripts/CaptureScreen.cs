using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaptureScreen : MonoBehaviour
{
    public RawImage screenshotDisplay; // UI Image to display the captured moment

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S))
        {
            CaptureMoment();
        }
    }
    public void CaptureMoment()
    {
        // Start the coroutine to capture the screen
        StartCoroutine(CaptureFromCamera());
        //StartCoroutine(CaptureMomentCoroutine());
    }

    private IEnumerator CaptureMomentCoroutine()
    {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();

        // Capture the screen and assign it to the RawImage
        Texture2D capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();
        screenshotDisplay.texture = capturedTexture;
    }
   private IEnumerator CaptureFromCamera()
    {
        // Get the aspect ratio of the camera
        float cameraAspect = (float)Camera.main.pixelWidth / Camera.main.pixelHeight;

        // Set the RenderTexture with the same aspect ratio as the camera
        int textureWidth = 1920; // Or any preferred width
        int textureHeight = Mathf.RoundToInt(textureWidth / cameraAspect);

        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        Camera.main.targetTexture = renderTexture;

        // Render the camera's view to the RenderTexture
        Camera.main.Render();

        // Read the pixels from the RenderTexture into a Texture2D
        RenderTexture.active = renderTexture;
        Texture2D capturedTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        capturedTexture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        capturedTexture.Apply();

        // Assign the captured texture to the RawImage
        screenshotDisplay.texture = capturedTexture;

        // Adjust the RawImage aspect ratio
        float rawImageAspect = (float)textureWidth / textureHeight;
        screenshotDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(
            screenshotDisplay.GetComponent<RectTransform>().sizeDelta.y * rawImageAspect,
            screenshotDisplay.GetComponent<RectTransform>().sizeDelta.y
        );

        // Cleanup
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        yield return null; // Optional: Allow the next frame to render
    }
}