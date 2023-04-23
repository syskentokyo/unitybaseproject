
# About
初めて起動したときにウィンドウを表示する。





# Build Environment

| Title |   Version   | Memo  |       |
| :---: |:-----------:| :---: | :---: |
|  OS   |             |       |       |
| Unity | 2021.3.16f1 |       |       |
|       |             |       |       |


# Target Platform

| Platform | Support | Memo  |       |
| :------: |:-------:| :---: | :---: |
| Windows  |    ○    |       |       |
|  WebGL   |     ○    |       |       |
|   Mac    |     ○    |       |       |
|   iOS    |     ○    |       |       |
| Android  |     ○    |       |       |
|   tvOS   |     ○    |       |       |


# Detail

* ウィンドウを出現する条件パターン
  * Unity起動時に、プロジェクトに対して、初期化完了フラグがない場合
  * Unity起動時に、プロジェクトのユーザごとの設定ファイルに、初期化完了フラグがない場合


# Setting

* Unity起動時に、プロジェクトに対して、初期化完了フラグを保存する場合
  * ProjectCommonWindow.csのTouchedInit1Button、TouchedInit2Buttonに、初期化処理を書いてください

* Unity起動時に、プロジェクトのユーザごとの設定ファイルに、初期化完了フラグを保存する場合
  * PerUserWindow.csのTouchedInit1Button、TouchedInit2Buttonに、初期化処理を書いてください



# Licence

MIT


