using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CharacterScreenshot : MonoBehaviour
{
    public Camera characterCamera; // Reference to the character camera
    public TextMeshProUGUI screenshotSavedPopup; // Reference to the screenshot saved popup
    public float popupFadeDuration = 1f; // Duration in seconds for the popup to fade away

    // Method to take a screenshot
    public void TakeScreenshot()
    {
        // Disable the character camera temporarily
        bool characterCameraEnabled = characterCamera.enabled;
        characterCamera.enabled = false;

        // Create a new RenderTexture to render the character camera's output
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        characterCamera.targetTexture = renderTexture; // Set the character camera's target texture

        // Render the character camera's view
        characterCamera.Render();

        // Create a new Texture2D to read the RenderTexture data
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture; // Set the active RenderTexture
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); // Read the RenderTexture data
        screenshotTexture.Apply(); // Apply changes to the Texture2D

        // Reset the character camera's target texture
        characterCamera.targetTexture = null;
        RenderTexture.active = null;

        // Convert the Texture2D to a PNG byte array
        byte[] bytes = screenshotTexture.EncodeToPNG();

        // Save the screenshot to the desktop
        string fileName = "CharacterScreenshot_" + DateTime.Now.ToString("yyyyMMdd") + ".png";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Screenshot saved to: " + filePath);

        // Show the screenshot saved popup
        if (screenshotSavedPopup != null)
        {
            screenshotSavedPopup.gameObject.SetActive(true);
            StartCoroutine(FadeOutPopup());
        }

        // Re-enable the character camera
        characterCamera.enabled = characterCameraEnabled;
    }

    // Coroutine to fade out the screenshot saved popup
    private IEnumerator FadeOutPopup()
    {
        float startTime = Time.time;
        Color originalColor = screenshotSavedPopup.color;
        while (Time.time - startTime < popupFadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / popupFadeDuration;
            Color fadeColor = originalColor;
            fadeColor.a = Mathf.Lerp(originalColor.a, 0f, normalizedTime);
            screenshotSavedPopup.color = fadeColor;
            yield return null;
        }

        // Ensure the popup is completely faded out
        Color fadedColor = originalColor;
        fadedColor.a = 0f;
        screenshotSavedPopup.color = fadedColor;

        // Deactivate the popup after fading out
        screenshotSavedPopup.gameObject.SetActive(false);
    }
}
