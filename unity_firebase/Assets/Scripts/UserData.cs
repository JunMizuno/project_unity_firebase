using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project;

public class UserData
{
    // @memo. privateにするとFirebaseに書き込むためにJson化するとデータが取得できない
    // @memo. 例外的にpublic宣言しています
    // @memo. キャメルケースやパスカルケースもそのままの形でカラム名として登録されます
    public int userIndex;
    public int UserIndex
    {
        get
        {
            return userIndex;
        }
    }

    public int deviceIndex;
    public int DeviceIndex
    {
        get
        {
            return deviceIndex;
        }
    }

    public string userName;
    public string UserName
    {
        get
        {
            return userName;
        }
        set
        {
            userName = value;
        }
    }

    public bool isNew;
    public bool IsNew
    {
        get
        {
            return isNew;
        }
        set
        {
            isNew = value;
        }
    }

    public UserData()
    {

    }

    public UserData(int _userIndex)
    {
        userIndex = _userIndex;

#if UNITY_ANDROID
        deviceIndex = (int)GlobalDefine.Device.Android;
#elif UNITY_IOS
        deviceIndex_ = (int)GlobalDefine.Device.iOS;
#else
        deviceIndex_ = (int)GlobalDefine.Device.Others;
#endif
    }
}
