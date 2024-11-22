
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5||USE_ADMOB_REWARDED_INTERSITIAL
using System.Collections;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
#endif

using System.Collections;
using UnityEngine;
using System;
public class AdmobInitlizationManager : GenericSingletonClass<AdmobInitlizationManager>
{


    public delegate void admobSDKInitializationAddition();
    public static event admobSDKInitializationAddition admobSdkInitializationAddition;
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5 || USE_ADMOB_REWARDED_INTERSITIAL
    private IEnumerator Start()
    {
        if (Time.timeScale != 0)
        {
            yield return new WaitForSeconds(1);
        }
        else {
            yield return new WaitForSecondsRealtime(2);
        }
        try
        {
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            InitializeGoogleMobileAds();
        }
        catch (Exception e)
        {

        }

    }

    private void InitializeGoogleMobileAds()
    {

        MobileAds.Initialize(status => {
            AdConstants.isAdmobSDKInitilized = true;
            if (admobSdkInitializationAddition != null) {
                admobSdkInitializationAddition.Invoke();
            }
        });
    }
#endif
}
