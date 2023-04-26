using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{
    enum SetupStatus
    {
        Init,
        ImportDefaultPackage,
        CreateDirectory,
        Git,
        UnityProjectSetting,
        AddLayer,
        
        End
    }
    
    
    public class BaseProjectManager
    {

        public Action _completedSetUpAction;

        private SetupConfig _currentConfig;

        private SetupStatus _currentSetupStatus = SetupStatus.Init;

        private int _currentPackageIndex = 0;
        private AddRequest _currentAddRequest = null;
        
        public void StartSetup()
        {
            EditorApplication.update += OnEditorUpdate;

            _currentConfig = SearchSetUpConfig();
            
            _currentSetupStatus = SetupStatus.Init;
            StartNextProcess();
        }





        #region 共通

        private void StartNextProcess()
        {
            switch (_currentSetupStatus)
            {
                case SetupStatus.Init:
                {
                    _currentSetupStatus = SetupStatus.ImportDefaultPackage;
                    StartDefaultPackage();
                    break;
                }
                case SetupStatus.ImportDefaultPackage:
                {
                    _currentSetupStatus = SetupStatus.CreateDirectory;
                    StartCreateDirectory();
                    break;
                }
                case SetupStatus.CreateDirectory:
                {
                    _currentSetupStatus = SetupStatus.Git;
                    break;
                }
                case SetupStatus.Git:
                {
                    _currentSetupStatus = SetupStatus.UnityProjectSetting;
                    break;
                }
                case SetupStatus.UnityProjectSetting:
                {
                    _currentSetupStatus = SetupStatus.AddLayer;

                    break;
                }
                
                case SetupStatus.AddLayer:
                    _currentSetupStatus = SetupStatus.End;
                    CompleteSetup();
                    break;
                    
            }
        }
        
        private void OnEditorUpdate()
        {
            switch (_currentSetupStatus)
            {
                case SetupStatus.Init:
                {

                    break;
                }
                case SetupStatus.ImportDefaultPackage:
                {
                    UpdateDefaultPackage();
                    break;
                }
                case SetupStatus.CreateDirectory:
                {

                    break;
                }
                case SetupStatus.Git:
                {

                    break;
                }
                case SetupStatus.UnityProjectSetting:
                {

                    break;
                }

                case SetupStatus.AddLayer:
                {
                    break;
                }

            }
        }
        
                
        private  SetupConfig SearchSetUpConfig()
        {
            SetupConfig setupConfig = null;
            string[] guids = AssetDatabase.FindAssets("t:SetupConfig");
            guids.ToList().ForEach(nextGUID =>
            {
                string filePath = AssetDatabase.GUIDToAssetPath(nextGUID);
                setupConfig = AssetDatabase.LoadAssetAtPath<SetupConfig>(filePath);

            });
            
            return setupConfig;
        }
        
        #endregion
        
        #region デフォルトパッケージ

        
        private void StartDefaultPackage()
        {
            Debug.Log("おすすめのパッケージインポート");
            _currentPackageIndex = 0;
            StartImportDefaultPackage();
        }

        private void StartImportDefaultPackage()
        {
            string nextPackageID = _currentConfig.GetTargetPackageIDList[_currentPackageIndex];
            Debug.Log("次のパッケージ:"+nextPackageID);
            _currentAddRequest =  Client.Add(nextPackageID);
        }

        private void UpdateDefaultPackage()
        {
            if (_currentAddRequest == null) return;

            if (_currentAddRequest.Status == StatusCode.Success)
            {
                _currentAddRequest = null;
                
                _currentPackageIndex++;
                
                if (_currentPackageIndex < _currentConfig.GetTargetPackageIDList.Count)
                {
                    //次のインポート開始
                    StartImportDefaultPackage();
                }
                else
                {
                    //すべてインポート終了
                    Debug.Log("すべてインポート終了");
                    StartNextProcess();//次の処理
                }
            }
        }

        
        #endregion

        #region ディレクトリ作成

        private void StartCreateDirectory()
        {
            CreateAllDirectoryProcess();
        }

        private void CreateAllDirectoryProcess()
        {
            _currentConfig.GetCreateDirectoryPathList.ForEach(dirPath =>
            {
                if (dirPath != "")
                {
                    CreateDirectoryProcess(dirPath);
                }
            });
        }

        private void CreateDirectoryProcess(string dirPath)
        {
            Debug.Log("ディレクトリ作成します。 "+dirPath);
            string saveDirPath = Path.GetDirectoryName(Application.dataPath);
            string createDirPath = saveDirPath+"/"+dirPath;
            string gitkeepPath = createDirPath + "/" + ".gitkeep";
            if (Directory.Exists(createDirPath) == false)
            {
                //サブディレクトリ作成
                Directory.CreateDirectory(createDirPath);
            }
            
            //GitKeep追加
            if (File.Exists(gitkeepPath) == false)
            {
                File.WriteAllText(gitkeepPath,"");
            }
        }
        

        #endregion



        private void CompleteSetup()
        {
            EditorApplication.update -= OnEditorUpdate;
            
            _completedSetUpAction?.Invoke();//終了の通知
        }
    }
}