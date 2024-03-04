using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.ResourceManagement.ResourceLocations;


public class CharacterCustomization : MonoBehaviour
{
    public Image bodyImage; // Reference to the Image component displaying the character's body
    public Image hairImage; // Reference to the Image component displaying the character's hair
    public Image hairFrontImage; // Reference to the Image component displaying the character's hair
    public Image eyesImage;
    public Image backgroundImage;
    // Add more Image references for other body parts as needed

    public Transform categoryMenuPanel; // Reference to the panel where category menu buttons will be added
    public Transform bodyPartPanel; // Reference to the panel where body part buttons will be added
    public GameObject categoryButtonPrefab; // Prefab for the category button
    public GameObject bodyPartButtonPrefab; // Prefab for the body part button

    private Dictionary<string, List<Sprite>> loadedBodyParts = new Dictionary<string, List<Sprite>>();

    void Start()
    {
        // Load all category folders from the specified path
        LoadCategories();
    }

    private void LoadCategories()
    {
        Addressables.LoadResourceLocationsAsync("BodyParts", typeof(Sprite)).Completed += OnLocationsLoaded;
    }

    private void OnLocationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> op)
    {
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var location in op.Result)
            {
                var categoryName = location.InternalId;

                // Create category button in the menu panel
                GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryMenuPanel);
                categoryButton.GetComponentInChildren<TextMeshProUGUI>().text = categoryName;
                categoryButton.GetComponent<Button>().onClick.AddListener(() => OnCategoryButtonClicked(categoryName));

                // Load body parts for the category
                var loadOperation = Addressables.LoadAssetsAsync<Sprite>(location, null);
                loadOperation.Completed += handle =>
                {
                    if (loadOperation.Result != null && loadOperation.Result.Count > 0)
                    {
                        loadedBodyParts[categoryName] = new List<Sprite>(loadOperation.Result);
                    }
                };
            }
        }
        else
        {
            Debug.LogError("Failed to load resource locations for BodyParts.");
        }
    }


    void OnCategoryButtonClicked(string categoryName)
    {
        // Clear existing body part buttons from the body part panel
        foreach (Transform child in bodyPartPanel)
        {
            Destroy(child.gameObject);
        }

        if (loadedBodyParts.ContainsKey(categoryName))
        {
            var bodyParts = loadedBodyParts[categoryName];

            // Create body part buttons in the body part panel
            foreach (Sprite bodyPart in bodyParts)
            {
                GameObject bodyPartButton = Instantiate(bodyPartButtonPrefab, bodyPartPanel);
                bodyPartButton.GetComponent<Image>().sprite = bodyPart;
                bodyPartButton.GetComponent<Button>().onClick.AddListener(() => OnBodyPartButtonClicked(categoryName, bodyPart));
            }
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
        if (loadedBodyParts.ContainsKey(categoryName))
        {
            var bodyParts = loadedBodyParts[categoryName];

            if (bodyParts.Count > 0)
            {
                // Select a random body part from the category
                Sprite randomBodyPart = bodyParts[UnityEngine.Random.Range(0, bodyParts.Count)];

                // Update the corresponding Image component with the selected random body part
                OnBodyPartButtonClicked(categoryName, randomBodyPart);
            }
        }
    }
}
