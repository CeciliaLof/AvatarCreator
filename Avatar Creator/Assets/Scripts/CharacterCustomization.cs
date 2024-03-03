using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCustomization : MonoBehaviour
{
    public Image bodyImage; // Reference to the Image component displaying the character's body
    public Image hairImage; // Reference to the Image component displaying the character's hair
    public Image hairFrontImage; // Reference to the Image component displaying the character's hair
    public Image eyesImage;
    public Image backgroundImage;
    // Add more Image references for other body parts as needed

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
            //bodyPartButton.GetComponentInChildren<TextMeshProUGUI>().text = bodyPart.name;

            bodyPartButton.GetComponent<Button>().onClick.AddListener(() => OnBodyPartButtonClicked(categoryName, bodyPart));
        }
    }

    void OnBodyPartButtonClicked(string categoryName, Sprite selectedBodyPart)
    {
        string categoryLowered = categoryName.ToLower();

        // Update the corresponding Image component with the selected body part
        // You can customize this logic based on your naming conventions
        switch (categoryLowered)
        {
            case "body":
                bodyImage.sprite = selectedBodyPart;
                break;
            case "hair back":
                hairImage.sprite = selectedBodyPart;
                break;
            case "hair front":
                hairFrontImage.sprite = selectedBodyPart;
                break;
            case "eyes":
                eyesImage.sprite = selectedBodyPart;
                break;
            case "background":
                backgroundImage.sprite = selectedBodyPart;
                break;
                // Add more cases for other body parts as needed
        }
    }

    public void RandomizeCharacter()
    {
        // Randomize each category separately
        RandomizeCategory("body");
        RandomizeCategory("hair back");
        RandomizeCategory("hair front");
        RandomizeCategory("eyes");
        RandomizeCategory("background");
        // Add more categories as needed
    }

    private void RandomizeCategory(string categoryName)
    {
        string folderPath = bodyPartsFolderPath + categoryName + "/";
        Sprite[] bodyParts = Resources.LoadAll<Sprite>(folderPath);

        if (bodyParts.Length > 0)
        {
            // Select a random body part from the category
            Sprite randomBodyPart = bodyParts[Random.Range(0, bodyParts.Length)];

            // Update the corresponding Image component with the selected random body part
            OnBodyPartButtonClicked(categoryName, randomBodyPart);
        }
    }
}
