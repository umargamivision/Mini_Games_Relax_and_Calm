using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DeviceType
{
    Android, Ios
}

public enum AndroidOS
{
    Any,
    Android_5_0,
    Android_5_1,
    Android_6_0,
    Android_7_0,
    Android_7_1,
    Android_8_0,
    Android_8_1,
    Android_9_0,
    Android_10,
    Android_11,
    Android_12
}

public enum IOSOS
{
    Any,
    IOS_11,
    IOS_12,
    IOS_13,
    IOS_14,
    IOS_15,
    IOS_16
}

[System.Serializable]
public class DeviceSettings 
{
    [EnumToggleButtons]
    public DeviceType platform;
    [ShowIf("platform", DeviceType.Android)]
    [Title("Android Devices")] 
    public AndroidDevice[] androidSettings;
    [ShowIf("platform", DeviceType.Ios)]
    [Title("IOS Devices")]
    public IOSDevice[] iosSettings;
    [Title("Ads to Show For All Devices")]
    public AdsControl allDevicesSettings;
    [Title("Ads to Show For Low End Devices")]
    public AdsControl lowEndDeviceSettings;
    [Title("Ads to Show For Limit Ad Devices")]
    public AdsControl limitadsettings;
    [Title("Ads to Show For Low On Memory Devices")]
    public AdsControl lowMemorySettings;

}

[System.Serializable]
public class AndroidDevice
{
    public AndroidOS os;
    public DeviceHardwareSettings deviceHardware;
}

[System.Serializable]
public class IOSDevice
{
    public IOSOS os;
    public DeviceHardwareSettings deviceHardware;

}

[System.Serializable]
public class DeviceHardwareSettings
{
    public int threshold_SystemMemory = 2048;
    public DeviceLowMemorySettings onLowMemorySettings;
    public int threshold_GraphicsMemory = 512;
    public DeviceLowMemorySettings onlowGraphicsMemorySettings;
    [Range(0, 100)]
    public int threshold_BatteryLevel;
    public DeviceLowMemorySettings onLowBatterySettings;

}

[System.Serializable]
public class DeviceLowMemorySettings
{
    public bool lowEndDevice = false;
    public bool limitAds = false;
}

#region Json Claases

[System.Serializable]
public class DeviceSettingsJson
{
    [EnumToggleButtons]
    public string platform;
    public DeviceJson[] androidSettings;
    public DeviceJson[] iosSettings;
    public AdsControl allDevicesSettings;
    public AdsControl lowEndDeviceSettings;
    public AdsControl limitadsettings;
    public AdsControl lowMemorySettings;
}

[System.Serializable]
public class DeviceJson
{
    public string os;
    public DeviceHardwareSettings deviceHardware;
    [Range(0, 100)]
    public int threshold_BatteryLevel = 20;
}

[System.Serializable]
public class AdsControl
{
    public int AdDelay;
    public int firstAppOpenAdSession;
    public bool showAppOpenAfterEveryAd;
    public bool banner, adaptiveBanner, native, staticAd, interstitialAd, RewardedAd, RewardedInterstitialAd, ApplovinAppOpenAd, AdmobAppOpenAd;
}

[System.Serializable]
public class LowEndDevices
{
    public string[] Devices = new string[] { "Vivo 2007", "OPPO CPH1853" };

}

[System.Serializable]
public class LowEndDevicesRoot
{
    public List<string> Devices;
}
#endregion
