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
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("個別メニュー");
            if (GUILayout.Button("Create Directory"))
            {
                _baseProjectManager.InitReadConfig();
                _baseProjectManager.CreateAllDirectory();
            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Setting Git"))
            {
                _baseProjectManager.InitReadConfig();
                _baseProjectManager.StartGitConfig();
            }
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Setting Unity Project"))
            {
                _baseProjectManager.InitReadConfig();
                _baseProjectManager.StartUnityProjectConfig();
            }
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Re Add Unity Layer"))
            {
                _baseProjectManager.InitReadConfig();
                _baseProjectManager.StartAddLayer();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("自動で設定できない項目");
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("推奨設定");
            // EditorGUILayout.LabelField("手動で変更することをおすすめします");
            // EditorGUILayout.LabelField("1.Edit -> Project Settings のEditorのParallel Importをオン");
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