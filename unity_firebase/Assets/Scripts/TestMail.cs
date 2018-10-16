using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMail : MonoBehaviour {
    // @memo. 変数にアンダーバーをつけた場合
    // @memo. Firebaseに書き込むためにJson化するとアンダーバーも付いてきます、恐らくスネークケースもそのままの形で登録されてしまうはずです
    public string username_;
    public string email_;

    public TestMail()
    {

    }

    public TestMail(string _username, string _email)
    {
        this.username_ = _username;
        this.email_ = _email;
    }
}
