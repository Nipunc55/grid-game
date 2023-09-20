using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonGridGenerator : MonoBehaviour
{
    [SerializeField]public GameObject buttonPrefab;
    [SerializeField]public GridLayoutGroup gridLayout;
    public int currentLevel = 25; // Hardcoded current level for now

    private void Start()
    {
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        for (int i = 1; i <= 100; i++)
        {
            GameObject instantiatedButton = Instantiate(buttonPrefab);
            instantiatedButton.transform.SetParent(gridLayout.transform, false);

            Button buttonComponent = instantiatedButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                Text buttonText = buttonComponent.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = i.ToString();
                }

                // Disable buttons above the current level and adjust opacity
                buttonComponent.interactable = i <= currentLevel;
                Color buttonColor = buttonComponent.image.color;
                buttonColor.a = buttonComponent.interactable ? 1.0f : 0.5f;
                buttonComponent.image.color = buttonColor;

                // Attach a click event to the button
                int levelToLoad = i;
                buttonComponent.onClick.AddListener(() => LoadLevel(levelToLoad));

            }
        }
    }
    private void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }
}

