using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyskenTLib.BuildSceneUtilEditor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
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
        private UnityProjectSetupConfig _currentUnityProjectSetupConfig;

        private SetupStatus _currentSetupStatus = SetupStatus.Init;

        private int _currentPackageIndex = 0;
        private AddRequest _currentAddRequest = null;

        public void StartSetup()
        {
            EditorApplication.update += OnEditorUpdate;

            _currentBaseSetupConfig = SearchSetUpConfig();
            _currentGitSetupConfig = SearchGitSetUpConfig();
            _currentUnityProjectSetupConfig = SearchUnityProjectSetUpConfig();

            _currentSetupStatus = SetupStatus.Init;

            //
            //事前準備
            Debug.Log("ビルド設定のうち、プライベートのものを作成");
            CustomBuild.AutoCreatePrivateConfig();


            //
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
                    StartUnityProjectSetting();
                    break;
                }
                case SetupStatus.UnityProjectSetting:
                {
                    _currentSetupStatus = SetupStatus.AddLayer;
                    StartAddLayerSetting();
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


        private BaseSetupConfig SearchSetUpConfig()
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

        private GitSetupConfig SearchGitSetUpConfig()
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

        private UnityProjectSetupConfig SearchUnityProjectSetUpConfig()
        {
            UnityProjectSetupConfig nextSetupConfig = null;
            string[] guids = AssetDatabase.FindAssets("t:UnityProjectSetupConfig");
            guids.ToList().ForEach(nextGUID =>
            {
                string filePath = AssetDatabase.GUIDToAssetPath(nextGUID);
                nextSetupConfig = AssetDatabase.LoadAssetAtPath<UnityProjectSetupConfig>(filePath);

            });

            return nextSetupConfig;
        }

        private List<CustomBuildConfig> SearchCustomBuildConfigConfig()
        {
            List<CustomBuildConfig> customBuildConfigList = new List<CustomBuildConfig>();


            string[] guids = AssetDatabase.FindAssets("t:CustomBuildConfig");
            guids.ToList().ForEach(nextGUID =>
            {
                CustomBuildConfig nextConfig = null;
                string filePath = AssetDatabase.GUIDToAssetPath(nextGUID);
                nextConfig = AssetDatabase.LoadAssetAtPath<CustomBuildConfig>(filePath);

                customBuildConfigList.Add(nextConfig);

            });

            return customBuildConfigList;
        }

        private string ReadLayerSettingFile()
        {
            string rawText = "";

            string targetDirPath = Application.dataPath + "/../ProjectSettings";
            string targetFilePath = targetDirPath + "/" + "TagManager.asset";
            rawText = File.ReadAllText(targetFilePath);
            return rawText;
        }

        private void SaveLayerSettingFile(string rawText)
        {

            string targetDirPath = Application.dataPath + "/../ProjectSettings";
            string targetFilePath = targetDirPath + "/" + "TagManager.asset";

            File.WriteAllText(targetFilePath, rawText);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
            Debug.Log("次のパッケージ:" + nextPackageID);
            _currentAddRequest = Client.Add(nextPackageID);
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
                    StartNextProcess(); //次の処理
                }
            }
        }


        #endregion

        #region ディレクトリ作成

        private void StartCreateDirectory()
        {
            CreateAllDirectoryProcess();

            StartNextProcess(); //次の処理
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
            Debug.Log("ディレクトリ作成します。 " + dirPath);
            string saveDirPath = Path.GetDirectoryName(Application.dataPath);
            string createDirPath = saveDirPath + "/" + dirPath;
            string gitkeepPath = createDirPath + "/" + ".gitkeep";
            if (Directory.Exists(createDirPath) == false)
            {
                //サブディレクトリ作成
                Directory.CreateDirectory(createDirPath);
            }

            //GitKeep追加
            if (File.Exists(gitkeepPath) == false)
            {
                File.WriteAllText(gitkeepPath, "");
            }
        }


        #endregion

        #region Git

        private void StartGitSetting()
        {
            Debug.Log("Gitの設定開始");
            StartGitSettingProcess();
            StartNextProcess(); //次の処理
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
            File.WriteAllText(gitkeepPath, configText);

        }

        private void CreateBaseGitLFSConfig()
        {

            Debug.Log("GitLFSの.gitattributes作成。 ");
            string configText = _currentGitSetupConfig.GetBaseGitLFS;
            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);
            string gitLFSPath = unityprojectDirPath + "/" + ".gitattributes";

            //設定ファイル作成
            File.WriteAllText(gitLFSPath, configText);

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

        private void CreateGitLFSConfig(string configText, List<string> saveDirPathList)
        {


            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);


            saveDirPathList.ForEach(saveDirPath =>
            {
                if (saveDirPath != "")
                {
                    string gitLFSPath = unityprojectDirPath + "/" + saveDirPath + "/" + ".gitattributes";

                    Debug.Log("GitLFS:それぞれフォルダ用の設定ファイル作成 " + gitLFSPath);

                    //設定ファイル作成
                    File.WriteAllText(gitLFSPath, configText);
                }
            });


        }

        #endregion

        #region UnityProjectSetting

        private void StartUnityProjectSetting()
        {
            Debug.Log("UnityProjectSettingの設定開始");

            StartUnityProjectSettingProcess();

            StartNextProcess(); //次の処理
        }

        private void StartUnityProjectSettingProcess()
        {
            StartOverwriteAppIDProcess();

            StartOverwriteAppName();
            StartOverwriteCompanyName();

            StartPlatformCommonGeneral();


            StartOverwriteIOSSetting();
            StartOverwriteAndroidSetting();
        }

        private void StartOverwriteAppIDProcess()
        {
            string overwriteAppID = "";

            switch (_currentUnityProjectSetupConfig.GetOverwriteAppIDType)
            {
                case UnityProjectAppIDType.None:
                {
                    return;
                }

                case UnityProjectAppIDType.OverwriteBaseID:
                {
                    overwriteAppID = _currentUnityProjectSetupConfig.GetBaseAppID;
                    break;
                }

                case UnityProjectAppIDType.OverwriteAddRandomID:
                {
                    string dateTxt = DateTime.Now.ToString("yyyyMMddHHmmss");
                    overwriteAppID = _currentUnityProjectSetupConfig.GetBaseAppID + ".app" + dateTxt;
                    break;
                }
            }

            if (overwriteAppID == "")
            {
                return;
            }

            Debug.Log("アプリIDを上書きします" + overwriteAppID);

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, overwriteAppID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, overwriteAppID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, overwriteAppID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.tvOS, overwriteAppID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, overwriteAppID);


            //自動ビルド設定に書き込む
            Debug.Log("自動ビルド設定のアプリID、チームIDを上書きします" + overwriteAppID);
            List<CustomBuildConfig> customBuildConfigList = SearchCustomBuildConfigConfig();
            customBuildConfigList.ForEach(buildConfig =>
            {
                buildConfig.overwrittenAppID_ONIOS = overwriteAppID;
                buildConfig.overwrittenTeamID_ONIOS = _currentUnityProjectSetupConfig.GetIOSTeamID;
                buildConfig.overwrittenAppID_ONANDROID = overwriteAppID;

                EditorUtility.SetDirty(buildConfig);

            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private void StartOverwriteAppName()
        {
            string appName = _currentUnityProjectSetupConfig.GetAppName;
            if (appName != "")
            {
                Debug.Log("アプリ名を上書き:" + appName);
                PlayerSettings.productName = appName;
            }

        }

        private void StartOverwriteCompanyName()
        {
            string companyName = _currentUnityProjectSetupConfig.GetCompanyName;
            if (companyName != "")
            {
                Debug.Log("会社名を上書き:" + companyName);
                PlayerSettings.companyName = companyName;
            }
        }

        private void StartPlatformCommonGeneral()
        {
            string appversion = "1.0.0";
            Debug.Log("アプリバージョン  " + appversion);
            PlayerSettings.bundleVersion = appversion;
        }


        private void StartOverwriteIOSSetting()
        {
            Debug.Log("iOS:AutomaticSign上書き " + _currentUnityProjectSetupConfig.GetIOSTurnONAutomaticSign);
            PlayerSettings.iOS.appleEnableAutomaticSigning = _currentUnityProjectSetupConfig.GetIOSTurnONAutomaticSign;

            string teamID = _currentUnityProjectSetupConfig.GetIOSTeamID;
            if (teamID != "")
            {
                Debug.Log("iOS:TeamID上書き " + teamID);
                PlayerSettings.iOS.appleDeveloperTeamID = teamID;
            }

            Debug.Log("iOS:サポートOSバージョン上書き " + _currentUnityProjectSetupConfig.GetIOSSupportMinOSVersion);
            PlayerSettings.iOS.targetOSVersionString = _currentUnityProjectSetupConfig.GetIOSSupportMinOSVersion;


            string cameraUsage = _currentUnityProjectSetupConfig.GetIOSCamraUsage;
            if (cameraUsage != "")
            {
                Debug.Log("iOS:カメラ利用理由上書き " + cameraUsage);
                PlayerSettings.iOS.cameraUsageDescription = cameraUsage;
            }

            string microphoneUsage = _currentUnityProjectSetupConfig.GetIOSMicrophoneUsage;
            if (microphoneUsage != "")
            {
                Debug.Log("iOS:マイク利用理由上書き " + microphoneUsage);
                PlayerSettings.iOS.microphoneUsageDescription = microphoneUsage;
            }

            string locationUsage = _currentUnityProjectSetupConfig.GetIOSLocationUsage;
            if (locationUsage != "")
            {
                Debug.Log("iOS:現在地利用理由上書き " + locationUsage);
                PlayerSettings.iOS.locationUsageDescription = teamID;
            }



        }


        private void StartOverwriteAndroidSetting()
        {


            Debug.Log("Android:サポートOSバージョン上書き " + _currentUnityProjectSetupConfig.GetAndroidSupportMinOSVersion);
            PlayerSettings.Android.minSdkVersion = _currentUnityProjectSetupConfig.GetAndroidSupportMinOSVersion;

            ScriptingImplementation targetScriptingImplementation = ScriptingImplementation.IL2CPP;
            Debug.Log("Android:ScriptBackend  " + targetScriptingImplementation);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, targetScriptingImplementation);


            AndroidArchitecture targetArchitecture = AndroidArchitecture.ARM64;
            Debug.Log("Android:アーキテクチャ  " + targetArchitecture);
            PlayerSettings.Android.targetArchitectures = targetArchitecture;


        }

        #endregion

        #region UnityLayerSetting

        private void StartAddLayerSetting()
        {
            Debug.Log("UnityのLayerの設定開始");

            StartAddLayerProcess();

            StartNextProcess(); //次の処理
        }

        private void StartAddLayerProcess()
        {
            string layerSettingText = ReadLayerSettingFile();

            string targetLayerText = _currentBaseSetupConfig.GetDefaultLayerSetting;
            
            string replaceLayerText = CreateAddLayerSettingText(CountTargetText(targetLayerText,"-"));
            Debug.Log("AddLayer:"+replaceLayerText);


            string savedText = layerSettingText.Replace(targetLayerText, replaceLayerText);
            
            SaveLayerSettingFile(savedText);
        }

        private string CreateAddLayerSettingText(int lineTotalNum)
        {
            string rawText = "";
            List<string> addLayerList = _currentBaseSetupConfig.GetLayerList;

            for (int i = 0; i < lineTotalNum; i++)
            {
                if (i >= addLayerList.Count)
                {
                    rawText += "  - " + "\n";
                }
                else
                {
                    rawText += "  - " +addLayerList[i]+ "\n";
                }
            }

            return rawText;
        }
    
        private int CountTargetText(string targetText, string searchText) 
        {
            return targetText.Length - targetText.Replace(searchText, "").Length;
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