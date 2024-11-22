using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GVNativeIAP{
	public class GVIAPAndroid {

		private static GVIAPAndroid instance;
        public GVIAPListener _gVIAPListener;
		AndroidJavaObject gviap;

		private GVIAPAndroid () {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            gviap = new AndroidJavaObject("farhan.gv.com.iap.GVIAP");
            gviap.Call("setContextAndActivity", context, activity);
            gviap.Call("initializeIAPSdk");
#endif
#endif
        }

        // static method to create instance of Singleton class 
        public static GVIAPAndroid getInstance() 
		{ 
			if (instance == null) 
				instance = new GVIAPAndroid(); 

			return instance; 
		}

		public void restorePurchases (){
            if (Application.platform == RuntimePlatform.Android)
            {
                gviap.Call("restorePurchase");
            }
            
        }

        
            public void purchaseThisProduct (string productId){
                gviap.Call("purchaseProduct", productId);    
        }
        public void getProductPrice(string productId)
        {
            

#if !UNITY_EDITOR
#if UNITY_ANDROID
                        gviap.Call("getProductPrice", productId);
#endif
#endif

        }
    }
}