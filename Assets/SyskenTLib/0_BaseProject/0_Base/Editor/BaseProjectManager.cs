using System;
using System.Collections.Generic;
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

        private BaseSetupConfig _currentBaseSetupConfig;
        private GitSetupConfig _currentGitSetupConfig;

        private SetupStatus _currentSetupStatus = SetupStatus.Init;

        private int _currentPackageIndex = 0;
        private AddRequest _currentAddRequest = null;
        
        public void StartSetup()
        {
            EditorApplication.update += OnEditorUpdate;

            _currentBaseSetupConfig = SearchSetUpConfig();
            _currentGitSetupConfig = SearchGitSetUpConfig();
            
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
                    StartGitSetting();
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
        
                
        private  BaseSetupConfig SearchSetUpConfig()
        {
            BaseSetupConfig baseSetupConfig = null;
            string[] guids = AssetDatabase.FindAssets("t:BaseSetupConfig");
            guids.ToList().ForEach(nextGUID =>
            {
                string filePath = AssetDatabase.GUIDToAssetPath(nextGUID);
                baseSetupConfig = AssetDatabase.LoadAssetAtPath<BaseSetupConfig>(filePath);

            });
            
            return baseSetupConfig;
        }
        
        private  GitSetupConfig SearchGitSetUpConfig()
        {
            GitSetupConfig nextSetupConfig = null;
            string[] guids = AssetDatabase.FindAssets("t:GitSetupConfig");
            guids.ToList().ForEach(nextGUID =>
            {
                string filePath = AssetDatabase.GUIDToAssetPath(nextGUID);
                nextSetupConfig = AssetDatabase.LoadAssetAtPath<GitSetupConfig>(filePath);

            });
            
            return nextSetupConfig;
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
            string nextPackageID = _currentBaseSetupConfig.GetTargetPackageIDList[_currentPackageIndex];
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
                
                if (_currentPackageIndex < _currentBaseSetupConfig.GetTargetPackageIDList.Count)
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
            
            StartNextProcess();//次の処理
        }

        private void CreateAllDirectoryProcess()
        {
            _currentBaseSetupConfig.GetCreateDirectoryPathList.ForEach(dirPath =>
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
        
        #region Git

        private void StartGitSetting()
        {
            Debug.Log("Gitの設定開始");
            StartGitSettingProcess();
            StartNextProcess();//次の処理
        }

        private void StartGitSettingProcess()
        {
            CreateBaseGitConfig();
            CreateBaseGitLFSConfig();
            
            //それぞれのGitLFS設定
            Create3DModelGitLFSConfig();
            CreateImageGitLFSConfig();
            CreateVideoGitLFSConfig();
            CreateAudioGitLFSConfig();
        }

        private void CreateBaseGitConfig()
        {
            
            Debug.Log("GitのIgnore作成。 ");
            string configText = _currentGitSetupConfig.GetGitIgnore;
            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);
            string gitkeepPath = unityprojectDirPath + "/" + ".gitignore";
            
            //設定ファイル作成
            File.WriteAllText(gitkeepPath,configText);
            
        }
        
        private void CreateBaseGitLFSConfig()
        {
            
            Debug.Log("GitLFSの.gitattributes作成。 ");
            string configText = _currentGitSetupConfig.GetBaseGitLFS;
            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);
            string gitLFSPath = unityprojectDirPath + "/" + ".gitattributes";
            
            //設定ファイル作成
            File.WriteAllText(gitLFSPath,configText);
            
        }
        
        private void Create3DModelGitLFSConfig()
        {
            CreateGitLFSConfig(_currentGitSetupConfig.GetModelGitLFS,
                _currentGitSetupConfig.GetGitLFS3DModelDirPathList);
        }
        private void CreateImageGitLFSConfig()
        {
            CreateGitLFSConfig(_currentGitSetupConfig.GetImageGitLFS,
                _currentGitSetupConfig.GetGitLFSImageDirPathList);
        }
        private void CreateVideoGitLFSConfig()
        {
            CreateGitLFSConfig(_currentGitSetupConfig.GetVideoGitLFS,
                _currentGitSetupConfig.GetGitLFSVideoDirPathList);
        }
        private void CreateAudioGitLFSConfig()
        {
            CreateGitLFSConfig(_currentGitSetupConfig.GetAudioGitLFS,
                _currentGitSetupConfig.GetGitLFSAudioDirPathList);
        }
        
        private void CreateGitLFSConfig(string configText,List<string> saveDirPathList)
        {
            

            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);

            
            saveDirPathList.ForEach(saveDirPath =>
            {
                if (saveDirPath != "")
                {
                    string gitLFSPath = unityprojectDirPath + "/" +saveDirPath+ "/"+ ".gitattributes";

                    Debug.Log("GitLFS:それぞれフォルダ用の設定ファイル作成 "+gitLFSPath);
                    
                    //設定ファイル作成
                    File.WriteAllText(gitLFSPath, configText);
                }
            });

            
        }
        
        #endregion


        #region 終了系

        private void CompleteSetup()
        {
            EditorApplication.update -= OnEditorUpdate;
            
            _completedSetUpAction?.Invoke();//終了の通知
        }
        
        

        #endregion
    }
}