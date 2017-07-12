#if UNITY_IOS
namespace Shopify.Tests {
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Shopify.Unity;
    using Shopify.Unity.SDK;
    using Shopify.Unity.SDK.iOS;

    [TestFixture]
    public class TestWebCheckout {
        [SetUp]
        public void Setup() {
            #if (SHOPIFY_TEST)
            ShopifyBuy.Reset();
            #endif

            ShopifyBuy.Init(new MockLoader());
        }

        [Test]        
        public void TestOnNativeMessageDismissedWithValidCheckout() {
            bool didSucceed = false;
            bool didCancel = false;
            bool didFail = false;

            var webViewMessageReceiver = CreateMockMessageReceiver("checkout-id");

            webViewMessageReceiver.OnSuccess = () => didSucceed = true;
            webViewMessageReceiver.OnCancelled = () => didCancel = true;
            webViewMessageReceiver.OnFailure = (e) => didFail = true;

            string mockMessage = "{\"Identifier\": \"test\", \"Content\": \"dismissed\"}";
            webViewMessageReceiver.OnNativeMessage(mockMessage);

            Assert.IsTrue(didSucceed);
            Assert.IsFalse(didCancel);
            Assert.IsFalse(didFail);
        }

        [Test]        
        public void TestOnNativeMessageDismissedWithInvalidCheckout() {
            bool didSucceed = false;
            bool didCancel = false;
            bool didFail = false;

            var webViewMessageReceiver = CreateMockMessageReceiver("checkout-id-failed");

            webViewMessageReceiver.OnSuccess = () => didSucceed = true;
            webViewMessageReceiver.OnCancelled = () => didCancel = true;
            webViewMessageReceiver.OnFailure = (e) => didFail = true;

            string mockMessage = "{\"Identifier\": \"test\", \"Content\": \"dismissed\"}";
            webViewMessageReceiver.OnNativeMessage(mockMessage);

            Assert.IsFalse(didSucceed);
            Assert.IsFalse(didCancel);
            Assert.IsTrue(didFail);
        }

        private iOSWebViewMessageReceiver CreateMockMessageReceiver(string checkoutId) {

            var webViewMessageReceiver = new iOSWebViewMessageReceiver();

            var cartDict = new Dictionary<string, object> { {"id", checkoutId} };
            webViewMessageReceiver.CurrentCheckout = new Checkout(cartDict);
            webViewMessageReceiver.Client = ShopifyBuy.Client();

            return webViewMessageReceiver;
        }
    }
}
#endif
