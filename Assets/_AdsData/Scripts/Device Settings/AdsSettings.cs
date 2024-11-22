using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
public class AdsSettings : MonoBehaviour
{
    [Title("Setup Devices Specifications")] // Ads Control on Devices
    public DeviceSettings deviceSettings;
    [Title("Setup Device that are forcefully set to low End regardless of their Specification")] // Devices to set low end
    public LowEndDevicesRoot lowEndDevices;

    #region Others
#if UNITY_EDITOR
    // For Developer use Only
    ////Start is called before the first frame update
    [Button("Create Device Json")]
    void CreateDeviceJson()
    {
        string deviceSettingJson = SaveJson(deviceSettings);
        Debug.Log(deviceSettingJson);
        UnityEditor.AssetDatabase.Refresh();
        if (File.Exists(Application.dataPath + "/Resources/Jsons/DeviceSettings.json"))
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/DeviceSettings.json", deviceSettingJson);
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/Jsons/");
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/DeviceSettings.json", deviceSettingJson);
        }
        UnityEditor.AssetDatabase.Refresh();
    }

    [Button("Create Low End Device Json")]
    void CreateLowEndDeviceJson()
    {
        string deviceSettingJson = JsonUtility.ToJson(lowEndDevices);
        Debug.Log(deviceSettingJson);
        UnityEditor.AssetDatabase.Refresh();
        if (File.Exists(Application.dataPath + "/Resources/Jsons/LowEndDevices.json"))
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/LowEndDevices.json", deviceSettingJson);
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/Jsons/");
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/LowEndDevices.json", deviceSettingJson);
        }
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
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
        deviceSettings.allDevicesSettings = settings.allDevicesSettings;
        deviceSettings.lowEndDeviceSettings = settings.lowEndDeviceSettings;
        deviceSettings.limitadsettings = settings.limitadsettings;
        deviceSettings.lowMemorySettings = settings.lowMemorySettings;
        string jsonString = JsonUtility.ToJson(deviceSettings);
        Debug.Log("Json String: " + jsonString + "\n ============= Json Ends here ==============");
        return jsonString;
    }


    #endregion



}
