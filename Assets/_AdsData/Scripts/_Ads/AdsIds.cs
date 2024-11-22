
using Sirenix.OdinInspector;
using UnityEngine;
public static class AdsIds
{
    #if USE_MAX
    public static string SDKKey()
    {
        #if UNITY_ANDROID
            return "123456789";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    public static string InterstitialAdUnitId()
    {
        #if UNITY_ANDROID
            return "asdfasdfadsfasdfads";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    public static string RewardedAdUnitId()
    {
        #if UNITY_ANDROID
            return "adsfasdfasdfasdfasd";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    public static string BannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "adsfasdfasdfasdfasdf";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    public static string MRECBannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "asdfasdfasdfasdfadsf";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    #if USE_IDLE_MREC
    public static string IdleMRECBannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "ec6805c550a1dd8d";
        #elif UNITY_IOS
            return "ec6805c550a1dd8d";
        #else
            return "";
        #endif
    }
    #endif
    #if USE_MAX && USE_MAX_OPENADS
    public static string AppOpenAdUnitId_MAX()
    {
        #if UNITY_ANDROID
            return "adsfadfasdfasdfasdfas";
        #elif UNITY_IOS
            return "";
        #else
            return "";
        #endif
    }
    #endif
    #endif
    #if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
    public static string AppOpenAdTier1()
    {
        #if UNITY_ANDROID
            return "ca-app-pub-4921234158243313/2036859603";
        #elif UNITY_IOS
            return "ca-app-pub-4921234158243313/8787860970";
        #else
            return "";
        #endif
    }
    public static string AppOpenAdTier2()
    {
        #if UNITY_ANDROID
            return "ca-app-pub-4921234158243313/1976369608";
        #elif UNITY_IOS
            return "ca-app-pub-4921234158243313/8162557901";
        #else
            return "";
        #endif
    }
    public static string AppOpenAdTier3()
    {
        #if UNITY_ANDROID
            return "ca-app-pub-4921234158243313/4107708921";
        #elif UNITY_IOS
            return "ca-app-pub-4921234158243313/7970986216";
        #else
            return "";
        #endif
    }
    #endif
    #if USE_ADMOB_REWARDED_INTERSITIAL
    public static string AdmobRewardedIntersitialUnitId()
    {
        #if UNITY_ANDROID
            return "ca-app-pub-4921234158243313/9479401200";
        #elif UNITY_IOS
            return "ca-app-pub-4921234158243313/1391536937";
        #else
            return "";
        #endif
    }
    #endif

    
    #if USE_MAX_LOW_INTERSITITAL
    public static string LowMaxIntersititialAdUnitId()
    {
        #if UNITY_ANDROID
            return "bc13bc07f5ff02e6";
        #elif UNITY_IOS
            return "7f220a28f7716a36";
        #else
            return "";
        #endif
    }
#endif

}