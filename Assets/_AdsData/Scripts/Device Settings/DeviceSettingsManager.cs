using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class DeviceSettingsManager : MonoBehaviour
{
    public AdsSettings _adsSettingsPrefab;
    [HideInInspector]
    public AdsSettings _adsSettings;
    [HideInInspector]
    public string deviceSettingsJson;
    private AndroidDevice currentAndroidDevice;
    private IOSDevice currentIosDevice;
    private LowEndDevices lowendDevices;
    private bool showProperity;
    [ShowIf("showProperity")]
    [InfoBox("Your Device is in Current State")]
    [InfoBox("is low End Device")]
    public bool lowEndDeviceSettings;
    [ShowIf("showProperity")]
    [InfoBox("Is Limit Ads")]
    public bool LimitAdsDeviceSettings;
    [ShowIf("showProperity")]
    [InfoBox("Is low Memory Ads")]
    public bool LowMemoryAdsSettings;
    [ShowIf("showProperity")]
    [InfoBox("Ads Enabled")]
    public bool ShowBanner;
    [ShowIf("showProperity")]
    public bool ShowNativeBanner, ShowStatic, ShowInterstitial, ShowRewarded, ShowRewardedInterstitial, showAppOpenAd;
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    // For Developer use Only
    ////Start is called before the first frame update
    private string SaveJson(DeviceSettings settings)
    {
        DeviceSettingsJson deviceSettings = new DeviceSettingsJson();
        deviceSettings.platform = settings.platform.ToString();
        deviceSettings.androidSettings = new DeviceJson[settings.androidSettings.Length];
        for (int i = 0; i < settings.androidSettings.Length; i++)
        {
            deviceSettings.androidSettings[i] = new DeviceJson();
            deviceSettings.androidSettings[i].os = settings.androidSettings[i].os.ToString();
            deviceSettings.androidSettings[i].deviceHardware = settings.androidSettings[i].deviceHardware;
            deviceSettings.androidSettings[i].deviceHardware.onLowBatterySettings = settings.androidSettings[i].deviceHardware.onLowBatterySettings;
            deviceSettings.androidSettings[i].threshold_BatteryLevel = settings.androidSettings[i].deviceHardware.threshold_BatteryLevel;

        }
        deviceSettings.iosSettings = new DeviceJson[settings.iosSettings.Length];
        for (int i = 0; i < settings.iosSettings.Length; i++)
        {
            deviceSettings.iosSettings[i] = new DeviceJson();
            deviceSettings.iosSettings[i].os = settings.iosSettings[i].os.ToString();
            deviceSettings.iosSettings[i].deviceHardware = settings.iosSettings[i].deviceHardware;
            deviceSettings.iosSettings[i].deviceHardware.onLowBatterySettings = settings.iosSettings[i].deviceHardware.onLowBatterySettings;
            deviceSettings.iosSettings[i].threshold_BatteryLevel = settings.iosSettings[i].deviceHardware.threshold_BatteryLevel;
        }
        deviceSettings.lowEndDeviceSettings = settings.lowEndDeviceSettings;
        deviceSettings.limitadsettings = settings.limitadsettings;
        deviceSettings.lowMemorySettings = settings.lowMemorySettings;
        string jsonString = JsonUtility.ToJson(deviceSettings);
        Debug.Log("Json String: " + jsonString + "\n ============= Json Ends here ==============");
        return jsonString;
    }
    public void GetJsonText(string deviceSettingsJson)
    {

        DeviceSettingsJson jsonSettings = JsonUtility.FromJson<DeviceSettingsJson>(deviceSettingsJson);
        if (jsonSettings != null)
        {
            _adsSettings.deviceSettings.platform = (DeviceType)System.Enum.Parse(typeof(DeviceType), jsonSettings.platform);

            _adsSettings.deviceSettings.androidSettings = new AndroidDevice[jsonSettings.androidSettings.Length];

            for (int i = 0; i < jsonSettings.androidSettings.Length; i++)
            {
                _adsSettings.deviceSettings.androidSettings[i] = new AndroidDevice();
                _adsSettings.deviceSettings.androidSettings[i].os = (AndroidOS)System.Enum.Parse(typeof(AndroidOS), jsonSettings.androidSettings[i].os);
                _adsSettings.deviceSettings.androidSettings[i].deviceHardware = jsonSettings.androidSettings[i].deviceHardware;
                _adsSettings.deviceSettings.androidSettings[i].deviceHardware.onLowBatterySettings = jsonSettings.androidSettings[i].deviceHardware.onLowBatterySettings;
                _adsSettings.deviceSettings.androidSettings[i].deviceHardware.threshold_BatteryLevel = jsonSettings.androidSettings[i].threshold_BatteryLevel;
            }
            _adsSettings.deviceSettings.iosSettings = new IOSDevice[jsonSettings.iosSettings.Length];

            for (int i = 0; i < jsonSettings.iosSettings.Length; i++)
            {
                _adsSettings.deviceSettings.iosSettings[i] = new IOSDevice();
                _adsSettings.deviceSettings.iosSettings[i].os = (IOSOS)System.Enum.Parse(typeof(IOSOS), jsonSettings.iosSettings[i].os);
                _adsSettings.deviceSettings.iosSettings[i].deviceHardware = jsonSettings.iosSettings[i].deviceHardware;
                _adsSettings.deviceSettings.iosSettings[i].deviceHardware.onLowBatterySettings = jsonSettings.iosSettings[i].deviceHardware.onLowBatterySettings;

                _adsSettings.deviceSettings.iosSettings[i].deviceHardware.threshold_BatteryLevel = jsonSettings.iosSettings[i].threshold_BatteryLevel;
            }

            _adsSettings.deviceSettings.lowEndDeviceSettings = jsonSettings.lowEndDeviceSettings;
            _adsSettings.deviceSettings.limitadsettings = jsonSettings.limitadsettings;
            _adsSettings.deviceSettings.lowMemorySettings = jsonSettings.lowMemorySettings;
            Debug.Log("Device settings are: " + _adsSettings.deviceSettings.androidSettings[0].os);
        }
    }


    public void ApplyDeviceSettings()
    {
        _adsSettings = Instantiate(_adsSettingsPrefab, this.transform);
        if (string.IsNullOrEmpty(deviceSettingsJson))
            deviceSettingsJson = JsonUtility.ToJson(_adsSettings.deviceSettings);

        DeviceSettingsJson jsonSettings = JsonUtility.FromJson<DeviceSettingsJson>(deviceSettingsJson);
        if (jsonSettings != null)
        {
            _adsSettings.deviceSettings.platform = (DeviceType)System.Enum.Parse(typeof(DeviceType), jsonSettings.platform);

            _adsSettings.deviceSettings.androidSettings = new AndroidDevice[jsonSettings.androidSettings.Length];

            for (int i = 0; i < jsonSettings.androidSettings.Length; i++)
            {
                _adsSettings.deviceSettings.androidSettings[i] = new AndroidDevice();
                _adsSettings.deviceSettings.androidSettings[i].os = (AndroidOS)System.Enum.Parse(typeof(AndroidOS), jsonSettings.androidSettings[i].os);
                _adsSettings.deviceSettings.androidSettings[i].deviceHardware = jsonSettings.androidSettings[i].deviceHardware;
                _adsSettings.deviceSettings.androidSettings[i].deviceHardware.onLowBatterySettings = jsonSettings.androidSettings[i].deviceHardware.onLowBatterySettings;

                _adsSettings.deviceSettings.androidSettings[i].deviceHardware.threshold_BatteryLevel = jsonSettings.androidSettings[i].threshold_BatteryLevel;
            }
            _adsSettings.deviceSettings.iosSettings = new IOSDevice[jsonSettings.iosSettings.Length];

            for (int i = 0; i < jsonSettings.iosSettings.Length; i++)
            {
                _adsSettings.deviceSettings.iosSettings[i] = new IOSDevice();
                _adsSettings.deviceSettings.iosSettings[i].os = (IOSOS)System.Enum.Parse(typeof(IOSOS), jsonSettings.iosSettings[i].os);
                _adsSettings.deviceSettings.iosSettings[i].deviceHardware = jsonSettings.iosSettings[i].deviceHardware;
                _adsSettings.deviceSettings.iosSettings[i].deviceHardware.onLowBatterySettings = jsonSettings.iosSettings[i].deviceHardware.onLowBatterySettings;
                _adsSettings.deviceSettings.iosSettings[i].deviceHardware.threshold_BatteryLevel = jsonSettings.iosSettings[i].threshold_BatteryLevel;
            }
            _adsSettings.deviceSettings.allDevicesSettings = jsonSettings.allDevicesSettings;
            _adsSettings.deviceSettings.lowEndDeviceSettings = jsonSettings.lowEndDeviceSettings;
            _adsSettings.deviceSettings.limitadsettings = jsonSettings.limitadsettings;
            _adsSettings.deviceSettings.lowMemorySettings = jsonSettings.lowMemorySettings;

            CheckDevice();
        }
    }

    public void CheckDevice()
    {

#if UNITY_ANDROID
        _adsSettings.deviceSettings.platform = DeviceType.Android;
#elif UNITY_IOS
        _adsSettings.deviceSettings.platform = DeviceType.Ios;
#endif
        bool isDeviceSettingsApplied = false;
        switch (_adsSettings.deviceSettings.platform)
        {
            case DeviceType.Android: // Android Device Settings


                if (_adsSettings.deviceSettings.androidSettings.Length > 0)
                {

                    AndroidDevice[] androidDevices = _adsSettings.deviceSettings.androidSettings;

                    for (int i = 0; i < _adsSettings.deviceSettings.androidSettings.Length; i++)
                    {
                        string osName = GetAndroidOSName(androidDevices[i]);

                        if (!string.IsNullOrEmpty(osName) && SystemInfo.operatingSystem.Contains(osName))
                        {
                            // Device matched with settings
                            Debug.Log("========== " + (osName) + " Detected =============");

                           currentAndroidDevice = androidDevices[i];

                            if (SystemInfo.systemMemorySize <= currentAndroidDevice.deviceHardware.threshold_SystemMemory)
                            {
                                // Device has low Memory
                                Debug.Log("========== " + (osName) + " Has low Ram than threshold =============");
                                SetAdsControl(currentAndroidDevice.deviceHardware.onLowMemorySettings);
                                isDeviceSettingsApplied = true;
                            }
                            if (SystemInfo.graphicsMemorySize <= currentAndroidDevice.deviceHardware.threshold_GraphicsMemory)
                            {
                                // Device has low Graphics
                                Debug.Log("========== " + (osName) + " Has low Graphics than threshold =============");
                                SetAdsControl(currentAndroidDevice.deviceHardware.onlowGraphicsMemorySettings);
                                isDeviceSettingsApplied = true;
                            }
                            if (SystemInfo.batteryLevel <= (currentAndroidDevice.deviceHardware.threshold_BatteryLevel / 100))
                            {
                                // Device has low Battery
                                Debug.Log("========== " + (osName) + " Has low Battery than threshold =============");
                                SetAdsControl(currentAndroidDevice.deviceHardware.onLowBatterySettings);
                                isDeviceSettingsApplied = true;
                            }
                        }
                    }
                    if (isDeviceSettingsApplied == false)
                    {
                        for (int i = 0; i < androidDevices.Length; i++)
                        {
                            if (androidDevices[i].os == AndroidOS.Any)
                            {
                                currentAndroidDevice = androidDevices[i];
                                if (SystemInfo.systemMemorySize <= currentAndroidDevice.deviceHardware.threshold_SystemMemory)
                                {
                                    // Device has low Memory
                                    Debug.Log("========== " + (currentAndroidDevice.os.ToString()) + " Has low Ram than threshold =============");
                                    SetAdsControl(currentAndroidDevice.deviceHardware.onLowMemorySettings);
                                }
                                if (SystemInfo.graphicsMemorySize <= currentAndroidDevice.deviceHardware.threshold_GraphicsMemory)
                                {
                                    // Device has low Graphics Memory
                                    Debug.Log("========== " + (currentAndroidDevice.os) + " Has low Graphics than threshold =============");
                                    SetAdsControl(currentAndroidDevice.deviceHardware.onlowGraphicsMemorySettings);
                                }
                                if (SystemInfo.batteryLevel <= (currentAndroidDevice.deviceHardware.threshold_BatteryLevel / 100))
                                {
                                    // Device has low Battery
                                    Debug.Log("========== " + (currentAndroidDevice.os) + " Has low Battery than threshold =============");
                                    SetAdsControl(currentAndroidDevice.deviceHardware.onLowBatterySettings);
                                }
                            }
                        }
                    }
                    isDeviceSettingsApplied = false;
                }

                break;

            case DeviceType.Ios: // IOS Device Settings

                Debug.Log("========== " + (SystemInfo.operatingSystem) + " Detected =============");

                if (_adsSettings.deviceSettings.iosSettings.Length > 0)
                {
                    IOSDevice[] iOSDevices = _adsSettings.deviceSettings.iosSettings;

                    for (int i = 0; i < iOSDevices.Length; i++)
                    {
                        string osName = GetIOSOSName(iOSDevices[i]);
                        if (!string.IsNullOrEmpty(osName) && SystemInfo.operatingSystem.ToLower().Contains(osName.ToLower()) || iOSDevices[i].os == IOSOS.Any)
                        {
                            // Device matched with settings
                            Debug.Log("========== " + (osName) + " Detected =============");

                            currentIosDevice = iOSDevices[i];
                            Debug.Log("========== " + (osName) + " Detected 2 =============");

                            if (SystemInfo.systemMemorySize < currentIosDevice.deviceHardware.threshold_SystemMemory)
                            {
                                // Device has low Memory
                                Debug.Log("========== " + (osName) + " Has low Ram than threshold =============");
                                SetAdsControl(currentIosDevice.deviceHardware.onLowMemorySettings);
                                isDeviceSettingsApplied = true;

                            }
                            if (SystemInfo.graphicsMemorySize < currentIosDevice.deviceHardware.threshold_GraphicsMemory)
                            {
                                // Device has low Graphics Memory
                                Debug.Log("========== " + (osName) + " Has low Graphics than threshold =============");
                                SetAdsControl(currentIosDevice.deviceHardware.onlowGraphicsMemorySettings);
                                isDeviceSettingsApplied = true;

                            }
                            if (SystemInfo.batteryLevel <= (currentIosDevice.deviceHardware.threshold_BatteryLevel / 100))
                            {
                                // Device has low Battery
                                Debug.Log("========== " + (osName) + " Has low Battery than threshold =============");
                                SetAdsControl(currentIosDevice.deviceHardware.onLowBatterySettings);
                                isDeviceSettingsApplied = true;
                            }

                        }
                    }

                    if (isDeviceSettingsApplied == false)
                    {
                        for (int i = 0; i < iOSDevices.Length; i++)
                        {
                            if (iOSDevices[i].os == IOSOS.Any)
                            {
                                currentIosDevice = iOSDevices[i];
                                if (SystemInfo.systemMemorySize < currentIosDevice.deviceHardware.threshold_SystemMemory)
                                {
                                    // Device has low Memory
                                    Debug.Log("========== " + (currentIosDevice.os.ToString()) + " Has low Ram than threshold =============");
                                    // GVAnalysisManager.Instance.sendRemoteConfigDeviceAnalytics("LOW_RAM_OS_" + (currentAndroidDevice.os.ToString()) + "_RAM_" + SystemInfo.systemMemorySize);
                                    SetAdsControl(currentIosDevice.deviceHardware.onLowMemorySettings);
                                }
                                if (SystemInfo.graphicsMemorySize < currentIosDevice.deviceHardware.threshold_GraphicsMemory)
                                {
                                    // Device has low Graphics Memory
                                    Debug.Log("========== " + (currentIosDevice.os) + " Has low Graphics than threshold =============");
                                    SetAdsControl(currentIosDevice.deviceHardware.onlowGraphicsMemorySettings);
                                }
                                if (SystemInfo.batteryLevel <= (currentIosDevice.deviceHardware.threshold_BatteryLevel / 100))
                                {
                                    // Device has low Battery
                                    Debug.Log("========== " + (currentIosDevice.os) + " Has low Battery than threshold =============");
                                    SetAdsControl(currentIosDevice.deviceHardware.onLowBatterySettings);
                                }
                            }
                        }
                    }
                    isDeviceSettingsApplied = false;

                }


                break;
        }

        if(AdConstants.isLowEndDevice == false || AdConstants.limitAds == false)
            ApplyAllAdsSettings();

        ShowReadOnlyProperties();

    }

    

    private void ApplyAllAdsSettings()
    {
        Debug.Log("applying all settings");
        AdConstants.showBannerAd = _adsSettings.deviceSettings.allDevicesSettings.banner;
        AdConstants.showNativeBannerAd = _adsSettings.deviceSettings.allDevicesSettings.native;
        AdConstants.showStaticAd = _adsSettings.deviceSettings.allDevicesSettings.staticAd;
        AdConstants.showInterstitialAd = _adsSettings.deviceSettings.allDevicesSettings.interstitialAd;
        AdConstants.showRewardedAd = _adsSettings.deviceSettings.allDevicesSettings.RewardedAd;
        AdConstants.showRewardedInterstitialAd = _adsSettings.deviceSettings.allDevicesSettings.RewardedInterstitialAd;
        AdConstants.showApplovinAppOpen = _adsSettings.deviceSettings.allDevicesSettings.ApplovinAppOpenAd;
        AdConstants.adDelay = _adsSettings.deviceSettings.allDevicesSettings.AdDelay;
        Debug.Log("applied all settings");




    }
    private void ShowReadOnlyProperties()
    {
        showProperity = true;
        lowEndDeviceSettings = AdConstants.isLowEndDevice;
        LimitAdsDeviceSettings = AdConstants.limitAds;
        ShowBanner = AdConstants.showBannerAd;
        ShowNativeBanner = AdConstants.showNativeBannerAd;
        ShowStatic = AdConstants.showStaticAd;
        ShowInterstitial = AdConstants.showInterstitialAd;
        ShowRewarded = AdConstants.showRewardedAd;
        ShowRewardedInterstitial = AdConstants.showRewardedInterstitialAd;
        showAppOpenAd = AdConstants.showApplovinAppOpen;
        Debug.LogFormat("<================ Current Ads Settings ================>\n" +
                        "Is Low End: {0}\n" +
                        "Is Limit Ads: {1}\n" +
                        "Is Low Memory Ads: {2}\n" +
                        "Show Banner Ads: {3}\n" +
                        "show Native Banner Ad: {4}\n" +
                        "Show Static Ad: {5}\n" +
                        "Show Interstitial Ad: {6}\n" +
                        "Show Rewarded Ad: {7}\n" +
                        "Show Rewarded Interstitial Ad: {8}\n" +
                        "Show App Open Ad: {9}\n" +
                        "<================ End ================>"
                        , AdConstants.isLowEndDevice,
                         AdConstants.limitAds,
                         LowMemoryAdsSettings,
                         AdConstants.showBannerAd,
                         AdConstants.showNativeBannerAd,
                         AdConstants.showStaticAd,
                         AdConstants.showInterstitialAd,
                         AdConstants.showRewardedAd,
                         AdConstants.showRewardedInterstitialAd,
                         AdConstants.showApplovinAppOpen);
    }

    public void ApplyDeviceModelSettings(string json)
    {


        lowendDevices = JsonUtility.FromJson<LowEndDevices>(json);

        switch (_adsSettings.deviceSettings.platform)
        {
            case DeviceType.Android:

                Debug.Log("========== Current Device: " + SystemInfo.deviceModel + json + " =============");

                for (int j = 0; j < lowendDevices.Devices.Length; j++)
                {
                    if (SystemInfo.deviceModel.ToLower().Contains(lowendDevices.Devices[j].ToLower()))
                    {
                        Debug.Log("========== " + SystemInfo.deviceModel + " Falls in low End Devices =============");
                        DeviceHardwareSettings lowEndDeviceSettings = new DeviceHardwareSettings();
                        lowEndDeviceSettings.onLowMemorySettings = new DeviceLowMemorySettings();
                        lowEndDeviceSettings.onLowMemorySettings.lowEndDevice = true;
                        SetAdsControl(lowEndDeviceSettings.onLowMemorySettings);
                        break;
                    }
                }

                break;


            case DeviceType.Ios:

                Debug.Log("========== Current Device: " + SystemInfo.operatingSystem + json + " =============");

                for (int j = 0; j < lowendDevices.Devices.Length; j++)
                {
                    if (SystemInfo.operatingSystem.ToLower().Contains(lowendDevices.Devices[j].ToLower()))
                    {
                        Debug.Log("========== " + SystemInfo.deviceModel + " Falls in low End Devices =============");
                        DeviceHardwareSettings lowEndDeviceSettings = new DeviceHardwareSettings();
                        lowEndDeviceSettings.onLowMemorySettings = new DeviceLowMemorySettings();
                        lowEndDeviceSettings.onLowMemorySettings.lowEndDevice = true;
                        SetAdsControl(lowEndDeviceSettings.onLowMemorySettings);
                        break;
                    }
                }

                break;
        }

    }

    public void SetAdsControl(DeviceLowMemorySettings lowmemorySettings)
    {
        if (lowmemorySettings.lowEndDevice == true)
            AdConstants.isLowEndDevice = true;
        if (lowmemorySettings.limitAds == true)
            AdConstants.limitAds = true;

        if (AdConstants.isLowEndDevice)
        {
            Debug.Log("========== Applying Low End Settings =============");

            AdConstants.showBannerAd = _adsSettings.deviceSettings.lowEndDeviceSettings.banner;
            AdConstants.showAdaptiveBannerAd = _adsSettings.deviceSettings.lowEndDeviceSettings.adaptiveBanner;
            AdConstants.showNativeBannerAd = _adsSettings.deviceSettings.lowEndDeviceSettings.native;
            AdConstants.showStaticAd = _adsSettings.deviceSettings.lowEndDeviceSettings.staticAd;
            AdConstants.showInterstitialAd = _adsSettings.deviceSettings.lowEndDeviceSettings.interstitialAd;
            AdConstants.showRewardedAd = _adsSettings.deviceSettings.lowEndDeviceSettings.RewardedAd;
            AdConstants.showRewardedInterstitialAd = _adsSettings.deviceSettings.lowEndDeviceSettings.RewardedInterstitialAd;
            AdConstants.showApplovinAppOpen = _adsSettings.deviceSettings.lowEndDeviceSettings.ApplovinAppOpenAd;
            AdConstants.showAdmobAppOpen = _adsSettings.deviceSettings.limitadsettings.AdmobAppOpenAd;
            AdConstants.adDelay = _adsSettings.deviceSettings.lowEndDeviceSettings.AdDelay;
 
        }
        if (AdConstants.limitAds)
        {
            Debug.Log("========== Applying limit Ads Settings =============");

            AdConstants.showBannerAd = _adsSettings.deviceSettings.limitadsettings.banner;
            AdConstants.showAdaptiveBannerAd = _adsSettings.deviceSettings.limitadsettings.adaptiveBanner;
            AdConstants.showNativeBannerAd = _adsSettings.deviceSettings.limitadsettings.native;
            AdConstants.showStaticAd = _adsSettings.deviceSettings.limitadsettings.staticAd;
            AdConstants.showInterstitialAd = _adsSettings.deviceSettings.limitadsettings.interstitialAd;
            AdConstants.showRewardedAd = _adsSettings.deviceSettings.limitadsettings.RewardedAd;
            AdConstants.showRewardedInterstitialAd = _adsSettings.deviceSettings.limitadsettings.RewardedInterstitialAd;
            AdConstants.showApplovinAppOpen = _adsSettings.deviceSettings.limitadsettings.ApplovinAppOpenAd;
            AdConstants.showAdmobAppOpen = _adsSettings.deviceSettings.limitadsettings.AdmobAppOpenAd;
            AdConstants.adDelay = _adsSettings.deviceSettings.limitadsettings.AdDelay;


        }
    }

    public void ApplyLowMemorySettings()
    {
        if (_adsSettings.deviceSettings.lowMemorySettings != null)
        {
            Debug.Log("========== Applying Low Memory Ads Settings =============");
            LowMemoryAdsSettings = true;
            AdConstants.showBannerAd = _adsSettings.deviceSettings.lowMemorySettings.banner;
            AdConstants.showAdaptiveBannerAd = _adsSettings.deviceSettings.lowMemorySettings.adaptiveBanner;
            AdConstants.showNativeBannerAd = _adsSettings.deviceSettings.lowMemorySettings.native;
            AdConstants.showStaticAd = _adsSettings.deviceSettings.lowMemorySettings.staticAd;
            AdConstants.showInterstitialAd = _adsSettings.deviceSettings.lowMemorySettings.interstitialAd;
            AdConstants.showRewardedAd = _adsSettings.deviceSettings.lowMemorySettings.RewardedAd;
            AdConstants.showRewardedInterstitialAd = _adsSettings.deviceSettings.lowMemorySettings.RewardedInterstitialAd;
            AdConstants.showApplovinAppOpen =  _adsSettings.deviceSettings.lowMemorySettings.ApplovinAppOpenAd;
            AdConstants.showAdmobAppOpen = _adsSettings.deviceSettings.limitadsettings.AdmobAppOpenAd;
            AdConstants.adDelay = _adsSettings.deviceSettings.lowMemorySettings.AdDelay;
        }

    }

    private string GetAndroidOSName(AndroidDevice androidDevice)
    {
        string deviceName = "";

        switch (androidDevice.os)
        {

            case AndroidOS.Android_5_0:

                deviceName = "Android OS 5.0";
                break;

            case AndroidOS.Android_5_1:

                deviceName = "Android OS 5.1";
                break;

            case AndroidOS.Android_6_0:

                deviceName = "Android OS 6.0";
                break;

            case AndroidOS.Android_7_0:

                deviceName = "Android OS 7.0";
                break;

            case AndroidOS.Android_7_1:

                deviceName = "Android OS 7.1";
                break;


            case AndroidOS.Android_8_0:

                deviceName = "Android OS 8.0";
                break;


            case AndroidOS.Android_8_1:

                deviceName = "Android OS 8.1";
                break;


            case AndroidOS.Android_9_0:

                deviceName = "Android OS 9.0";
                break;

            case AndroidOS.Android_10:

                deviceName = "Android OS 10";
                break;

            case AndroidOS.Android_11:

                deviceName = "Android OS 11";
                break;

            case AndroidOS.Android_12:

                deviceName = "Android OS 12";
                break;

        }

        return deviceName;
    }

    private string GetIOSOSName(IOSDevice iosDevice)
    {
        string deviceName = "";
        switch (iosDevice.os)
        {

            case IOSOS.IOS_11:

                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 11";
                else
                    deviceName = "IOS 11";


                break;

            case IOSOS.IOS_12:

                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 12";
                else
                    deviceName = "IOS 12";
                break;
            case IOSOS.IOS_13:

                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 13";
                else
                    deviceName = "IOS 13";
                break;

            case IOSOS.IOS_14:

                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 14";
                else
                    deviceName = "IOS 14";
                break;

            case IOSOS.IOS_15:
                Debug.Log("Device Checking : " + SystemInfo.operatingSystem.ToLower() + " with " + ("iPadOS").ToLower());
                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 15";
                else
                    deviceName = "IOS 15";
                break;

            case IOSOS.IOS_16:

                if (SystemInfo.operatingSystem.ToLower().Contains(("iPadOS").ToLower()))
                    deviceName = "iPadOS 16";
                else
                    deviceName = "IOS 16";
                break;


        }

        return deviceName;
    }


}
