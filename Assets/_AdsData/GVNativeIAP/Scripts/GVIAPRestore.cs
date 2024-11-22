using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace GVNativeIAP
{
    [System.Serializable]
    public class GVIAPCustomEventRestore : UnityEvent<string> { }

    public class GVIAPRestore : MonoBehaviour
    {

        //public GVIAPCustomEventRestore onRestoreFetch;

        [Space(20)]  //added
        public GVIAPCustomEventRestore onRestoreSuccess;
        public GVIAPCustomEventRestore onRestoreFail;

        void Start()
        {
            registerCallbacks();  //added
        }

        void OnEnable()
        {
            //GVIAPListener.purchaseRestore += callOnRestoreFetch;
        }


        void OnDisable()
        {
            //GVIAPListener.purchaseRestore -= callOnRestoreFetch;
        }

        public void fetchRestoreIds()
        {
            //IAPBehaviour.instance?.InAppButtonClick();
            UnityPurchaser.Instance.RestorePurchases();
        }

        public void callOnRestoreFetch(IAPRestoreIDs restoreIds)
        {
            //onRestoreFetch.Invoke (restoreIds);
        }

        void registerCallbacks()
        {
            GVIAPListener.purchaseSuccess += callRestoreSuccess;
            GVIAPListener.purchaseFail += callRestoreFail;

        }



        void deregisterCallbacks()
        {
            GVIAPListener.purchaseSuccess -= callRestoreSuccess;
            GVIAPListener.purchaseFail -= callRestoreFail;
        }


        void callRestoreSuccess(PurchaseEventArgs args)
        {
            //IAPBehaviour.instance?.InAppSuccessTrigger();
            //deregisterCallbacks();
            Debug.Log("GVIAPRestore: callRestoreSuccess() Product ID: " + args.purchasedProduct.definition.id);
            onRestoreSuccess.Invoke(args.purchasedProduct.definition.id);
            switch (args.purchasedProduct.definition.id)
            {
                case "shape_transform_remove_ads":
                    AdConstants.disableAds();
                    break;
            }

        }

        void callRestoreFail(string errorMsg)
        {
            //deregisterCallbacks();
            //IAPBehaviour.instance?.InAppFailureTrigger();
            onRestoreFail.Invoke("");
        }




    }
}