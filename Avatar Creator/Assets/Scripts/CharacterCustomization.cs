using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCustomization : MonoBehaviour
{
    public Image characterImage; // Reference to the Image component displaying the character
    public string bodyPartsFolderPath = "BodyParts/";
    public Transform categoryMenuPanel; // Reference to the panel where category menu buttons will be added
    public Transform bodyPartPanel; // Reference to the panel where body part buttons will be added
    public GameObject categoryButtonPrefab; // Prefab for the category button
    public GameObject bodyPartButtonPrefab; // Prefab for the body part button

    void Start()
    {
        // Load all category folders from the specified path
        string[] categories = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/" + bodyPartsFolderPath);

        // Create category buttons in the menu panel
        foreach (string categoryPath in categories)
        {
            string categoryName = System.IO.Path.GetFileName(categoryPath);

            GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryMenuPanel);
            categoryButton.GetComponentInChildren<TextMeshProUGUI>().text = categoryName;
            categoryButton.GetComponent<Button>().onClick.AddListener(() => OnCategoryButtonClicked(categoryName));
        }
    }

    void OnCategoryButtonClicked(string categoryName)
    {
        // Clear existing body part buttons from the body part panel
        foreach (Transform child in bodyPartPanel)
        {
            Destroy(child.gameObject);
        }

        // Load all body part sprites from the selected category folder
        string folderPath = bodyPartsFolderPath + categoryName + "/";
        Sprite[] bodyParts = Resources.LoadAll<Sprite>(folderPath);

        // Create body part buttons in the body part panel
        foreach (Sprite bodyPart in bodyParts)
        {
            GameObject bodyPartButton = Instantiate(bodyPartButtonPrefab, bodyPartPanel);
            bodyPartButton.GetComponent<Image>().sprite = bodyPart;

            // Display the sprite name in the body part button's text component
            bodyPartButton.GetComponentInChildren<TextMeshProUGUI>().text = bodyPart.name;

            bodyPartButton.GetComponent<Button>().onClick.AddListener(() => OnBodyPartButtonClicked(bodyPart));
        }
    }

    void OnBodyPartButtonClicked(Sprite selectedBodyPart)
    {
        // Update character image with the selected body part
        characterImage.sprite = selectedBodyPart;
    }
}
