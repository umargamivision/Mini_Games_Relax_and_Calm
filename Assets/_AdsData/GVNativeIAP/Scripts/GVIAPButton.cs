using System;
using System.Collections;
using System.Collections.Generic;
#if USE_APPSFLYER
using AppsFlyerSDK;
#endif
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace GVNativeIAP
{
	public enum ProductType
	{
		CONSUMEABLE = 0,
		NONCONSUMABLE = 1,
		SUBSCRIPTION = 2
	};
	[System.Serializable]
	public class GVIAPCustomEventButton : UnityEvent { }

	public class GVIAPButton : MonoBehaviour
	{

		public string productId = "Product ID";
		public ProductType _productType = ProductType.CONSUMEABLE;
		public string _productPrice = "Loading...";
		public bool isPriceLoaded = false;
		public Text _priceTxt;

		[Space(20)]
		public GVIAPCustomEventButton onPurchaseSuccess;
		public GVIAPCustomEventButton onPurchaseFail;

		// Use this for initialization
		void Start()
		{

			GVIAPListener.OnProductPriceSuccess += GVIAPListener_OnProductPriceSuccess;
			if (_priceTxt)
				_priceTxt.text = "Loading...";
			StartCoroutine("LoadItemPriceCR");
		}

		private void OnDestroy()
		{
			GVIAPListener.OnProductPriceSuccess -= GVIAPListener_OnProductPriceSuccess;
		}

		void registerCallbacks()
		{
			GVIAPListener.purchaseSuccess += callPurchaseSuccess;
			GVIAPListener.purchaseFail += callPurchaseFail;

		}



		void deregisterCallbacks()
		{
			GVIAPListener.purchaseSuccess -= callPurchaseSuccess;
			GVIAPListener.purchaseFail -= callPurchaseFail;
		}

		public void purchaseProduct()
		{
			registerCallbacks();
			//	InAppResponsePanels.instance?.ShowWaiting();
			UnityPurchaser.Instance.BuyProductID(productId);
		}
		int GetProdcutTypeIntfromEnum(ProductType type)
		{
			switch (type)
			{
				case ProductType.CONSUMEABLE:
					return 0;
				case ProductType.NONCONSUMABLE:
					return 1;
				case ProductType.SUBSCRIPTION:
					return 2;
				default:
					return 0;
			}

		}

		void callPurchaseSuccess(PurchaseEventArgs args)
		{
			if (args.purchasedProduct.definition.id == productId)
			{
				deregisterCallbacks();
				onPurchaseSuccess.Invoke();


				if (TenjinSdkInitializer.initializer)
					TenjinSdkInitializer.initializer.OnProcessPurchase(args);
				if (GVAnalysisManager.Instance)
					GVAnalysisManager.Instance.CustomEvent(args.purchasedProduct.definition.id + "_Success");

#if USE_FIREBASE
				Parameter[] PurchaseParameters = {
					 new Parameter(FirebaseAnalytics.ParameterItemId, args.purchasedProduct.definition.id.ToString()),
					 new Parameter(FirebaseAnalytics.ParameterPrice, _productPrice.ToString()),
					 new Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),


					};

				FirebaseAnalytics.LogEvent("IAP_Purchase", PurchaseParameters);
#endif

#if USE_APPSFLYER
				ProcessPurchase(args);
#endif

				//AppOpen
				Invoke(nameof(StopAppOpen), 1);
			}
		}

		void callPurchaseFail(string errorMsg)
		{
			deregisterCallbacks();
			onPurchaseFail.Invoke();

			//AppOpen
			Invoke(nameof(StopAppOpen), 1);
		}



		private void GVIAPListener_OnProductPriceSuccess(string prodID, string prodPrice)
		{
			if (prodID == productId)
			{
				deregisterCallbacks();

				Debug.Log(/*this,*/ "OnProductPriceSuccess Product ID: " + productId + " Price: " + prodPrice);
				_productPrice = prodPrice;
				isPriceLoaded = true;
				if (_priceTxt != null)
					_priceTxt.text = prodPrice;
			}
		}

		IEnumerator LoadItemPriceCR()
		{
			yield return new WaitForSecondsRealtime(5);
			UnityPurchaser.Instance.getProductPrice(productId);

		}


		void StopAppOpen()
		{
			AdConstants.IsAdWasShowing = false;
		}

#if USE_APPSFLYER
		// IAP AppsFlyer

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			
			if (String.Equals(args.purchasedProduct.definition.id, args.purchasedProduct.definition.id, StringComparison.Ordinal))
			{
				var price = args.purchasedProduct.metadata.localizedPrice;
				double revenue = decimal.ToDouble(price);
				string prodID = args.purchasedProduct.definition.id;
				string currency = args.purchasedProduct.metadata.isoCurrencyCode;
				var transactionID = args.purchasedProduct.transactionID;

				Dictionary<string, string> eventValues = new Dictionary<string, string>();
				eventValues.Add(AFInAppEvents.CURRENCY, currency);
				eventValues.Add(AFInAppEvents.REVENUE, revenue.ToString());
				eventValues.Add(AFInAppEvents.CONTENT_ID, prodID);

				AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);

#if UNITY_IOS && !UNITY_EDITOR
				
				AFSDKPurchaseDetailsIOS details = AFSDKPurchaseDetailsIOS.Init(prodID, revenue.ToString(), currency, transactionID);

				AppsFlyer.validateAndSendInAppPurchase(details, null, this);

#elif UNITY_ANDROID && !UNITY_EDITOR

				AFPurchaseDetailsAndroid details = new AFPurchaseDetailsAndroid(AFPurchaseType.Subscription, "token", prodID, revenue.ToString(), currency);

				AppsFlyer.validateAndSendInAppPurchase(details,null, this);
#endif

			}

			return PurchaseProcessingResult.Complete;
		}
#endif

	}
}