using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gamePlayPanel;

    

    public void OpenUrl(string url)
    {
       
        Application.OpenURL(url);
    }


    public void PlayButton()
    {

    }

}
