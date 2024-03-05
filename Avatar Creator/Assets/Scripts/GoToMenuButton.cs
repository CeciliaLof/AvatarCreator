using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuButton : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
