using UnityEditor;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{
    public class SetupWindow : EditorWindow
    {
        private BaseProjectManager _baseProjectManager;
        
        public static void ShowSetUpWindow()
        {
            ShowWindow();;
        }
        
        
        [MenuItem("SyskenTLib/BaseProject/SetupWindow", priority = 1)]
        private static void ShowWindow()
        {
            var window = GetWindow<SetupWindow>();
            window.titleContent = new UnityEngine.GUIContent("SetupWindow");
            window.Show();
        }

        private void OnGUI()
        {
            if (_baseProjectManager == null)
            {
                _baseProjectManager = new BaseProjectManager();
                _baseProjectManager._completedSetUpAction += OnCompleteSetUp;
            }
            
            
            EditorGUILayout.BeginVertical("Box");



            EditorGUILayout.LabelField("Init");
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("プロジェクトへ最低限の設定を行います");

            if (GUILayout.Button("Init"))
            {
                _baseProjectManager.StartSetup();
            }
            
            EditorGUILayout.Space(10);

            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }

        private void OnCompleteSetUp()
        {
            Debug.Log("初期設定終わり");
            
            this.Close();
        }
    }
}