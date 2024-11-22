using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
//using GameAnalyticsSDK;

public class AnalyticsManager : GenericSingletonClass<AnalyticsManager> {


	public bool TimeSpentAnalytics = true;

	#region AdOnGamesEnd
	public void MoreGamesQuit()
	{
		//Analytics.CustomEvent ("AdOnGameEnd", new Dictionary<string, object> {
		//	{"Status", "Quited"} } 
		//);
	}
	public void MoreGamesCancel()
	{
		//Analytics.CustomEvent ("AdOnGameEnd", new Dictionary<string, object> {
		//	{"Status", "Cancel"} } 
		//);
	}
	public void MoreGamesMore()
	{
		//Analytics.CustomEvent ("AdOnGameEnd", new Dictionary<string, object> {
		//	{"Status", "MoreGamesList"} } 
		//);
	}

	#endregion



	#region PrivacyPolicy
	public void PrivacyPolicyConsentYes()
	{
		//Analytics.CustomEvent ("PrivacyPolicy", new Dictionary<string, object> {
		//	{"Consent", "Agree"} } 
		//);

	}
	public void PrivacyPolicyConsentNo()
	{
		//Analytics.CustomEvent ("PrivacyPolicy", new Dictionary<string, object> {
		//	{"Consent", "no"} } 
		//);

	}
	public void PrivacyPolicyDocumentReading()
	{
		//Analytics.CustomEvent ("PrivacyPolicy", new Dictionary<string, object> {
		//	{"Consent", "Policy"} } 
		//);

	}
	#endregion


	#region HouseAdsName
	// game name occurances
	public void HouseAdsNameEvent(string name)
	{
		//Analytics.CustomEvent ("HouseAds", new Dictionary<string, object> {
		//	{"GameName",name} } 
		//);
	}
	#endregion


	#region HouseAds_Click
	public void HouseAds_SpecificGameEvent_Click(string name,string input)
	{
		//Analytics.CustomEvent ("InHouseAd", new Dictionary<string, object> {
		//	{ 
		//		name, input

		//	} }
		//);


	}

	#endregion

	public void otherads(int adcompany)
	{
		//Analytics.CustomEvent ("OtherAds", new Dictionary<string, object> {
		//	{
		//		"Adtype",adcompany
		//	} } 
		//);


	}


    public void adsServed(string type)
	{

        //Analytics.CustomEvent("Ads-Analysis", new Dictionary<string, object> {
        //    {
        //        "Version | v_"+Application.version+" |",type
        //    } }
        //);
    }

    public void currentVersion()
    {
        //if (PlayerPrefs.GetString("Version") != Application.version)
        //{
        //    Analytics.CustomEvent("GameVersion", new Dictionary<string, object> { {
        //        "Version",Application.version
        //    } }
        //);
        //    PlayerPrefs.SetString("Version", Application.version);
        //}

    }

	public void GameStartStatus()
	{
		////        Debug.Log ("ads :"+loc);
		//Analytics.CustomEvent ("GameStart", new Dictionary<string, object> 
		//	{ { "State", "Started"} } );
	}

	public void InternetConnection(bool check)
	{
		////        Debug.Log ("ads :"+loc);
		//Analytics.CustomEvent ("isInternet", new Dictionary<string, object> 
		//	{ { "connected", check} } );
	}



    public void characterUnlock(int characterId)
	{
		//Analytics.CustomEvent ("characterUnlock", new Dictionary<string, object> {
		//	{
		//		"Status",characterId
		//	} } 
		//);
//		Debug.Log ("Status "+characterId);

	}


	public void characterDeath(int levelId, int characterId)
	{
		//Analytics.CustomEvent ("characterDeathRatio", new Dictionary<string, object> {
		//	{
		//		levelId+"",characterId+""
		//	} } 
		//);
	}



    void OnApplicationQuit()
    {
        //userTimeSpent();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //if (!pauseStatus)
        //{
        //    //	Debug.Log ("delay added to ads for "+AdConstants.timeToDelayAds);
        //    userTimeSpent();
        //}
    }

    public void miniEvents(string events,string status)
    {
	    //Analytics.CustomEvent(events, new Dictionary<string, object>
		   // { { "status", status } });
	    ////		Debug.Log ("rateus");
    }

    void userTimeSpent()
	{
		
//		int ts = (int)Time.realtimeSinceStartup + PlayerPrefs.GetInt("TimeSpend");
////		Debug.Log ("ts:"+ts);

//		string log = "0-100";
//		if (ts >= 0 && ts < 100) {
//			log = "0-100";
//		}
//		else if (ts >= 100 && ts < 200) 
//		{
//			log = "100-200";
//		}
//		else if (ts >= 200 && ts < 400) 
//		{
//			log = "200-400";
//		}
//		else if (ts >= 400 && ts < 800) 
//		{
//			log = "400-800";
//		}
//		else if (ts >= 800 && ts < 1600) 
//		{
//			log = "800-1600";
//		}
//		else if (ts >= 1600) 
//		{
//			log = "1600>";
//		}
#if UNITY_IOS
  //      Analytics.CustomEvent ("UserPlayTime", new Dictionary<string, object> {
		//	{
  //              "Time",log
		//	}
		//} 
		//);


#endif
  //      PlayerPrefs.SetInt ("TimeSpend", ts);


		//PlayerPrefs.Save ();
	}

	public enum rewardedstate
	{
		FREE_COINS,
		EXTRA_LIFE,
		POWER_INCREASE,
		HEALTH_INCREASE,
		UNLOCK_LEVEL,
		NO_VIDEO,
		UNLOCK_PLAYER,
		DOUBLE_COINS
	}

	public void rewarded_watch(rewardedstate gamestate,int playerID=-1,int levelno=-1)
	{
		//Analytics.CustomEvent ("rewarded_watch", new Dictionary<string, object> 
		//	{ { "state",gamestate } } );

	}

	#region givingLevelState //umair
	public enum levelState
	{
		LEVEL_STARTED,
		LEVEL_ENDED,
		HIGH_LEVEL_STARTED,
		HIGH_LEVEL_ENDED,
		OTHER_MODES //pass 0 for started, pass 1 for ended for other modes
	}

	public void LevelStatus(levelState levelstate,int levelNumber)
	{
	//	if (levelstate != levelState.OTHER_MODES) {

	//		if (levelstate == levelState.LEVEL_STARTED || levelstate == levelState.HIGH_LEVEL_STARTED) {
	//			if (PlayerPrefs.GetInt ("StageStarted_" + levelNumber, 0) == 0) {
	//				Debug.Log ("Stage Started: " + levelNumber);
	//				Analytics.CustomEvent ("Levels", new Dictionary<string, object> 
	//		{ { levelstate.ToString (),levelNumber } });

	//				PlayerPrefs.SetInt ("StageStarted_" + levelNumber, 1);
	//			}
	//		}
	//		if (levelstate == levelState.LEVEL_ENDED || levelstate == levelState.HIGH_LEVEL_ENDED) {
	//			if (PlayerPrefs.GetInt ("StageEnded_" + levelNumber, 0) == 0) {
	//				Debug.Log ("Stage Ended: " + levelNumber);
	//				Analytics.CustomEvent ("Levels", new Dictionary<string, object> 
	//			{ { levelstate.ToString (),levelNumber } });

	//				PlayerPrefs.SetInt ("StageEnded_" + levelNumber, 1);
	//			}
	//		}
	//	} else {
	

	//			if (levelNumber == 0) {
	//				Analytics.CustomEvent ("Levels", new Dictionary<string, object> 
	//					{ { levelstate.ToString (),"Started" } });
	//			} else {
	//				Analytics.CustomEvent ("Levels", new Dictionary<string, object> 
	//					{ { levelstate.ToString (),"Ended" } });
	//			}


	////		Analytics.CustomEvent ("Levels", new Dictionary<string, object> 
	////			{ { levelstate.ToString (),levelNumber } });
		
	//	}



	}
	// You can call this line with different param you want. //umair
	//AnalyticsManager.Instance.LevelStatus (AnalyticsManager.levelState.LEVEL_STARTED, 1);
	#endregion

	public void rateus()
	{
//		Analytics.CustomEvent ("RATEUS");
////		Debug.Log ("rateus");
	}

	public void moregames()
	{
//		Analytics.CustomEvent ("moregames");

////		Debug.Log ("moregames");

	}

	public void AdsLoadStatus(string loc)
	{
////		Debug.Log ("ads :"+loc);
//		Analytics.CustomEvent ("AdsLoadStatus", new Dictionary<string, object> 
//			{ {"state_"+Application.version,loc } } );

//		GameStartStatus ();

	}



	public void InHouseLoadStatus(string loc)
	{
//		Debug.Log ("house :"+loc);

		//Analytics.CustomEvent ("InHouseLoadStatus", new Dictionary<string, object> 
		//	{ { "state",loc } } );
	}


	public void languageSelect(string lang)
	{

		//Analytics.CustomEvent ("Language", new Dictionary<string, object> 
		//	{ { "name",lang } } );
	}



	#region inapp
	public void inAppPurchaseAnalytics(string itemName,bool status)
	{

		//Analytics.CustomEvent ("InAppPurchase", new Dictionary<string, object> 
		//	{ { itemName, status } } );
	}
	#endregion

	public void AutoFire(string itemName,bool status)
	{

		//Analytics.CustomEvent ("AutoFire", new Dictionary<string, object> 
		//	{ { itemName, status } } );
	}
	public void MatchesWon()
	{

		//Analytics.CustomEvent ("MatchesWon", new Dictionary<string, object>
		//	{ { "WinCount","win"} } );
	}

	public void MatchesLoss()
	{

		//Analytics.CustomEvent ("MatchesLoss", new Dictionary<string, object>
		//	{ { "LossCount","loss" } } );
	}


	public void NeverAskAgainForRemoveAds(){
		//xia
		//Analytics.CustomEvent ("IAP-Panel", new Dictionary<string, object> { {
		//		"NeverButton ", "Clicked"
		//	}
		//}
		//);
		//        AnalyticsManager.Instance.NeverAskAgainFor ();
	}


    public void crossPromotionAnalysis(string status)
    {
        //Analytics.CustomEvent("CrossPromotion", new Dictionary<string, object> {
        //    {"Status", status} }
        //);

    }



}
