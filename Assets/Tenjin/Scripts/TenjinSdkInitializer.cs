using System;
using System.Collections.Generic;
using Google.MiniJSON;
using UnityEngine;
using UnityEngine.Purchasing;

public class TenjinSdkInitializer : MonoBehaviour
{
    public static TenjinSdkInitializer initializer;

    [SerializeField]
    private string tenjinKey;
    BaseTenjin instance;


    private void Awake()
    {
        initializer = this;
    }

    void Start()
    {
        TenjinConnect();
    }


    public void TenjinConnect()
    {
        instance = Tenjin.getInstance(tenjinKey);
#if UNITY_IOS
        instance.SetAppStoreType(AppStoreType.other);
#elif UNITY_ANDROID
        instance.SetAppStoreType(AppStoreType.googleplay);
#endif
        instance.Connect();

#if tenjin_applovin_enabled
        //Applovin
        instance.SubscribeAppLovinImpressions();
#endif
    }

#if tenjin_admob_enabled
    //ADMOB 
    public void AdmobAppOpenSubscription(object appOpen, string adUnitId)
    {
        instance.SubscribeAdMobAppOpenViewImpressions(appOpen, adUnitId);
    }

    public void AdmobBannerSubscription(object bannerView, string adUnitId)
    {
        instance.SubscribeAdMobBannerViewImpressions(bannerView, adUnitId);
    }

    public void AdmobAdaptiveBannerViewSubscription(object adaptiveBannerView, string adUnitId)
    {
        instance.SubscribeAdMobAdaptiveBannerViewImpressions(adaptiveBannerView, adUnitId);
    }

    public void AdmobMRecSubscription(object mrecView, string adUnitId)
    {
        instance.SubscribeAdMobMRecImpressions(mrecView, adUnitId);
    }

    public void AdmobInterstitialStaticSubscription(object interstitialAd, string adUnitId)
    {
        instance.SubscribeAdMobInterstitialAdStaticImpressions(interstitialAd, adUnitId);
    }

    public void AdmobInterstitialVideoSubscription(object interstitialAd, string adUnitId)
    {
        instance.SubscribeAdMobInterstitialAdVideoImpressions(interstitialAd, adUnitId);
    }

    public void AdmobRewardedSubscription(object rewardedAd, string adUnitId)
    {
        instance.SubscribeAdMobRewardedAdImpressions(rewardedAd, adUnitId);
    }

    public void AdmobRewardedInterstitialSubscription(object rewardedInterstitialAd, string adUnitId)
    {
        instance.SubscribeAdMobRewardedInterstitialAdImpressions(rewardedInterstitialAd, adUnitId);
    }

#endif

    // IAP Tenjin
    public void OnProcessPurchase(PurchaseEventArgs purchaseEventArgs)
    {
        var price = purchaseEventArgs.purchasedProduct.metadata.localizedPrice;
        double lPrice = decimal.ToDouble(price);
        var currencyCode = purchaseEventArgs.purchasedProduct.metadata.isoCurrencyCode;

        var wrapper = Json.Deserialize(purchaseEventArgs.purchasedProduct.receipt) as Dictionary<string, object>;  // https://gist.github.com/darktable/1411710
        if (null == wrapper)
        {
            return;
        }

        var store = (string)wrapper["Store"]; // GooglePlay, AmazonAppStore, AppleAppStore, etc.
        var payload = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt. For Android, it is the raw JSON receipt.
        var productId = purchaseEventArgs.purchasedProduct.definition.id;


#if UNITY_ANDROID
        if (store.Equals("GooglePlay"))
        {
            var googleDetails = Json.Deserialize(payload) as Dictionary<string, object>;
            var googleJson = (string)googleDetails["json"];
            var googleSig = (string)googleDetails["signature"];

            CompletedAndroidPurchase(productId, currencyCode, 1, lPrice, googleJson, googleSig);
        }
#endif

#if UNITY_IPHONE

        var transactionId = purchaseEventArgs.purchasedProduct.transactionID;
        CompletedIosPurchase(productId, currencyCode, 1, lPrice, transactionId, payload);
#endif
    }

    private void CompletedIosPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string TransactionId, string Receipt)
    {
        instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, TransactionId, Receipt, null);
    }

    private void CompletedAndroidPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string Receipt, string Signature)
    {
        instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, Receipt, Signature);
    }




}

