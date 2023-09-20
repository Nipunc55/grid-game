using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetector : MonoBehaviour
{
   public BannerAds bannerAd;

     private void OnEnable()
    {
        // This code is executed when the GameObject becomes active (enabled).
        Debug.Log("GameObject is now active.");
        bannerAd.DestroyAd();
        
        
    }

    private void OnDisable()
    {
        // This code is executed when the GameObject becomes inactive (disabled).
        Debug.Log("GameObject is now inactive.");
        bannerAd.LoadAd();
        
    }
}
