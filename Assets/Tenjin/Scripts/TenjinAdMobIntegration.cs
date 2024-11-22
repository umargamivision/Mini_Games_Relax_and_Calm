//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class TenjinAdMobIntegration
{
    // TODO: Potential issue with multiple ad types being subscribed to at once TENJIN-16020
    private static bool _sub_appOpen, _sub_banner, _sub_adaptive_banner, _sub_mrec, _sub_interstitial_static, _sub_interstitial_video, _sub_rewarded, _sub_rewarded_interstitial = false;
    public TenjinAdMobIntegration()
    {
    }
    public static void ListenForAppOpenViewImpressions(object appOpen, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_appOpen)
        {
            Debug.Log("Ignoring duplicate admob appOpen subscription");
            return;
        }
        GoogleMobileAds.Api.AppOpenAd newAppOpen = (GoogleMobileAds.Api.AppOpenAd)appOpen;
        newAppOpen.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newAppOpen.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                        value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing appOpen impression " + ex.ToString());
                }
            }
        };
        //_sub_appOpen = true;
#endif
    }
    public static void ListenForBannerViewImpressions(object bannerView, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_banner)
        {
            Debug.Log("Ignoring duplicate admob bannerView subscription");
            return;
        }
        GoogleMobileAds.Api.BannerView newBannerView = (GoogleMobileAds.Api.BannerView)bannerView;
        newBannerView.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newBannerView.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                            value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing bannerView impression " + ex.ToString());
                }
            }
        };
        //_sub_banner = true;
#endif
    }
    public static void ListenForAdaptiveBannerViewImpressions(object adaptiveBannerView, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_adaptive_banner)
        {
            Debug.Log("Ignoring duplicate admob bannerView subscription");
            return;
        }
        GoogleMobileAds.Api.BannerView newAdaptiveBannerView = (GoogleMobileAds.Api.BannerView)adaptiveBannerView;
        newAdaptiveBannerView.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newAdaptiveBannerView.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                        value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing bannerView impression " + ex.ToString());
                }
            }
        };
        //_sub_adaptive_banner = true;
#endif
    }
    public static void ListenForMRecViewImpressions(object mrecView, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_mrec)
        {
            Debug.Log("Ignoring duplicate admob mrecView subscription");
            return;
        }
        GoogleMobileAds.Api.BannerView newMRecView = (GoogleMobileAds.Api.BannerView)mrecView;
        newMRecView.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newMRecView.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                        value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing mrecView impression " + ex.ToString());
                }
            }
        };
        //_sub_mrec = true;
#endif
    }

    public static void ListenForInterstitialAdStaticImpressions(object interstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_interstitial_static)
        {
            Debug.Log("Ignoring duplicate admob interstitialAd static subscription");
            return;
        }
        GoogleMobileAds.Api.InterstitialAd newInterstitialAd = (GoogleMobileAds.Api.InterstitialAd)interstitialAd;
        newInterstitialAd.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newInterstitialAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                        value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing interstitialAd static impression " + ex.ToString());
                }
            }
        };
        //_sub_interstitial_static = true;
#endif
    }

    public static void ListenForInterstitialAdVideoImpressions(object interstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_interstitial_video)
        {
            Debug.Log("Ignoring duplicate admob interstitialAd video subscription");
            return;
        }
        GoogleMobileAds.Api.InterstitialAd newInterstitialAd = (GoogleMobileAds.Api.InterstitialAd)interstitialAd;
        newInterstitialAd.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newInterstitialAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                        value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing interstitialAd video impression " + ex.ToString());
                }
            }
        };
        //_sub_interstitial_video = true;
#endif
    }

    public static void ListenForRewardedAdImpressions(object rewardedAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_rewarded)
        {
            Debug.Log("Ignoring duplicate admob rewardedAd subscription");
            return;
        }
        GoogleMobileAds.Api.RewardedAd newRewardedAd = (GoogleMobileAds.Api.RewardedAd)rewardedAd;
        newRewardedAd.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newRewardedAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                            value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing rewardedAd impression " + ex.ToString());
                }
            }
        };
        //_sub_rewarded = true;
#endif
    }
    public static void ListenForRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        if (_sub_rewarded_interstitial)
        {
            Debug.Log("Ignoring duplicate admob rewardedInterstitialAd subscription");
            return;
        }
        GoogleMobileAds.Api.RewardedInterstitialAd newRewardedInterstitialAd = (GoogleMobileAds.Api.RewardedInterstitialAd)rewardedInterstitialAd;
        newRewardedInterstitialAd.OnAdPaid += (args) =>
        {
            GoogleMobileAds.Api.ResponseInfo responseInfo = newRewardedInterstitialAd.GetResponseInfo();
            if (responseInfo != null)
            {
                String adResponseId = responseInfo.GetResponseId();
                try
                {
                    AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                    {
                        ad_unit_id = adUnitId,
#if UNITY_ANDROID
                            value_micros = args.Value,
#elif UNITY_IPHONE
                        value_micros = (args.Value / 1000000.0),
#else
                            value_micros = args.Value,
#endif
                        currency_code = args.CurrencyCode,
                        response_id = adResponseId,
                        precision_type = args.Precision.ToString(),
                        mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                    };
                    string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing rewardedInterstitialAd impression " + ex.ToString());
                }
            }
        };
        //_sub_rewarded_interstitial = true;
#endif
    }
}

[System.Serializable]
internal class AdMobImpressionDataToJSON
{
    public string currency_code;
    public string ad_unit_id;
    public string response_id;
    public string precision_type;
    public string mediation_adapter_class_name;
#if UNITY_IPHONE
    public double value_micros;
#elif UNITY_ANDROID
    public long value_micros;
#else
    public long value_micros;
#endif
}