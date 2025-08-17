using System.Threading;
using Cysharp.Threading.Tasks;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Text;
using System;
using MiniJSON;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

internal sealed class WebTest : MonoBehaviour, ITestRequestableOnInspector {

    [SerializeField] private string _username = "yadori_ouchi";
    [SerializeField] private string _password = "ouchi";

    private CancellationToken _token = default;
    public void SetUp(CancellationToken token) {
        _token = token;
    }
    /*
 *  [Host] => 127.0.0.1
 *  [User-Agent] => UnityPlayer/2023.1.20f1 (UnityWebRequest/1.0, libcurl/8.4.0-DEV)
 *  [Accept] => 
 *  [Accept-Encoding] => deflate, gzip
 *  [Content-Type] => application/x-www-form-urlencoded
 *  [X-Unity-Version] => 2023.1.20f1
 *  [Content-Length] => 36
*/
    public async UniTask SendAnyRequest() {
        UnityWebRequest.ClearCookieCache();
        string challange = string.Empty;
        string url = @"http://127.0.0.1/qula-cava/";
        WWWForm form = new WWWForm();
        form.AddField("username", _username);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form)) {
            request.timeout = ServerUtility.DEFAULT_TIMEOUT;
            request.SetRequestHeader(ServerUtility.CONTENT_TYPE, ServerUtility.CONTENT_TYPE_X_WWW_FORM);
            request.downloadHandler = new DownloadHandlerBuffer();
            try {
                await request.SendWebRequest().ToUniTask(cancellationToken: _token);
            }
            catch {
                Debug.LogError("Request was cancelled or failed to send.");
                return;
            }
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError($"Error: {request.error}");
                return;
            }
            string response = request.downloadHandler.text;
            Debug.Log(response);
            challange = "Hello, world!";
        }



        string loginUrl = @"http://127.0.0.1/qula-cava/login.php";

        string hashedPassword = string.Empty;
        string hashedResponse = string.Empty;
        using (SHA256 sha256 = SHA256.Create()) {
            byte[] byteCodes = Encoding.UTF8.GetBytes(_password);
            byte[] hashed = sha256.ComputeHash(byteCodes);
            StringBuilder stringified = new StringBuilder();
            foreach (byte b in hashed) {
                stringified.Append(b.ToString("x2"));
            }
            hashedPassword = stringified.ToString();
        }
        using (SHA256 sha256 = SHA256.Create()) {
            string stringResponse = hashedPassword + challange;
            byte[] byteCodes = Encoding.UTF8.GetBytes(stringResponse);
            byte[] hashed = sha256.ComputeHash(byteCodes);

            StringBuilder stringified = new StringBuilder();
            foreach (byte b in hashed) {
                stringified.Append(b.ToString("x2"));
            }
            hashedResponse = stringified.ToString();
        }
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("username", _username);
        loginForm.AddField("response", hashedResponse);

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, loginForm)) {
            request.timeout = ServerUtility.DEFAULT_TIMEOUT;
            request.SetRequestHeader(ServerUtility.CONTENT_TYPE, ServerUtility.CONTENT_TYPE_X_WWW_FORM);
            request.downloadHandler = new DownloadHandlerBuffer();
            try {
                await request.SendWebRequest().ToUniTask(cancellationToken: _token);
            }
            catch {
                Debug.LogError("Request was cancelled or failed to send.");
                return;
            }
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError($"Error: {request.error}");
                return;
            }
            string response = request.downloadHandler.text;
            Debug.Log(response);
        }
    }
}

