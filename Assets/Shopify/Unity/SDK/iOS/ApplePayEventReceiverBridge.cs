namespace Shopify.Unity.SDK.iOS {
    using System.Collections.Generic;
    using System.Collections;

#if !SHOPIFY_MONO_UNIT_TEST
    using UnityEngine;
    public partial class ApplePayEventReceiverBridge : MonoBehaviour { }
#endif

#if UNITY_IOS
    public partial class ApplePayEventReceiverBridge : IApplePayEventReceiver {
        public IApplePayEventReceiver Receiver;

        public void UpdateSummaryItemsForShippingIdentifier(string serializedMessage) {
            Receiver.UpdateSummaryItemsForShippingIdentifier(serializedMessage);
        }

        public void UpdateSummaryItemsForShippingContact(string serializedMessage) {
            Receiver.UpdateSummaryItemsForShippingContact(serializedMessage);
        }

        public void FetchApplePayCheckoutStatusForToken(string serializedMessage) {
            Receiver.FetchApplePayCheckoutStatusForToken(serializedMessage);
        }

        public void DidFinishCheckoutSession(string serializedMessage) {
            Receiver.DidFinishCheckoutSession(serializedMessage);
        }
    }
#endif
}