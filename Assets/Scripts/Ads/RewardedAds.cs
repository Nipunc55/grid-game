using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;



public class RewardedAds : MonoBehaviour
{
    public TileMap tileMap;
    bool rewarded =false;
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });
    }
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-1937248389541205/9922855031";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-1937248389541205/9922855031";
#else
  private string _adUnitId = "unused";
#endif

  private RewardedAd rewardedAd;

  /// <summary>
  /// Loads the rewarded ad.
  /// </summary>
  public void LoadRewardedAd()
  {
      // Clean up the old ad before loading a new one.
      if (rewardedAd != null)
      {
            rewardedAd.Destroy();
            rewardedAd = null;
      }

      Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();
      adRequest.Keywords.Add("unity-admob-sample");

      // send the request to load the ad.
      RewardedAd.Load(_adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              rewardedAd = ad;
          });
  }
  public void ShowRewardedAd()
{
    const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    if (rewardedAd != null && rewardedAd.CanShowAd())
    {
        
            rewardedAd.Show((Reward reward) =>
            {
                // Debug.LogError("reward ad show");
                // TODO: Reward the user.
                // Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                rewarded=true;
               
                
                 
                
            });
        
        
    }
    
}
private void Update() {
    if(rewarded){
           tileMap.NextLevel();
           rewarded=false;
           LoadRewardedAd();

    }
}

}
