using SyskenTLib.BaseProject.Base.Editor;
using SyskenTLib.FirstWindowsEditor.Window;
using UnityEditor;
using UnityEngine;

namespace SyskenTLib.FirstWindowsEditor
{
    [InitializeOnLoad]
    public class StartAppManager
    {
        static StartAppManager(){
            // Debug.Log("起動トリガー発動");
            EditorApplication.delayCall += DelayCallStartApp;
            

            
            
        }

        static void DelayCallStartApp()
        {
            //Unityの初期化終わったタイミングで呼ばれる
            
            //チェック呼び出し
            StartProjectCommonWindow();
            
            
            //
            // MEMO ユーザごと呼び出し
            //
            // StartPerUserWindow();
        }

        private static void StartProjectCommonWindow()
        {
            SaveDataManager saveDataManager = new SaveDataManager();

            if (saveDataManager.IsExistProjectCommonConfig()==false)
            {
                saveDataManager.SaveProjectCommonConfig();//設定を開いたことにする
                
                //まだ処理してない場合
                // PerUserWindow.ShowWindow();
                
                SetupWindow.ShowSetUpWindow();
                
                
            }
            else
            {
                // Debug.Log("StartProjectCommonWindow：起動処理完了してました");
            }
        }
        

        private static void StartPerUserWindow()
        {
            
            SaveDataManager saveDataManager = new SaveDataManager();
            if (saveDataManager.IsExistUserConfig()==false)
            {
                //まだ処理してない場合
                ProjectCommonWindow.ShowWindow();
            } else
            {
                // Debug.Log("StartPerUserWindow：起動処理完了してました");
            }
        }
        
        
        
        
    }
}