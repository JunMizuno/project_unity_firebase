using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;

public class FirebaseDatabaseManager : MonoBehaviour {

    /// <summary>
    /// 開始時
    /// </summary>
    void Start () {
        CheckAvailableGooglePlayDeveloperService();

        // リアルタイムデータベースのAPIを呼び出す前に必ず設定する
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testproject-e2271.firebaseio.com/");
    }

    /// <summary>
    /// GooglePlay開発者サービスのバージョン確認
    /// </summary>
    private void CheckAvailableGooglePlayDeveloperService() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Set a flag here indiciating that Firebase is ready to use by your
                // application.
            }
            else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

}
