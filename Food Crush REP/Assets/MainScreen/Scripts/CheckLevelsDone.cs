using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.ComponentModel;

public class CheckLevelsDone : MonoBehaviour
{
    public Button[] buttons;
    public List<LevelData> levelDataList;
    private string savePath;
    //public static int ClickedLevel;
    public static int ClickedLevel = -1;
    public GameObject LevelInfo;
     LevelInfoSetUp levelinfoscript;
    public List<int> LevelMoves;
    public void ChangeClickedLevel(int numbelevel)
    {
        ClickedLevel = numbelevel;
    }
    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "playerProgress.json");
        levelinfoscript = LevelInfo.GetComponent<LevelInfoSetUp>();
    }
/*    void Start()
    {

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            levelDataList = JsonConvert.DeserializeObject<List<LevelData>>(json);
            if (ClickedLevel != -1)
            {
                if (LevelOverPanel.score > levelDataList[ClickedLevel].score)
                {
                    levelDataList[ClickedLevel].score = LevelOverPanel.score;
                    levelDataList[ClickedLevel].starsCollected = LevelOverPanel.stars;
                }
                if (levelDataList[ClickedLevel].passed)
                {
                    CheckToUnlockNextLevel(ClickedLevel);
                }
                SaveTheData();
                ClickedLevel = -1;
            }
            ChangeAccordingtoSave();
        }
        else
        {

            levelDataList = new List<LevelData>();

            for (int i = 0; i < 6; i++)
            {
                LevelData levelData = new LevelData();
                levelData.passed = false;
                levelData.score = 0;
                levelData.starsCollected = 0;
                levelData.moves = LevelMoves[i];
                levelDataList.Add(levelData);
            }
            SaveTheData();
            ChangeAccordingtoSave();


        }

    }*/
    private void OnEnable()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            levelDataList = JsonConvert.DeserializeObject<List<LevelData>>(json);
            if (ClickedLevel != -1)
            {
                if (LevelOverPanel.score > levelDataList[ClickedLevel].score)
                {
                    levelDataList[ClickedLevel].score = LevelOverPanel.score;
                    levelDataList[ClickedLevel].starsCollected = LevelOverPanel.stars;
                }
                if (levelDataList[ClickedLevel].passed)
                {
                    levelDataList[ClickedLevel].passed = LevelOverPanel.passed;

                    // CheckToUnlockNextLevel(ClickedLevel);
                }
                SaveTheData();
                ClickedLevel = -1;
            }
            ChangeAccordingtoSave();
        }
        else
        {

            levelDataList = new List<LevelData>();

            for (int i = 0; i < 6; i++)
            {
                LevelData levelData = new LevelData();
                levelData.passed = false;
                levelData.score = 0;
                levelData.starsCollected = 0;
                levelData.moves = LevelMoves[i];
                levelDataList.Add(levelData);
            }
            SaveTheData();
            ChangeAccordingtoSave();
        }
     }
    private void CheckToUnlockNextLevel(int clickedlevel)
    {
        if (levelDataList[clickedlevel + 1].passed == false)
        {
            buttons[clickedlevel + 1].interactable = true;
            buttons[clickedlevel + 1].GetComponent<LoadSceneLevel>().SetThelevelValue(0, 0, clickedlevel+ 1, levelDataList[clickedlevel+1].moves, false);
            levelDataList[clickedlevel].passed = LevelOverPanel.passed;
        }
    }
    public void ResetProgres()
    {
        if (File.Exists(savePath)){
            File.Delete(savePath);
        }
    }
    public void ChangeNumbet()
    {

        SaveTheData();
    }
    public void SaveTheData()
    {
        string json = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }
    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            levelDataList = JsonConvert.DeserializeObject<List<LevelData>>(json);

        }
        else
        {
            levelDataList = new List<LevelData>();

            for (int i = 0; i < 6; i++)
            {
                LevelData levelData = new LevelData();
                levelData.passed = false;
                levelData.score = 0;
                levelData.starsCollected = 0;
                levelDataList.Add(levelData);
            }

        }
    }
    public void ChangeLevelData()
    {
        int levelNumber = 1;
        bool passed = true;
        int score = 100;
        int starsCollected = 1;
        if (levelNumber >= 1 && levelNumber <= levelDataList.Count)
        {
            LevelData levelData = levelDataList[levelNumber - 1];
            levelData.passed = passed;
            levelData.score = score;
            levelData.starsCollected = starsCollected;

            SaveTheData(); // Save the updated data to the file
        }
        else
        {
            Debug.LogWarning("Invalid level number!");
        }
    }
    public void ChangeAccordingtoSave()
    {
        if (levelDataList[0].passed == false)
        {
            buttons[0].interactable = true;
            buttons[0].GetComponent<LoadSceneLevel>().SetThelevelValue(levelDataList[0].score, levelDataList[0].starsCollected, 0, levelDataList[0].moves, false);
        }
        for (int i = 1; i < buttons.Length; i++)
        {
            if (levelDataList[i].passed)
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<LoadSceneLevel>().SetThelevelValue(levelDataList[i].score, levelDataList[i].starsCollected, i, levelDataList[i].moves, true);

            }
            else if (levelDataList[i-1].passed)
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<LoadSceneLevel>().SetThelevelValue(levelDataList[i].score, levelDataList[i].starsCollected, i, levelDataList[i].moves, false);
                break;
            }
        }
   

    }
    public void ButtonClickedLevelInfo(int levelnum, int score, int stars, bool passed)
    {
        LevelInfo.SetActive(true);
        levelinfoscript.SetUpTheLevelInfoPanel(levelnum, score, stars, passed);
        this.gameObject.SetActive(false);
    }
}
