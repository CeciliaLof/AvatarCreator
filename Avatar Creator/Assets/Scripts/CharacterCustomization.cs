using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CharacterCustomization : MonoBehaviour
{
    public Image bodyImage;
    public Image hairBackImage;
    public Image hairFrontImage;
    public Image eyesImage;
    public Image backgroundImage;

    public Transform categoryMenuPanel;
    public Transform bodyPartPanel;
    public GameObject categoryButtonPrefab;
    public GameObject bodyPartButtonPrefab;

    public string[] artStyleFolders;

    void Start()
    {
        // Load sprites based on the default art style (first in the array)
        LoadSpritesForArtStyle(artStyleFolders[0]);

        // Load all subfolders (categories) under the first art style folder
        string[] categoryFolders = Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, artStyleFolders[0]));

        // Create category buttons in the menu panel
        foreach (string categoryFolder in categoryFolders)
        {
            string categoryName = Path.GetFileName(categoryFolder);

            // Create category button
            GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryMenuPanel);
            categoryButton.GetComponentInChildren<TextMeshProUGUI>().text = categoryName;
            categoryButton.GetComponent<Button>().onClick.AddListener(() => OnCategoryButtonClicked(categoryName));
        }

        RandomizeCharacter();
    }

    public void LoadSpritesForArtStyle(string artStyle)
    {
        // Load sprites for the selected art style
        string artStyleFolderPath = Path.Combine(Application.streamingAssetsPath, artStyle);
        bodyImage.sprite = Resources.Load<Sprite>(Path.Combine(artStyleFolderPath, "Body"));
        hairBackImage.sprite = Resources.Load<Sprite>(Path.Combine(artStyleFolderPath, "Hair Back"));
        hairFrontImage.sprite = Resources.Load<Sprite>(Path.Combine(artStyleFolderPath, "Hair Front"));
        eyesImage.sprite = Resources.Load<Sprite>(Path.Combine(artStyleFolderPath, "Eyes"));
        backgroundImage.sprite = Resources.Load<Sprite>(Path.Combine(artStyleFolderPath, "Background"));
    }

    void OnCategoryButtonClicked(string categoryName)
    {
        // Clear existing body part buttons from the body part panel
        foreach (Transform child in bodyPartPanel)
        {
            Destroy(child.gameObject);
        }

        // Load all body part sprites from the selected category folder
        string folderPath = Path.Combine(Application.streamingAssetsPath, artStyleFolders[0], categoryName);
        string[] bodyPartPaths = Directory.GetFiles(folderPath, "*.png");

        // Create body part buttons in the body part panel
        foreach (string bodyPartPath in bodyPartPaths)
        {
            // Load body part sprite
            byte[] fileData = File.ReadAllBytes(bodyPartPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            Sprite bodyPartSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // Create body part button
            GameObject bodyPartButton = Instantiate(bodyPartButtonPrefab, bodyPartPanel);
            bodyPartButton.GetComponent<Image>().sprite = bodyPartSprite;

            // Add click listener to body part button
            bodyPartButton.GetComponent<Button>().onClick.AddListener(() => OnBodyPartButtonClicked(categoryName, bodyPartSprite));
        }
    }
    void OnBodyPartButtonClicked(string categoryName, Sprite selectedBodyPart)
    {
        // Update the corresponding Image component with the selected body part
        switch (categoryName.ToLower())
        {
            case "body":
                bodyImage.sprite = selectedBodyPart;
                break;
            case "hair back":
                hairBackImage.sprite = selectedBodyPart;
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
        // Load all body part sprites from the selected category folder
        string folderPath = Path.Combine(Application.streamingAssetsPath, artStyleFolders[0], categoryName);
        string[] bodyPartPaths = Directory.GetFiles(folderPath, "*.png");

        if (bodyPartPaths.Length > 0)
        {
            // Select a random body part from the category
            string randomBodyPartPath = bodyPartPaths[Random.Range(0, bodyPartPaths.Length)];

            // Load body part sprite
            byte[] fileData = File.ReadAllBytes(randomBodyPartPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            Sprite randomBodyPartSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // Update the corresponding Image component with the selected random body part
            OnBodyPartButtonClicked(categoryName, randomBodyPartSprite);
        }
    }
}
