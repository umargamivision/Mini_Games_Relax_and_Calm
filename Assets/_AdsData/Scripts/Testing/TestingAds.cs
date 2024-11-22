using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TestingAds : MonoBehaviour
{
    public Text debugText;
    Vector2 textSize;


    void OnEnable()
    {

        if (debugText) textSize = debugText.gameObject.GetComponent<RectTransform>().sizeDelta;
        Application.logMessageReceived += ShowLogsOnText;

    }

    void OnDisable()
    {
        Application.logMessageReceived -= ShowLogsOnText;
     

    }

    public void ShowAppOpen()
    {
         if (AdController.instance)
            AdController.instance.ShowAppLovinAppOpen();
    }

    public void ShowDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }
    public void ShowBanner()
    {
        if (AdController.instance)
            AdController.instance.ShowBannerAd(AdController.BannerAdTypes.BANNER);//(AdController.BannerAdTypes.BANNER);

    }
    public void HideBanner()
    {
        if (AdController.instance)
            AdController.instance.HideBannerAd(AdController.BannerAdTypes.BANNER);//(AdController.BannerAdTypes.BANNER);
    }

    public void DestroyBanner()
    {
        if (AdController.instance)
            AdController.instance.DestroyBannerAd(AdController.BannerAdTypes.BANNER);//(AdController.BannerAdTypes.BANNER);
    }

    public void ShowAdaptiveBanner()
    {
        if (AdController.instance)
            AdController.instance.ShowBannerAd(AdController.BannerAdTypes.ADAPTIVE);
    }

    public void DestroyAdaptiveBanner()
    {
        if (AdController.instance)
            AdController.instance.DestroyBannerAd(AdController.BannerAdTypes.ADAPTIVE);//(AdController.BannerAdTypes.ADAPTIVE);
    }

    public void HideAdaptiveBanner()
    {
        if (AdController.instance)
            AdController.instance.HideBannerAd(AdController.BannerAdTypes.ADAPTIVE);//(AdController.BannerAdTypes.ADAPTIVE);
    }

    public void ShowNativeBanner()
    {
        if (AdController.instance)
            AdController.instance.ShowBannerAd(AdController.BannerAdTypes.NATIVE);// (AdController.BannerAdTypes.NATIVE);
    }

    public void ShowIdleNativeBanner()
    {
#if USE_IDLE_MREC
        if (IdleMrecManager.Instance)
            IdleMrecManager.Instance.ShowIdleMRec();// (AdController.BannerAdTypes.NATIVE);
#endif
    }

    public void HideNativeBanner()
    {
        if (AdController.instance)
            AdController.instance.HideBannerAd(AdController.BannerAdTypes.NATIVE);//(AdController.BannerAdTypes.NATIVE);
    }
    public void HideIdleNativeBanner()
    {
#if USE_IDLE_MREC
        if (IdleMrecManager.Instance)
            IdleMrecManager.Instance.HideIdleMRec();//(AdController.BannerAdTypes.NATIVE);
#endif
    }

    public void DestroyNativeBanner()
    {
        if (AdController.instance)
            AdController.instance.DestroyBannerAd(AdController.BannerAdTypes.NATIVE);//(AdController.BannerAdTypes.NATIVE);
    }

    public void RequestStaticAd()
    {
        if (AdController.instance) 
            AdController.instance.LoadAd(AdController.AdType.STATIC);

            
    }

    public void ShowStaticAd()
    {
        if (AdController.instance)
        {
            AdController.instance.ShowAd(AdController.AdType.STATIC, "interstitial_test");
            //AdConstants.currentState = AdConstants.States.OnPause;
            //AdController.instance.ChangeState();

        }

    }

    public void RequestInterstitilAd()
    {
        if (AdController.instance)
            AdController.instance.LoadAd(AdController.AdType.INTERSTITIAL);
    }

    public void ShowInterstitilAd()
    {
        if (AdController.instance)
        {
            //AdController.instance.ShowAd(AdController.AdType.STATIC);
            AdController.instance.ShowAd(AdController.AdType.INTERSTITIAL, "interstitial_test");
        }
    }


    public void RequestRewardedAd()
    {
        if (AdController.instance && !AdController.instance.IsRewardedAdAvailable())
            AdController.instance.LoadAd(AdController.AdType.REWARDED);
    }

    public void ShowRewardedAd()
    {
        if (AdController.instance)
        {
            AdController.instance.ShowAd(AdController.AdType.REWARDED, "reward_test");

        }
    }


    public void RequestRewardedInterstitialAd()
    {
        if (AdController.instance)
        {
            AdController.instance.LoadAd(AdController.AdType.REWARDED_INTERSTITIAL);
      
        }
    }

    public void ShowRewardedInterstitialAd()
    {
        if (AdController.instance)
        {
            AdController.instance.ShowAd(AdController.AdType.REWARDED_INTERSTITIAL, "reward_test");
        }
    }

    public void ShowRateUs()
    {
        if (AdController.instance)
        {
            AdController.instance.PromptRateMenu();
        }
    }





    public void ShowLogsOnText(string logString, string stackTrace, LogType type)
    {
        if (debugText)
            debugText.text = "\n======================================\n"+logString;
    }

    public void CheckAndShowRewardedAd()
    {
        if (AdController.instance.IsRewardedAdAvailable())
        {
            //AdController.instance.ShowAd(AdController.AdType.REWARDED);
            Debug.Log("========== Rewarded Ad Available ==========");

        }
        else
        {
            Debug.Log("========== No Rewarded Ad Available ==========");
        }
    }
}
