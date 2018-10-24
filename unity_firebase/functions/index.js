// The Cloud Functions for Firebase SDK to create Cloud Functions and setup triggers.
// functionsという変数に格納されたオブジェクトが取得できる
const functions = require('firebase-functions');

// The Firebase Admin SDK to access the Firebase Realtime Database.
const admin = require('firebase-admin');
admin.initializeApp();

// @memo. 以下の様な形で処理が実装できる

// @memo. https://us-central1-testproject-e2271.cloudfunctions.net/selectUserIndex?text=引数
// @memo. ブラウザで叩くとres.send()の内容が表示される
exports.selectUserIndex = functions.https.onRequest((req, res) => {
    const original = req.query.text;
    console.log('original:', original);
    if (original == 'develop') {
        res.send(original);
    } else if (original == 'real') {
        res.send(original);
    } else {
        res.send(original);
    }
    return null;
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

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//  response.send("Hello from Firebase!");
// });
