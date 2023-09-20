using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class HapticFeedbackManager : MonoBehaviour
{
    public static HapticFeedbackManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void LiteVibration()
    {
        if (PlayerPrefs.GetInt("HapticFeedback") == 0) return;
        HapticFeedback.LightFeedback();
    }
    public void MediumVibration()
    {
        if (PlayerPrefs.GetInt("HapticFeedback") == 0) return;
        HapticFeedback.MediumFeedback();
    }
    public void HeavyVibration()
    {
        // if (PlayerPrefs.GetInt("HapticFeedback") == 0) return;
        HapticFeedback.HeavyFeedback();
    }
}