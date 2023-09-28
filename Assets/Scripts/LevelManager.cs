using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
[Serializable]
public class Level
{
    public int gridSizeX;
    public int gridSizeY;
    public bool levelCompleted = false;

    public List<CellData> resultCells = new List<CellData>();

    [Serializable]
    public class CellData
    {
        public Vector2 cellIndex;
        public int cellResultCount;
    }


}


[Serializable]
public class LevelData
{
    public int level;
    public int gridSizeX;
    public int gridSizeY;
    public bool levelCompleted = false;

    public List<CellData> resultCells = new List<CellData>();

    [Serializable]
    public class CellData
    {
        public Vector2 cellIndex;
        public int cellResultCount;
    }


}


public class AllLevels
{
    public List<LevelData> Levels;

}

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public TileMap tileMap;
    public List<Level> levels = new List<Level>();
    public List<LevelData> levelsData = new List<LevelData>();
    public GameObject levelButton;
    public Transform[] pages;
    public int currentLvelIndex;
    public LevelData currentLevel;
    private const string LevelKey = "CurrentLevel";
    public GameObject menu;
    public Text levelText;
    public Text winPanelLevelText;
    private int totalLevels = 100;
    public bool[] completedLevels = new bool[100];
    void Start()
    {
        // currentLvelIndex = LoadLevel();
        currentLevel = levelsData[currentLvelIndex];
        UpdateSliderValue(currentLvelIndex);

        CreateLevelMenu();
        CheckCompletedLevels();
    }
    public void CreateLevelMenu()
    {
        for (int i = 0; i < 100; i++)
        {
            int pageNum = i / 20;
            // Debug.Log(""+pageNum);
            GameObject instance = Instantiate(levelButton, Vector3.zero, Quaternion.identity);
            Text textComponent = instance.transform.GetChild(0).GetComponent<Text>();
            textComponent.text = (i + 1).ToString(); ;
            instance.transform.SetParent(pages[pageNum], false);
        }
    }

    public void NextLevel()
    {
       
        currentLvelIndex++;
        SaveLevel(currentLvelIndex);

        currentLevel = levelsData[currentLvelIndex];
        UpdateSliderValue(currentLvelIndex);
    }
    public void MarkLevelComplete()
    {
        Debug.Log("mark level completed");
        completedLevels[currentLvelIndex] = true;
        gameManager.SaveUserData();
        CheckCompletedLevels();
        NextLevel();
    }
    public void SkipLevel()
    {
        Debug.Log("Level Up");
        currentLvelIndex++;
        SaveLevel(currentLvelIndex);

        currentLevel = levelsData[currentLvelIndex];
        UpdateSliderValue(currentLvelIndex);
    }



    // Function to save the current level number
    public static void SaveLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(LevelKey, levelNumber);
        PlayerPrefs.Save();
    }

    // Function to load the saved level number
    public static int LoadLevel()
    {
        if (PlayerPrefs.HasKey(LevelKey))
        {
            return PlayerPrefs.GetInt(LevelKey);
        }
        else
        {
            return 0; // Default level number if not found
        }
    }
    public void SetLevelText()
    {
        UpdateSliderValue(currentLvelIndex);


    }
    public void setCurrentLevel()
    {

    }
    private void UpdateSliderValue(int level)
    {
        levelText.text = (level + 1).ToString();
        winPanelLevelText.text = "Level " + (level + 1).ToString();
        // float sliderValue = (float)(level + 1) / totalLevels;
        // slider.value = sliderValue;
    }


    public void Savejson()
    {
        AllLevels lvs = new AllLevels();
        string path = "/Levels/lvs.json";

        lvs.Levels = levelsData;

        string json = JsonUtility.ToJson(lvs, true);
        File.WriteAllText(Application.dataPath + path, json);
    }

    public void Loadjson()
    {
        string path = "/Levels/lvs.json";

        string json = File.ReadAllText(Application.dataPath + path);
        AllLevels lvs = JsonUtility.FromJson<AllLevels>(json);
        levelsData = lvs.Levels;
    }

    public void PlayCustomLevel(int index)
    {
        currentLvelIndex = index;
        currentLevel = levelsData[index];
        tileMap.StartGame();
        menu.gameObject.SetActive(false);
        UpdateSliderValue(index);

    }
    void CheckCompletedLevels()
    {

        for (int i = 0; i < completedLevels.Length; i++)
        {
            if (completedLevels[i])
            {
                MarkLevelCompleted(i);

            }

        }
    }
    int pageNum = 0;
    int levelsPerPage = 16;
    void MarkLevelCompleted(int _index)
    {

        // if(_index>0 && (_index%16) ==0 )pageNum++;
        pageNum = _index / levelsPerPage;
        int _currentIndex = _index - (levelsPerPage * pageNum);
        Debug.Log(_currentIndex);
        Debug.Log("pageNUm :" + pageNum + " _index :" + _index);
        pages[pageNum].transform.GetChild(_currentIndex).GetChild(0).GetComponent<Ricimi.AnimatedButton>().interactable = true;

        for (int i = 1; i < 4; i++)
        {
            pages[pageNum].transform.GetChild(_currentIndex).GetChild(i).GetChild(3).gameObject.SetActive(true);

        }



    }
}
