#if !SHOPIFY_MONO_UNIT_TEST
namespace Shopify.Unity.SDK {
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;
    using System;
    using Shopify.Unity.MiniJSON;
    using UnityEngine;

    internal class LoaderComponent : MonoBehaviour {
        string URL;
        LoaderResponseHandler Callback;
        Dictionary<string, string> Headers;

        public LoaderComponent SetURL(string url) {
            URL = url;
            return this;
        }

        public LoaderComponent SetHeaders(Dictionary<string, string> headers) {
            Headers = headers;
            return this;
        }

        public void Load(string query, LoaderResponseHandler callback) {
            Callback = callback;
            byte[] body = Encoding.ASCII.GetBytes(query);
            WWW w = new WWW(URL, body, Headers);
            StartCoroutine(DoWWW(w));
        }

        private IEnumerator DoWWW(WWW w) {
            yield return w;

            if (!string.IsNullOrEmpty(w.error)) {
                Callback(null, w.error);
            } else {
                Callback(w.text, null);
            }

            w.Dispose();

#if UNITY_EDITOR
            MonoBehaviour.DestroyImmediate(this);
#else
            MonoBehaviour.Destroy(this);
#endif
        }
    }

    /// <summary>
    /// Performs network communication to send GraphQL queries between Unity and a Shopify GraphQL endpoint.
    /// </summary>
    public class UnityLoader : BaseLoader {
        private Dictionary<string, string> _Headers = new Dictionary<string, string>();

        /// <param name="domain">Shopify store domain that you'd like to query on</param>
        /// <param name="accessToken">access token used to communicate with the Shopify Store</param>
        public UnityLoader(string domain, string accessToken) : base(domain, accessToken) { }

        /// <summary>
        /// Send a query to the Storefront API.
        /// </summary>
        /// <param name="query">Query that will be sent to the Storefront API</param>
        /// <param name="callback">callback that receives a response</param>
        override public void Load(string query, LoaderResponseHandler callback) {
            GlobalGameObject.AddComponent<LoaderComponent>()
                .SetURL("https://" + Domain + "/api/graphql.json")
                .SetHeaders(_Headers)
                .Load(query, callback);
        }

        override public void SetHeader(string key, string value) {
            _Headers[key] = value;
        }

        override public string SDKVariantName() {
            return "unity";
        }
    }

    public class UnityLoaderProvider : ILoaderProvider {
        BaseLoader ILoaderProvider.GetLoader(string accessToken, string domain) {
            return new UnityLoader(domain, accessToken);
        }
    }
}
#endif