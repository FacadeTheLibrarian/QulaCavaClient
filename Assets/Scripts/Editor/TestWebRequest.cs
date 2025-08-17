using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WebTest))]
internal sealed class TestWebRequest : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("テストリクエスト実行")) {
            ITestRequestableOnInspector webTest = target as ITestRequestableOnInspector;
            webTest.SendAnyRequest();
        }
    }
}