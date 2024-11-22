using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GVMenuItems
{
	#region Variables

	public static int go_count = 0, components_count = 0, missing_count = 0;

	#endregion

	#region Features

	#region Ads

	[MenuItem("GV/Add All Define Symbols")]
	public static void AddDefineSymbolsAll()
	{
		AddDefineSymbols.Add("USE_MAX");
		AddDefineSymbols.Add("USE_MAX_OPENADS");
		AddDefineSymbols.Add("USE_FIREBASE");
		AddDefineSymbols.Add("USE_REMOTE_CONFIG");
		AddDefineSymbols.Add("USE_CRASHLYTICS");
		AddDefineSymbols.Add("USE_APPSFLYER");
		AddDefineSymbols.Add("tenjin_applovin_enabled");
	}

	[MenuItem("GV/Features/Applovin MAX/Enable Applovin MAX")]
	private static void AddMax()
	{
		AddDefineSymbols.Add("USE_MAX");

	}

	[MenuItem("GV/Features/Applovin MAX/Disable Applovin MAX")]
	private static void RemoveMax()
	{
		AddDefineSymbols.Clear("USE_MAX");

	}

	[MenuItem("GV/Features/Applovin MAX/Enable Max Open Ads")]
	private static void AddOpenAds()
	{
		AddDefineSymbols.Add("USE_MAX_OPENADS");

	}

	[MenuItem("GV/Features/Applovin MAX/Disable Max Open Ads")]
	private static void DisableOpenAds()
	{
		AddDefineSymbols.Clear("USE_MAX_OPENADS");

	}

	[MenuItem("GV/Features/Applovin MAX/Enable Idle MREC")]
	private static void AddIdleMREC()
	{
		AddDefineSymbols.Add("USE_IDLE_MREC");

	}

	[MenuItem("GV/Features/Applovin MAX/Disable Idle MREC")]
	private static void DisableIdleMREC()
	{
		AddDefineSymbols.Clear("USE_IDLE_MREC");

	}
	[MenuItem("GV/Features/Applovin MAX/Enable Low Intersitital")]
	private static void AddLowReward()
	{
		AddDefineSymbols.Add("USE_MAX_LOW_INTERSITITAL");

	}

	[MenuItem("GV/Features/Applovin MAX/Disable Low Intersitital")]
	private static void DisableLowReward()
	{
		AddDefineSymbols.Clear("USE_MAX_LOW_INTERSITITAL");

	}


	[MenuItem("GV/Features/Admob/Enable Admob Paid Event")]
	private static void AddAdmobPaidAdEvent()
	{
		AddDefineSymbols.Add("USE_ADMOB_PAID_EVENT");

	}

	[MenuItem("GV/Features/Admob/Disable Admob Paid Event")]
	private static void RemoveAdmobPaidAdEvent()
	{
		AddDefineSymbols.Clear("USE_ADMOB_PAID_EVENT");

	}

	[MenuItem("GV/Features/Admob/Enable Admob Open Ads 7.2.0")]
	private static void AddAdmobOpenAd720()
	{
		AddDefineSymbols.Add("USE_ADMOB_OPEN_AD_7_2_0");

	}

	[MenuItem("GV/Features/Admob/Disable Admob Open Ads 7.2.0")]
	private static void RemoveAdmobOpenAd720()
	{
		AddDefineSymbols.Clear("USE_ADMOB_OPEN_AD_7_2_0");

	}

	[MenuItem("GV/Features/Admob/Enable Admob Open Ads 7.8")]
	private static void AddAdmobOpenAd780()
	{
		AddDefineSymbols.Add("USE_ADMOB_OPEN_AD_8_5");

	}

	[MenuItem("GV/Features/Admob/Disable Admob Open Ads 7.8")]
	private static void RemoveAdmobOpenAd780()
	{
		AddDefineSymbols.Clear("USE_ADMOB_OPEN_AD_8_5");

	}


	[MenuItem("GV/Features/Admob/Enable Rewarded Intersitial")]
	private static void AddAdmobRewardedIntersitial()
	{
		AddDefineSymbols.Add("USE_ADMOB_REWARDED_INTERSITIAL");

	}

	[MenuItem("GV/Features/Admob/Disable Rewarded Intersitial")]
	private static void RemoveAdmobewardedIntersitial()
	{
		AddDefineSymbols.Clear("USE_ADMOB_REWARDED_INTERSITIAL");

	}

	// Open App Ads


	#endregion

	#region Facebook

	[MenuItem("GV/Features/Facebook/Enable Facebook Adapter Consent")]
	private static void AddFacebookConsent()
	{
		AddDefineSymbols.Add("USE_FB_ADAPTER");

	}

	[MenuItem("GV/Features/Facebook/Disable Facebook Adapter Consent")]
	private static void disableFacebookConsent()
	{
		AddDefineSymbols.Clear("USE_FB_ADAPTER");

	}


	#endregion

	#region Firebase Region

	[MenuItem("GV/Features/Firebase/Analytics/Enable Analytics")]
	private static void AddFirebase()
	{
		AddDefineSymbols.Add("USE_FIREBASE");

	}

	[MenuItem("GV/Features/Firebase/Analytics/Disable Analytics")]
	private static void RemoveFirebase()
	{
		AddDefineSymbols.Clear("USE_FIREBASE");

	}


	[MenuItem("GV/Features/Firebase/Remote Config/Enable Remote Config")]
	private static void AddRemoteConfig()
	{
		AddDefineSymbols.Add("USE_REMOTE_CONFIG");

	}

	[MenuItem("GV/Features/Firebase/Remote Config/Disable Remote Config")]
	private static void RemoveRemoteConfig()
	{
		AddDefineSymbols.Clear("USE_REMOTE_CONFIG");

	}


	[MenuItem("GV/Features/Firebase/Crashlytics/Enable Crashlytics")]
	private static void AddCrashlytics()
	{
		AddDefineSymbols.Add("USE_CRASHLYTICS");

	}

	[MenuItem("GV/Features/Firebase/Crashlytics/Disable Crashlytics")]
	private static void RemoveCrashlytics()
	{
		AddDefineSymbols.Clear("USE_CRASHLYTICS");

	}



	#endregion

	#region Android

	[MenuItem("GV/Features/Android/Native Rate Us/Enable")]
	private static void ShowNativeRate()
	{
		AddDefineSymbols.Add("SHOW_NATIVE_RATE");

	}

	[MenuItem("GV/Features/Android/Native Rate Us/Disable")]
	private static void disableNativeRate()
	{
		AddDefineSymbols.Clear("SHOW_NATIVE_RATE");

	}

	#endregion

	#region Delegates
	[MenuItem("GV/Features/Ads Delegates/Enable Delegates")]
	private static void EnableDelegates()
	{
		AddDefineSymbols.Add("USE_DELEGATES");

	}

	[MenuItem("GV/Features/Ads Delegates/Disable Delegates")]
	private static void DisableDelegates()
	{
		AddDefineSymbols.Clear("USE_DELEGATES");

	}

	#endregion

	#region Patch
	[MenuItem("GV/Features/Patch/Enable Patch")]
	private static void AddPatch()
	{
		AddDefineSymbols.Add("USE_PATCH");

	}

	[MenuItem("GV/Features/Patch/Disable Patch")]
	private static void RemovePatch()
	{
		AddDefineSymbols.Clear("USE_PATCH");

	}

	#endregion

	#endregion

	#region Utils

	[MenuItem("GV/Utils/Clear PlayerPrefs")]
	private static void ClearPrefs()
	{
		PlayerPrefs.DeleteAll();

		EditorUtility.DisplayDialog("Data Cleared", "PlayerPrefs are cleared now", "OK");
	}

	[MenuItem("GV/Utils/Multiply W&H by 2 &#2")]
	public static void Multiply_W_H_15()
	{
		MultiplySelected(2f);
	}
	public static void DivideSelected(float by)
	{
		GameObject[] go = Selection.gameObjects;

		for (int i = 0; i < go.Length; i++)
		{
			if (go[i].transform.GetComponent<RectTransform>() == null)
			{
				Debug.Log("This is not a RectTransform", go[i].gameObject);
			}
			else
			{

				RectTransform rect = go[i].GetComponent<RectTransform>();
				UnityEditor.Undo.RecordObject(rect, "Resizing Object");
				rect.sizeDelta = new Vector2(rect.sizeDelta.x / by, rect.sizeDelta.y / by);
			}
		}
	}
	public static void MultiplySelected(float by)
	{
		GameObject[] go = Selection.gameObjects;

		for (int i = 0; i < go.Length; i++)
		{
			if (go[i].transform.GetComponent<RectTransform>() == null)
			{
				Debug.Log("This is not a RectTransform", go[i].gameObject);
			}
			else
			{

				RectTransform rect = go[i].GetComponent<RectTransform>();
				UnityEditor.Undo.RecordObject(rect, "Resizing Object");
				rect.sizeDelta = new Vector2(rect.sizeDelta.x * by, rect.sizeDelta.y * by);
			}
		}

	}
	[MenuItem("GV/Utils/Find Missing Scripts")]
	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		go_count = 0;
		components_count = 0;
		missing_count = 0;
		foreach (GameObject g in go)
		{
			FindInGO(g);
		}
		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}
	private static void FindInGO(GameObject g)
	{
		go_count++;
		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			components_count++;
			if (components[i] == null)
			{
				missing_count++;
				string s = g.name;
				Transform t = g.transform;
				while (t.parent != null)
				{
					s = t.parent.name + "/" + s;
					t = t.parent;
				}
				Debug.Log(s + " has an empty script attached in position: " + i, g);
			}
		}

		foreach (Transform childT in g.transform)
		{
			FindInGO(childT.gameObject);
		}
	}
	[MenuItem("GV/Utils/Divide Width&Height by 2 &2")]
	public static void Divide_W_H_2()
	{
		DivideSelected(2);
	}

	[MenuItem("GV/Open 1st Scene &1")]
	public static void openPlugin()
	{
		int i = 0;
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

		EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
	}


	#endregion

	[MenuItem("GV/Version")]
	public static void showDIalog()
	{
		EditorUtility.DisplayDialog("GV Ads Plugin | APPLOVIN MAX | V1.3", "", "OK");
	}

}
