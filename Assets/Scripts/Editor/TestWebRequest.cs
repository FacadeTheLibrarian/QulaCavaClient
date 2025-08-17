using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WebTest))]
internal sealed class TestWebRequest : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("テストリクエスト実行")) {
            WebTest webTest = target as WebTest;
            webTest.SendRequest();
        }
    }
}