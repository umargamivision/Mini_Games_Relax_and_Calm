using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
public class AdsEditorWindow : EditorWindow
{
    public class AdKeyData
    {
        public string androidKey="", iOSKey="";
        string propertyName, functionName;
        Func<bool> showCondition;
        public AdKeyData(string propertyName, string functionName, Func<bool> showCondition)
        {
            this.propertyName = propertyName;
            this.functionName = functionName;
            this.showCondition = showCondition;
        }
        public void Display()
        {
            if (showCondition())
            {
                if (IsAndroid)
                    androidKey = ShowTextFieldWithRemoveSpace(propertyName, androidKey);
                else
                    iOSKey = ShowTextFieldWithRemoveSpace(propertyName, iOSKey);
            }
        }
        string ShowTextFieldWithRemoveSpace(string fieldName, string text)
        {
            GUIStyle style = new(EditorStyles.textArea)
            {
                wordWrap = true
            };
            EditorGUILayout.BeginHorizontal();
            string fieldText = text;
            GUILayout.Label(fieldName, GUILayout.Width(250));
            fieldText = EditorGUILayout.TextArea(fieldText, style);
            if (!string.IsNullOrEmpty(fieldText) && fieldText.Contains(" "))
            {
                if (GUILayout.Button("FIX", GUILayout.Width(30)))
                {
                    fieldText = fieldText.Replace(" ", "");
                }
            }
            EditorGUILayout.EndHorizontal();
            return fieldText;
        }
        public void GetValuesFromFile(string filePath)
        {
            androidKey = GetKey(functionName, true, filePath);
            iOSKey = GetKey(functionName, false, filePath);
        }
        public bool GetIsChanged(string filePath)
        {
            return androidKey != GetKey(functionName, true, filePath) || iOSKey != GetKey(functionName, false, filePath);
        }
        bool IsAndroid
        {
            get
            {
#if UNITY_ANDROID
                return true;
#elif UNITY_IOS
                return false;
#endif
            }
        }
        public enum Platform
        {
            Android,
            iOS,
            Other
        }
        private const string Pattern = @"public static string {0}\(\)\s*{{\s*#if UNITY_ANDROID\s*return ""(.*?)"";\s*#elif UNITY_IOS\s*return ""(.*?)"";\s*#else\s*return """";\s*#endif\s*}}";
        public string GetKey(string functionName, bool isAndroid, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return "";
            }
            string fileContent = File.ReadAllText(filePath);
            string formattedPattern = string.Format(Pattern, functionName);
            Match match = Regex.Match(fileContent, formattedPattern, RegexOptions.Singleline);
            if (match.Success)
            {
                Platform platform = isAndroid ? Platform.Android : Platform.iOS;
                int groupIndex = (platform == Platform.Android) ? 1 : (platform == Platform.iOS) ? 2 : -1;
                if (groupIndex != -1)
                {
                    return match.Groups[groupIndex].Value;
                }
            }
            return "";
        }
    }

    static bool IsMax()
    {
#if USE_MAX
        return true;
#else
        return false;
#endif
    }
    static bool IsIdleMREC_MAX()
    {
#if USE_IDLE_MREC
        return true;
#else
        return false;
#endif
    }
    static bool IsMAX_OpenAds()
    {
#if USE_MAX_OPENADS
        return true;
#else
        return false;
#endif
    }
    static bool IsAdMob_OpenAds()
    {
#if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
        return true;
#else
        return false;
#endif
    }
    static bool IsAdMob_RewardedInterstitial()
    {
#if USE_ADMOB_REWARDED_INTERSITIAL
        return true;
#else
        return false;
#endif
    }
    static bool IsMAX_LowInterstitial()
    {
#if USE_MAX_LOW_INTERSITITAL
        return true;
#else
        return false;
#endif
    }
    public enum IDType
    {
        SDKKey,
        InterstitialAdUnitId,
        RewardedAdUnitId,
        BannerAdUnitId,
        MRECBannerAdUnitId,
        IdleMRECBannerAdUnitId,
        AppOpenAdUnitId_MAX,
        AppOpenAdTier1,
        AppOpenAdTier2,
        AppOpenAdTier3,
        AdMobRewardedInterstitialUnitId,
        LowMaxInterstitialAdUnitId
    }
    Dictionary<IDType, AdKeyData> adsKeyData = new(){
        {IDType.SDKKey,new("SDK Key","SDKKey",IsMax)},
        {IDType.InterstitialAdUnitId,new("Interstitial Ad Id","InterstitialAdUnitId",IsMax)},
        {IDType.RewardedAdUnitId,new("Rewarded Ad Id","RewardedAdUnitId",IsMax)},
        {IDType.BannerAdUnitId,new("Banner Ad Id","BannerAdUnitId",IsMax)},
        {IDType.MRECBannerAdUnitId,new("MREC Banner Ad Id","MRECBannerAdUnitId",IsMax)},
        {IDType.IdleMRECBannerAdUnitId,new("Idle MREC Banner Ad Id","IdleMRECBannerAdUnitId",()=>IsMax()&&IsIdleMREC_MAX())},
        { IDType.LowMaxInterstitialAdUnitId, new("Low Max Interstitial Id", "LowMaxIntersititialAdUnitId", IsMAX_LowInterstitial) },
        { IDType.AppOpenAdUnitId_MAX,new("App OpenAd Id MAX","AppOpenAdUnitId_MAX",()=>IsMax()&&IsMAX_OpenAds())},
        {IDType.AppOpenAdTier1,new("App OpenAd Tier 1","AppOpenAdTier1",IsAdMob_OpenAds)},
        {IDType.AppOpenAdTier2,new("App OpenAd Tier 2","AppOpenAdTier2",IsAdMob_OpenAds)},
        {IDType.AppOpenAdTier3,new("App OpenAd Tier 3","AppOpenAdTier3",IsAdMob_OpenAds)},
        {IDType.AdMobRewardedInterstitialUnitId,new("AdMob Rewarded Interstitial Id","AdmobRewardedIntersitialUnitId",IsAdMob_RewardedInterstitial)},
      
    };
    static string filePathAfterAsset = "_AdsData/Scripts/_Ads/AdsIds.cs";
    [MenuItem("GV/AdsWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow<AdsEditorWindow>();
        window.titleContent = new GUIContent("AdsWindow");
        window.Show();
    }
    private void OnEnable()
    {
        UpdateInitialKeys();
    }
    private void OnLostFocus()
    {
        string filePath = Path.Combine(Application.dataPath, filePathAfterAsset);
        foreach (var item in adsKeyData)
        {
            if (item.Value.GetIsChanged(filePath))
            {
                UpdateScript(filePath);
                break;
            }
        }
    }
    private Vector2 scrollPos;

    void ShowFeatures()
    {
        GUILayout.Label("Symbols : ", EditorStyles.boldLabel);
        AddRemoveSymbol("MAX SDK", "USE_MAX");
        AddRemoveSymbol("MAX Open Ads", "USE_MAX_OPENADS");
        AddRemoveSymbol("Max Low Intersitital", "USE_MAX_LOW_INTERSITITAL");
        AddRemoveSymbol("IDLE MREC", "USE_IDLE_MREC");
        AddRemoveSymbol("ADMOB PAID EVENT", "USE_ADMOB_PAID_EVENT");
        AddRemoveSymbol("ADMOB OPEN AD 7.2.0", "USE_ADMOB_OPEN_AD_7_2_0");
        AddRemoveSymbol("ADMOB OPEN AD 7.8", "USE_ADMOB_OPEN_AD_8_5");
        AddRemoveSymbol("ADMOB REWARDED INTERSTITIAL", "USE_ADMOB_REWARDED_INTERSITIAL");
        AddRemoveSymbol("FB ADAPTER", "USE_FB_ADAPTER");
        AddRemoveSymbol("FIREBASE", "USE_FIREBASE");
        AddRemoveSymbol("REMOTE CONFIG", "USE_REMOTE_CONFIG");
        AddRemoveSymbol("CRASHLYTICS", "USE_CRASHLYTICS");
        AddRemoveSymbol("NATIVE RATE", "SHOW_NATIVE_RATE");
        AddRemoveSymbol("DELEGATES", "USE_DELEGATES");
        AddRemoveSymbol("PATCH", "USE_PATCH");
        AddRemoveSymbol("APPFLYER", "USE_APPSFLYER");

    }
    private void AddRemoveSymbol(string symbolName, string symbol)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(symbolName + " :", GUILayout.Width(250));
        if (!AddDefineSymbols.CheckSymbol(symbol))
        {
            if (GUILayout.Button("Add"))
            {
                AddDefineSymbols.Add(symbol);
            }
        }
        else
        {
            if (GUILayout.Button("Remove"))
            {
                AddDefineSymbols.Clear(symbol);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        AdsKeysEditor();
        GUILayout.Space(10);
        ShowFeatures();
        GUILayout.Space(10);
        GUILayout.EndScrollView();
    }
    void UpdateInitialKeys()
    {
        string filePath = Path.Combine(Application.dataPath, filePathAfterAsset);
        foreach (var item in adsKeyData)
        {
            item.Value.GetValuesFromFile(filePath);
        }
    }
    private void AdsKeysEditor()
    {
        GUILayout.Label("Ads keys : ", EditorStyles.boldLabel);
        string filePath = Path.Combine(Application.dataPath, filePathAfterAsset); ;
        /*if (GUILayout.Button("Update Initial Keys"))
        {
            UpdateInitialKeys();
        }*/
        foreach (var item in adsKeyData)
        {
            item.Value.Display();
        }
        /*if (GUILayout.Button("OverrideKeys"))
        {
            UpdateScript(filePath);
        }*/
    }
    void UpdateScript(string fileName)
    {
        string newString = $@"
using Sirenix.OdinInspector;
using UnityEngine;
public static class AdsIds
{{
    #if USE_MAX
    public static string SDKKey()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.SDKKey].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.SDKKey].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string InterstitialAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.InterstitialAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.InterstitialAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string RewardedAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.RewardedAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.RewardedAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string BannerAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.BannerAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.BannerAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string MRECBannerAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.MRECBannerAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.MRECBannerAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    #if USE_IDLE_MREC
    public static string IdleMRECBannerAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.IdleMRECBannerAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.IdleMRECBannerAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    #endif
    #if USE_MAX && USE_MAX_OPENADS
    public static string AppOpenAdUnitId_MAX()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.AppOpenAdUnitId_MAX].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.AppOpenAdUnitId_MAX].iOSKey}"";
        #else
            return """";
        #endif
    }}
    #endif
    #endif
    #if USE_ADMOB_OPEN_AD_7_2_0 || USE_ADMOB_OPEN_AD_8_5
    public static string AppOpenAdTier1()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.AppOpenAdTier1].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.AppOpenAdTier1].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string AppOpenAdTier2()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.AppOpenAdTier2].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.AppOpenAdTier2].iOSKey}"";
        #else
            return """";
        #endif
    }}
    public static string AppOpenAdTier3()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.AppOpenAdTier3].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.AppOpenAdTier3].iOSKey}"";
        #else
            return """";
        #endif
    }}
    #endif
    #if USE_ADMOB_REWARDED_INTERSITIAL
    public static string AdmobRewardedIntersitialUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.AdMobRewardedInterstitialUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.AdMobRewardedInterstitialUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
    #endif

    
    #if USE_MAX_LOW_INTERSITITAL
    public static string LowMaxIntersititialAdUnitId()
    {{
        #if UNITY_ANDROID
            return ""{adsKeyData[IDType.LowMaxInterstitialAdUnitId].androidKey}"";
        #elif UNITY_IOS
            return ""{adsKeyData[IDType.LowMaxInterstitialAdUnitId].iOSKey}"";
        #else
            return """";
        #endif
    }}
#endif

}}";
        try
        {
            File.WriteAllText(fileName, newString);
            AssetDatabase.Refresh();
            Debug.Log($"BILLI : File Write Success");
        }
        catch (Exception error)
        {
            Debug.LogError("Error creating file: " + error.Message);
        }
    }

}
