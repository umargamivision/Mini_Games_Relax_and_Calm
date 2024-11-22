using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class UnityPurchaser : MonoBehaviour, IStoreListener
{
    public GVNativeIAP.GVIAPListener _gVIAPListener;
    private string _currentProductID;
    public string[] IAP_SKUS_Consumables;
    public string[] IAP_SKUS_NonConsumables;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


    public static string kProductIDConsumable = "consumable";
    public static string kProductIDNonConsumable = "nonconsumable";
    public static string kProductIDSubscription = "subscription";

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";



    private static UnityPurchaser _instance = new UnityPurchaser();

    private UnityPurchaser() { }

    public static UnityPurchaser Instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<UnityPurchaser>();
            }
            return _instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        if (IAP_SKUS_Consumables != null)
            foreach (string productID in IAP_SKUS_Consumables)
                builder.AddProduct(productID, ProductType.Consumable);

        if (IAP_SKUS_NonConsumables != null)
            foreach (string productID in IAP_SKUS_NonConsumables)
                builder.AddProduct(productID, ProductType.NonConsumable);

        //builder.AddProduct("", ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder); // ASAD
    }


    public bool IsInitialized()
    {

        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyProductID(string productId)
    {

        if (IsInitialized())
        {

            _currentProductID = productId;
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
                //IAPBehaviour.instance?.InAppFailureTrigger();
            }
            else
            {
                Debug.Log("BuyProductID:" + product.definition.id +
                    " FAIL. Not purchasing product, either is not found or is not available for purchase");
            }

            AdConstants.IsAdWasShowing = true;

        }
        else
        {
            //IAPBehaviour.instance?.InAppFailureTrigger();
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void getProductPrice(string productId)
    {
        if (productId != null)
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null)
            {
                _gVIAPListener.productPriceSuccessFull(productId, product.metadata.localizedPriceString);
            }
        }
        else
        {
            Debug.Log("getProductPrice: Fail...");
        }
    }
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        if (error != null)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'",
                args.purchasedProduct.definition.id));
        if (args.purchasedProduct.definition.id != null)
        {
            _gVIAPListener.purchaseSuccessfull(args);
        }
        return PurchaseProcessingResult.Complete;
    }

   
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
        _gVIAPListener.purchaseFailed(failureReason.ToString());
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("On Initialized Failed");
        //   throw new NotImplementedException();
    }
}

