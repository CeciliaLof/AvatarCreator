using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreenshot : MonoBehaviour
{
    public Camera characterCamera; // Reference to the camera used to render the character
    public RectTransform characterBounds; // Bounds of the character in the UI
    public Button screenshotButton; // Button to trigger taking the screenshot

    public void TakeScreenshot()
    {
        // Create a RenderTexture to render the character
        RenderTexture renderTexture = new RenderTexture((int)characterBounds.rect.width, (int)characterBounds.rect.height, 24);
        characterCamera.targetTexture = renderTexture;

        // Render the character
        characterCamera.Render();

        // Create a texture to hold the rendered image
        Texture2D texture = new Texture2D((int)characterBounds.rect.width, (int)characterBounds.rect.height, TextureFormat.RGB24, false);

        // Read the pixels from the RenderTexture and apply them to the texture
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Reset the active RenderTexture and camera target texture
        RenderTexture.active = null;
        characterCamera.targetTexture = null;

        // Encode the texture as a PNG
        byte[] bytes = texture.EncodeToPNG();

        // Save the screenshot as a file
        string fileName = "CharacterScreenshot.png";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // Destroy temporary objects
        Destroy(renderTexture);
        Destroy(texture);
    }

    void Start()
    {
        screenshotButton.onClick.AddListener(TakeScreenshot);
    }
}
