using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI diamondsText;
    public AudioSource bgMusic;
    public GameObject muteIcon;
    public GameObject sfxMuteIcon;
    public Sprite[] muteSprites;
    public GameObject storeGameObject;

    private bool _bgm;

    public bool BGM
    {
        get
        {
            return _bgm;
        }
        set
        {

            /*if (value == _bgm)
            {
                return;
            }*/
            _bgm = value;
            userData.bgm = value;
            bgMusic.mute = !value;
            muteIcon.GetComponent<Image>().sprite = value ? muteSprites[1] : muteSprites[0];
        }
    }

    private bool _sfx;

    public bool SFX
    {
        get
        {
            return _sfx;
        }
        set
        {

            /*if (value == _bgm)
            {
                return;
            }*/
            _sfx = value;
            userData.Sfx = value;
            sfxMuteIcon.GetComponent<Image>().sprite = value ? muteSprites[2] : muteSprites[3];
        }
    }

    [Header("Level Data")]
    public LevelManager gameData;
    public UserData userData;

    [System.Serializable]
    public class UserData
    {
        public bool AdsOn;
        public int Diamonds;
        public int CLevel;
        public bool bgm;
        public bool Sfx;
        public bool[] completedLevels = new bool[100];
    }

    void Awake()
    {
        // Adcon = adObject.GetComponent<GoogleAdMobController>();

        // Adcon.RequestBannerAd();
        // Adcon.RequestAndLoadInterstitialAd();

        LoadUserData();
    }
    void Start(){
        setDiamondCountText();
    }

    public void LoadUserData()
    {
         UserData udtemp = new UserData();
        udtemp.Diamonds = 10;
        udtemp.CLevel = 0;
        udtemp.bgm = true;
        udtemp.Sfx = true;
       
         for (int i = 0; i < 100; i++)  
          {
          udtemp.completedLevels[i] = false; // Initialize every even-indexed element to true, and odd-indexed elements to false
         }

        string udString = PlayerPrefs.GetString("udata", JsonUtility.ToJson(udtemp));

        udtemp = JsonUtility.FromJson<UserData>(udString);

        try
        {
            userData.Diamonds = udtemp.Diamonds;
        }
        catch (System.Exception e)
        {
            userData.Diamonds = 10;
            udtemp.Diamonds = 10;
        }

        try
        {
            userData.CLevel = udtemp.CLevel;
            userData.completedLevels=udtemp.completedLevels;
        }
        catch (System.Exception e)
        {
            userData.CLevel = 0;
            udtemp.CLevel = 0;
        }

        gameData.currentLvelIndex = userData.CLevel;
        gameData.SetLevelText();
        gameData.setCurrentLevel();
        gameData.completedLevels=userData.completedLevels;
        setDiamondCountText();


        try
        {
            userData.bgm = udtemp.bgm;
        }
        catch (System.Exception e)
        {
            userData.bgm = true;
            udtemp.bgm = true;
        }

        BGM = userData.bgm;

        try
        {
            userData.Sfx = udtemp.Sfx;
        }
        catch (System.Exception e)
        {
            userData.Sfx = true;
            udtemp.Sfx = true;
        }

        SFX = userData.Sfx;

        userData = udtemp;
    }

    public void SaveUserData()
    {
        userData.CLevel = gameData.currentLvelIndex;
        userData.completedLevels=gameData.completedLevels;
        UserData UdataTemp = new UserData();
        UdataTemp = userData;
        PlayerPrefs.SetString("udata", JsonUtility.ToJson(UdataTemp));
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {

        SaveUserData();
    }
    public bool BuyGems(int _gemCount)
    {
        if (userData.Diamonds > 0)
        {
            userData.Diamonds = userData.Diamonds - _gemCount;
            setDiamondCountText();
            SaveUserData();
            return true;

        }
        storeGameObject.SetActive(true);
        return false;





    }
    void setDiamondCountText()
    {
        diamondsText.text = userData.Diamonds.ToString();

    }
    public void MuteB()
    {
        BGM = !BGM;
    }

    public void sfxB()
    {
        SFX = !SFX;
    }
    public void BuyDiamonds(int count)
    {
        userData.Diamonds = userData.Diamonds + count;
        SaveUserData();
        setDiamondCountText();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
