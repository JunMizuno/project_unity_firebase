using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class FirebaseAuthManager : MonoBehaviour
{
    FirebaseAuth auth_ = null;
    FirebaseUser user_ = null;

    /// <summary>
    /// 開始時
    /// </summary>
	void Start()
    {
        InitializeFirebase();
    }

    /// <summary>
    /// FirebaseのAuthモジュールを初期化
    /// </summary>
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth_ = FirebaseAuth.DefaultInstance;
        auth_.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    /// <summary>
    /// ユーザー認証変更時のコールバックリスナー
    /// </summary>
    /// <param name="_sender">Sender.</param>
    /// <param name="_eventArgs">Event arguments.</param>
    void AuthStateChanged(object _sender, System.EventArgs _eventArgs)
    {
        if (auth_.CurrentUser != user_)
        {
            bool signedIn = user_ != auth_.CurrentUser && auth_.CurrentUser != null;
            if (!signedIn && user_ != null)
            {
                Debug.Log("Signed out " + user_.UserId);
            }

            user_ = auth_.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user_.UserId);
            }
        }
    }

    void OnDestroy()
    {
        auth_.StateChanged -= AuthStateChanged;
        auth_ = null;
    }

    /// <summary>
    /// ユーザー登録
    /// </summary>
    void RegistrationUserInfomation(string _email, string _password)
    {

    }
}
