// The Cloud Functions for Firebase SDK to create Cloud Functions and setup triggers.
// functionsという変数に格納されたオブジェクトが取得できる
const functions = require('firebase-functions');

// The Firebase Admin SDK to access the Firebase Realtime Database.
const admin = require('firebase-admin');
admin.initializeApp();

// @memo. 以下の様な形で処理が実装できる
// @memo. コンソール上から削除したい場合はGoogleCloudPlatformから削除できる

/**
 * @brief ユーザーデータを取得
 */
// @memo. https://us-central1-testproject-e2271.cloudfunctions.net/selectUserData?pass=ユーザーID
exports.selectUserData = functions.https.onRequest((req, res) => {
    const originalIndex = req.query.pass;
    var path = "users/" + originalIndex;
    //console.log('path:' + path);

    admin.database().ref(path).once('value').then(function (snapshot) {
        var json = snapshot.val();
        res.send(json);
        return null;
    }).catch(error => {
        console.error(error);
        res.error(-1);
    });
});

/**
 * @brief ユーザーインデックスを取得
 */
// @memo. https://us-central1-testproject-e2271.cloudfunctions.net/selectUserIndex?pass=ユーザーID
exports.selectUserIndex = functions.https.onRequest((req, res) => {
    const originalIndex = req.query.pass;
    var path = "users/" + originalIndex + "/userIndex";
    //console.log('path:' + path);

    admin.database().ref(path).once('value').then(function (snapshot) {
        var index = snapshot.val();
        res.send(index);
        return null;
    }).catch(error => {
        console.error(error);
        res.error(-1);
    });
});

/**
 * @brief デバイスインデックスを取得
 */
// @memo. https://us-central1-testproject-e2271.cloudfunctions.net/selectUserIndex?pass=ユーザーID
exports.selectDeviceIndex = functions.https.onRequest((req, res) => {
    const originalIndex = req.query.pass;
    var path = "users/" + originalIndex + "/deviceIndex";
    //console.log('path:' + path);

    admin.database().ref(path).once('value').then(function (snapshot) {
        var index = snapshot.val();
        res.send(index);
        return null;
    }).catch(error => {
        console.error(error);
        res.error(-1);
    });
});

// expressというnodeモジュールを使用して通信
// @memo. https://us-central1-testproject-e2271.cloudfunctions.net/addMessage?text=引数
// @memo. Detabase上に下記で指定した「messages」テーブルの中に一意のIndexが作成され、その直下に「originai:」のキーと引数の文字列が格納された
exports.addMessage = functions.https.onRequest((req, res) => {
    const original = req.query.text;

    admin.database().ref('/messages').push({ original: original }).then((snapshot) => {
        res.redirect(303, snapshot.ref.toString());
        return null;
    }).catch(error => {
        console.error(error);
        res.error(500);
    });
});

/**
 * @brief プッシュ通知
 */
// @memo.
exports.pushNotification = functions.https.onRequest((req, res) => {
    const userIndex = req.body.userIndex;
    var path = "users/" + userIndex + "/token";
    //console.log('path:' + path);

    admin.database().ref(path).once('value').then(function (snapshot) {
        var token = snapshot.val();
        const payload = {
            notification: {
                title: 'プッシュタイトル',
                body: `プッシュ内容`,
                icon: 'アイコンアドレス(ex)Storageに上がっているものなど)'
            }
        };

        return admin.messaging().sendToDevice(token, payload);
    }).catch(error => {
        console.log(error);
        res.error(-1);
    });
});

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//  response.send("Hello from Firebase!");
// });
