using System.Collections;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using JetBrains.Annotations;
using UnityEngine;
using Firebase;
using System;
public class AdsManager : MonoBehaviour
{
    public static Action rewardAction;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #region Remote config values
    public static int RC_Inter_timer_ad = 240;
    public static int RC_powerup_count = 3;
    public static bool RC_Inter_play_button = true;
    public static bool RC_Inter_back_button = true;
    public static bool RC_Banner = true;
    public static bool RC_Inter_mobl_leave = false;
    public static bool RC_back_btn_mrec = true;
    public static bool RC_settings_mrec = true;
    public static bool RC_Play_mrec = false;
    public static bool RC_zombie_mod_mrec = true;
    public static bool RC_mega_rmp_mrec = true;

    public static bool RC_mobl_spwn = false;
    public static bool RC_Rewarded_cheats_unlk_ad = true;
    public static bool RC_AppOpen = true;
    public static bool RC_inter_cht_cod_pnl_leave = true;

    // for immersive
    public static bool RC_Im_hs_1 = true;
    public static bool RC_Im_hs_2 = true;
    public static bool RC_Im_hs_3 = true;
    public static bool RC_Im_hs_4 = true;
    public static bool RC_im_mega_bilbrd = true;
    public static bool RC_im_ramp_end_1 = true;
    public static bool RC_im_ramp_end_2 = true;
    public static bool RC_im_ramp_strt_1 = true;
    public static bool RC_im_ramp_strt_2 = true;
    public static bool RC_im_front_road_bilbrd_1 = true;
    public static bool RC_im_front_road_bilbrd_2 = true;
    public static bool RC_im_left_road_bilbrd_1 = true;
    public static bool RC_im_left_road_bilbrd_2 = true;
    public static bool RC_im_left_road_bilbrd_3 = true;
    public static bool RC_mode_unlock = true;
    public static bool RC_im_cheat_code = true;
    public static bool RC_Car_Door_Enter = true;
    public static bool RC_mobl_spwn_inter = false;


    public static bool isZombieModeUnlock = false;
    public static bool isMegaRampUnlock = false;

    public void Fetch_Inter_timer_ad_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_timer_ad = data.DefaultValue_Number;
    }
    public void Fetch_powerup_count_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_powerup_count = data.DefaultValue_Number;
    }
    public void Fetch_Inter_play_button_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_play_button = data.DefaultValue_Boolean;
    }
    public void Fetch_Inter_back_button_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_back_button = data.DefaultValue_Boolean;
    }
    public void Fetch_Banner_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Banner = data.DefaultValue_Boolean;
    }

    public void Fetch_Inter_mobl_leave_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_mobl_leave = data.DefaultValue_Boolean;
    }

    public void Fetch_back_btn_mrec_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_back_btn_mrec = data.DefaultValue_Boolean;
    }
    public void Fetch_settings_mrec_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_settings_mrec = data.DefaultValue_Boolean;
    }
    public void Fetch_Play_mrec_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Play_mrec = data.DefaultValue_Boolean;
    }
    public void Fetch_zombie_mod_mrec_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_zombie_mod_mrec = data.DefaultValue_Boolean;
    }
    public void Fetch_mega_rmp_mrec_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_mega_rmp_mrec = data.DefaultValue_Boolean;
    }
    public void Fetch_mobl_spwn_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_mobl_spwn = data.DefaultValue_Boolean;
    }
    public void Fetch_Rewarded_cheats_unlk_ad_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Rewarded_cheats_unlk_ad = data.DefaultValue_Boolean;
    }

    public void Fetch_App_Open_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_AppOpen = data.DefaultValue_Boolean;
    }

    public void Fetch_inter_cht_cod_pnl_leave_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_inter_cht_cod_pnl_leave = data.DefaultValue_Boolean;
    }


    // immersive

    public void Fetch_Im_hs_1_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Im_hs_1 = data.DefaultValue_Boolean;
    }
    public void Fetch_Im_hs_2_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Im_hs_2 = data.DefaultValue_Boolean;
    }

    public void Fetch_Im_hs_3_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Im_hs_3 = data.DefaultValue_Boolean;
    }

    public void Fetch_Im_hs_4_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Im_hs_4 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_mega_bilbrd_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_mega_bilbrd = data.DefaultValue_Boolean;
    }

    public void Fetch_im_ramp_end_1_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_ramp_end_1 = data.DefaultValue_Boolean;
    }
    public void Fetch_im_ramp_end_2_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_ramp_end_2 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_ramp_strt_1_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_ramp_strt_1 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_ramp_strt_2_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_ramp_strt_2 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_front_road_bilbrd_1_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_front_road_bilbrd_1 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_front_road_bilbrd_2_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_front_road_bilbrd_2 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_left_road_bilbrd_1_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_left_road_bilbrd_1 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_left_road_bilbrd_2_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_left_road_bilbrd_2 = data.DefaultValue_Boolean;
    }

    public void Fetch_im_left_road_bilbrd_3_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_left_road_bilbrd_3 = data.DefaultValue_Boolean;
    }

    public void Fetch_mode_unlock_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_mode_unlock = data.DefaultValue_Boolean;
    }

    public void Fetch_im_cheat_code_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_im_cheat_code = data.DefaultValue_Boolean;
    }

    public void Fetch_RC_Car_Door_Enter_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Car_Door_Enter = data.DefaultValue_Boolean;
    }
    public void Fetch_RC_mobl_spwn_inter_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_mobl_spwn_inter = data.DefaultValue_Boolean;
    }



    // other Ads





    #endregion

    public static void ShowBanner()
    {
        if (RC_Banner)
            AdController.instance?.ShowBannerAd(AdController.BannerAdTypes.BANNER);
    }
    public static void HideBanner()
    {
        AdController.instance?.HideBannerAd(AdController.BannerAdTypes.BANNER);
    }
    public static bool IsInterstitialAvailable()
    {
        return AdController.instance != null && AdController.instance.IsInterstitialAdAvailable();
    }
    public static void ShowInterstitilAd(string placement)
    {
        AdController.instance?.ShowAd(AdController.AdType.INTERSTITIAL, placement);
    }

    public static bool IsRewardedAvailable()
    {
        return AdController.instance != null && AdController.instance.IsRewardedAdAvailable();
    }

    public static void ShowRewardedAd(Action _rewardAction, string placement)
    {
        rewardAction = null;
        rewardAction = _rewardAction;
        AdController.gaveRewardMethod += GiveReward;
        AdController.instance?.ShowAd(AdController.AdType.REWARDED, placement);
        SendFirebaseEevents("Rewarded_ad_" + placement);
    }

    public static void GiveReward()
    {
        AdController.gaveRewardMethod -= GiveReward;
        rewardAction?.Invoke();
    }

    public static void ShowMrec()
    {
        if (AdController.instance != null)
            AdController.instance.ShowBannerAd(AdController.BannerAdTypes.NATIVE);
    }

    public static void ChangeMrecPosition(MaxSdkBase.AdViewPosition NativeBannerPos)
    {
        if (AdController.instance != null)
        {
            // Implementation here
        }
    }
    public static void HideMrec()
    {
        if (AdController.instance != null)
            AdController.instance.HideBannerAd(AdController.BannerAdTypes.NATIVE);
    }

    public static void SendFirebaseEevents(string eventName)
    {
        if (GVAnalysisManager.instance != null)
        {
            GVAnalysisManager.instance.CustomEvent(eventName);
        }
    }
}
