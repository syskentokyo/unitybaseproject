using UnityEditor;
using UnityEngine;

namespace SyskenTLib.FirstWindowsEditor.Window
{
    public class ProjectCommonWindow : EditorWindow
    {
        [MenuItem("SyskenTLib/FirstWindow/ProjectCommonWindow", priority = 10)]
        public static void ShowWindow()
        {
            var window = GetWindow<ProjectCommonWindow>();
            window.titleContent = new GUIContent("ProjectCommonWindow");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("Box");



            EditorGUILayout.LabelField("Init");
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Initボタンを押すと、プロジェクト対して、初期化したことを保存します。");
            EditorGUILayout.LabelField("他の人が同じプロジェクトを開くと、初期化したことになります。");
            if (GUILayout.Button("Delete Key"))
            {
                TouchedDeleteKeyButton();
            }

            
            if (GUILayout.Button("Init1"))
            {
                InitCommon();
                TouchedInit1Button();
            }

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Init2"))
            {
                InitCommon();
                TouchedInit2Button();
            }
            
            EditorGUILayout.Space(10);

            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }


        private void InitCommon()
        {
            //Init実行したことを保存する
            SaveDataManager saveDataManager = new SaveDataManager();
            saveDataManager.SaveProjectCommonConfig();
        }

        private void TouchedInit1Button()
        {
            //
            // MEMO: ここに初期化処理を書いてください
            //
            
            
            this.Close();
        }

        private void TouchedInit2Button()
        {
            //
            // MEMO: ここに初期化処理を書いてください
            //
            
            
            this.Close();
        }
        
        
        private void TouchedDeleteKeyButton()
        {
            //もう１度、初回表示されるようにします。
            SaveDataManager saveDataManager = new SaveDataManager();
            saveDataManager.RemoveProjectCommon();
        }
    }
}
