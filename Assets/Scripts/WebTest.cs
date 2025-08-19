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
    public async UniTask SendAnyRequest() {
        UnityWebRequest.ClearCookieCache();
        LoginChallenge challenge = default;
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
            challenge = JsonUtility.FromJson<LoginChallenge>(response);
        }

        string loginUrl = @"http://127.0.0.1/qula-cava/login.php";

        string hashedPassword = SHA256Wrapper.Hash(_password);
        string hashedResponse = SHA256Wrapper.Hash(hashedPassword + challenge.data);
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

