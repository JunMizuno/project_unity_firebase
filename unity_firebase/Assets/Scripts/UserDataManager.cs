using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour {
    UserDataManager instance = null;

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
	}
}
