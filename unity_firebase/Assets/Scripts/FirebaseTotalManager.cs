﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseTotalManager : MonoBehaviour {
    private FirebaseTotalManager instance_ = null;

    [SerializeField]
    public FirebaseDatabaseManager databaseManager_;

    [SerializeField]
    public FirebaseStorageManager storageManager_;

    [SerializeField]
    public FirebaseMessageManager messageManager_;






    /// <summary>
    /// 起動時
    /// </summary>
    private void Awake()
    {
        if (!instance_) 
        {
            instance_ = this;
        }

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 開始時
    /// </summary>
	void Start ()
    {
        string result = "";
        StartCoroutine(Utility.RequestFirebaseFunction("https://us-central1-testproject-e2271.cloudfunctions.net/selectUserData" + "?pass=test01", r => {
            result = r;
            Debug.Log("<color=cyan>" + result + "</color>");
        }));
    }
}
