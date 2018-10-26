using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Messaging;

public class FirebaseMessageManager : MonoBehaviour {
    private string token_ = "";
    public string Token 
    {
        get 
        {
            return token_;
        }
    }

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () 
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
	}

    /// <summary>
    /// トークン受け取り時のリスナー
    /// </summary>
    /// <param name="_sender">Sender.</param>
    /// <param name="_token">Token.</param>
    public void OnTokenReceived(object _sender, TokenReceivedEventArgs _token)
    {
        Debug.Log("Received Registration Token: " +_token.Token);
        token_ = _token.Token;
    }

    /// <summary>
    /// メッセージ受け取り時のリスナー
    /// </summary>
    /// <param name="_sender">Sender.</param>
    /// <param name="_e">E.</param>
    public void OnMessageReceived(object _sender, MessageReceivedEventArgs _e)
    {
        Debug.Log("From: " + _e.Message.From);
        Debug.Log("Message ID: " + _e.Message.MessageId);
    }
}
