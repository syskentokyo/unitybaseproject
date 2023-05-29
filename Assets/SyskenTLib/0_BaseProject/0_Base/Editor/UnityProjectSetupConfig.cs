using UnityEditor;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{

    public enum UnityProjectAppIDType
    {
        None,
        OverwriteBaseID,
        OverwriteAddRandomID,
    }
    

    public class UnityProjectSetupConfig : ScriptableObject
    {
        
        
        [Header("アプリIDを上書きするか")]
        [SerializeField] private UnityProjectAppIDType _overwriteAppIDType = UnityProjectAppIDType.None;
        public UnityProjectAppIDType GetOverwriteAppIDType => _overwriteAppIDType;
        
        [Header("上書きするアプリID")]
        [SerializeField] private string _baseAppID = "";
        public string GetBaseAppID => _baseAppID;
        

        [Header("アプリ名")]
        [SerializeField] private string _appName = "";
        public string GetAppName => _appName;
        
        [Header("会社名")]
        [SerializeField] private string _companyName = "";
        public string GetCompanyName => _companyName;
        
        
        [Header("上書きするiOS系の設定")]
        [SerializeField] private string _iOSteamID = "";
        public string GetIOSTeamID => _iOSteamID;
        
        [SerializeField] private bool _iOSTurnONAutomaticSign = false;
        public bool GetIOSTurnONAutomaticSign => _iOSTurnONAutomaticSign;

        [SerializeField] private string _iOSSupportMinOSVersion = "16.0";
        public string GetIOSSupportMinOSVersion => _iOSSupportMinOSVersion;
        
        [SerializeField] private string _iOSCameraUsage = "";
        public string GetIOSCamraUsage => _iOSCameraUsage;
        
        [SerializeField] private string _iOSMicrophoneUsage = "";
        public string GetIOSMicrophoneUsage => _iOSMicrophoneUsage;
        
        [SerializeField] private string _iOSLocationUsage = "";
        public string GetIOSLocationUsage => _iOSLocationUsage;
        
        [Header("上書きするAndroid系の設定")]
        [SerializeField] private AndroidSdkVersions _androidSupportMinOSVersion = AndroidSdkVersions.AndroidApiLevel30;
        public AndroidSdkVersions GetAndroidSupportMinOSVersion => _androidSupportMinOSVersion;
        

        
        
    }
}