using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadArtStyleScene(string artStyle)
    {
        // Save the selected art style to PlayerPrefs
        PlayerPrefs.SetString("SelectedArtStyle", artStyle);

        // Load scene corresponding to the selected art style
        SceneManager.LoadScene(artStyle + "Scene");
    }
}
