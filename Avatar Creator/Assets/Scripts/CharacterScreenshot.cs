using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharacterScreenshot : MonoBehaviour
{
    public Camera characterCamera; // Reference to the camera used to render the character
    public RectTransform characterBounds; // Bounds of the character in the UI
    public Button screenshotButton; // Button to trigger taking the screenshot

    public void TakeScreenshot()
    {
        // Capture the screenshot as a texture
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        // Check if the screenshot texture is not null
        if (texture != null)
        {
            // Convert the texture to a PNG byte array
            byte[] bytes = texture.EncodeToPNG();

            // Specify the directory where the screenshot will be saved
            string directoryPath = Application.persistentDataPath + "/Screenshots";
            Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist

            // Construct the file path for the screenshot
            string filePath = Path.Combine(directoryPath, "CharacterScreenshot.png");

            // Write the PNG byte array to the file
            File.WriteAllBytes(filePath, bytes);

            // Log the path where the screenshot was saved
            Debug.Log("Screenshot saved to: " + filePath);

            // Destroy the temporary texture
            Destroy(texture);
        }
        else
        {
            Debug.LogError("Failed to capture screenshot.");
        }
    }

    void Start()
    {
        screenshotButton.onClick.AddListener(TakeScreenshot);
    }
}
