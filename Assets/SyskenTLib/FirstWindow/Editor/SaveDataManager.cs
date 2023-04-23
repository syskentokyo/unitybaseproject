using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace SyskenTLib.FirstWindowsEditor
{
 public class SaveDataManager
 {
     private readonly string SAVE_KEY = "SyskenTLib.FirstWindowsEditor.SaveDataManager.Save1";

     private readonly string SAVE_DIR_PATH = "/../SyskenTLibSetting";
     private readonly string SAVE_FILE_NAME = "firstwindow_save.json";


     private readonly string SAVE_VALUE_NODATA = "nodata";
        
        private string GetCurrentDate()
        {
            DateTime nowTime = DateTime.Now;
            return nowTime.ToString("yyyyMMddHHmmss");
        }
        
        
        #region ユーザごとにの保存
        public bool IsExistUserConfig()
        {
            string savedTextValue = EditorUserSettings.GetConfigValue(SAVE_KEY);
            if (savedTextValue == null|| savedTextValue == SAVE_VALUE_NODATA)
            {
                //値が保存されてなかった場合
                return false;
            }
            else
            {
                //値が保存されていた場合
                return true;

            }
        }
        
        public void SaveUserConfig()
        {
            string saveValue = GetCurrentDate();
            
            EditorUserSettings.SetConfigValue(SAVE_KEY,saveValue);
        }

        public void RemoveUserConfig()
        {
            EditorUserSettings.SetConfigValue(SAVE_KEY,SAVE_VALUE_NODATA);
        }
        
        
        
        #endregion
        #region プロジェクトに対しての保存
        
        private string GetProjectCommonConfigDirPath()
        {
            return Application.dataPath + SAVE_DIR_PATH;
        }

        private string GetProjectCommonConfigFilePath()
        {
            return GetProjectCommonConfigDirPath()+"/" + SAVE_FILE_NAME;
        }
        
        
        
        public bool IsExistProjectCommonConfig()
        {
            if (File.Exists(GetProjectCommonConfigFilePath()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        public void SaveProjectCommonConfig()
        {
            string saveValue = GetCurrentDate();
            SaveDataJSON saveJSON = new SaveDataJSON();
            saveJSON.saveDateText = saveValue;
            
            string jsonText  = JsonUtility.ToJson (saveJSON);

            if (Directory.Exists(GetProjectCommonConfigDirPath())==false)
            {
                //ディレクトリが存在しなかったとき
                Directory.CreateDirectory(GetProjectCommonConfigDirPath());//ディレクトリ作成
            }
            
            //ファイル保存
            File.WriteAllText(GetProjectCommonConfigFilePath(), jsonText);


        }
        
        public void RemoveProjectCommon()
        {
            File.Delete(GetProjectCommonConfigFilePath());
        }
        
        #endregion
    }
}
