using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component

    public AudioClip categoryButtonAudioClip; // Audio clip for category buttons
    public AudioClip itemButtonAudioClip; // Audio clip for item buttons

    void Start()
    {
        // Find all buttons with the specified tags
        GameObject[] categoryButtons = GameObject.FindGameObjectsWithTag("categoryButton");
        GameObject[] itemButtons = GameObject.FindGameObjectsWithTag("itemButton");

        // Assign audio clips and listeners to category buttons
        foreach (GameObject buttonGO in categoryButtons)
        {
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => PlaySound(categoryButtonAudioClip));
        }

        // Assign audio clips and listeners to item buttons
        foreach (GameObject buttonGO in itemButtons)
        {
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => PlaySound(itemButtonAudioClip));
        }
    }

    void PlaySound(AudioClip audioClip)
    {
        // Play the specified audio clip
        audioSource.PlayOneShot(audioClip);
    }
}
