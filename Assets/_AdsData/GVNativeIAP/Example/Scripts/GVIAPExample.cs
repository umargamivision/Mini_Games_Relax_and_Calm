using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GVNativeIAP;

public class GVIAPExample : MonoBehaviour {

	public Text responseText,coinsText;



	int first=5;
	int second=10;

	// Use this for initialization
	void Start () {
        GVIAPListener.OnProductPriceSuccess += GVIAPListener_OnProductPriceSuccess;
		coinsText.text=PlayerPrefs.GetInt("dummycoins", 0).ToString();
	}

    private void GVIAPListener_OnProductPriceSuccess(string productID, string productPrice)
    {
		Debug.Log(/*this,*/"GVIAPExample: " + "GVIAPListener_OnProductPriceSuccess: " + productID);
		Debug.Log(/*this,*/"GVIAPExample: " + "GVIAPListener_OnProductPriceSuccess: " + productPrice);
    }

    public void callThisEventResponse(IAPRestoreIDs obj){
		responseText.text = obj.ToString ();
	}

	public void itemPurchaseSuccess(int id){

		switch(id)
        {
			case 0:
				responseText.text = "Item Purchase Successfully";
				//PlayerPrefs.SetInt(GVAdsManager.noAdsString, 1);
			break;

			case 1:
				responseText.text = "120 coins purchased";
				PlayerPrefs.SetInt("dummycoins", PlayerPrefs.GetInt("dummycoins") + 120);
				coinsText.text = PlayerPrefs.GetInt("dummycoins", 0).ToString();
				break;
			case 2:
				responseText.text = "30 coins purchased";
				PlayerPrefs.SetInt("dummycoins", PlayerPrefs.GetInt("dummycoins") + 30);
				coinsText.text = PlayerPrefs.GetInt("dummycoins", 0).ToString();
				break;
			case 3:
				responseText.text = "100 coins purchased";
				PlayerPrefs.SetInt("dummycoins", PlayerPrefs.GetInt("dummycoins") + 100);
				coinsText.text = PlayerPrefs.GetInt("dummycoins", 0).ToString();
				break;

		}

	}

	public void itemPurchaseFail(){
		responseText.text = "Fail To Purchase Item";
	}

    public void getItemPrice(string productID)
    {
        GVIAPAndroid.getInstance().getProductPrice(productID);
    }
}
