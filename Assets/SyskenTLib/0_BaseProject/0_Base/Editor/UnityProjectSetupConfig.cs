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
        
        [Header("上書きするiOSのチームID")]
        [SerializeField] private string _iOSteamID = "";
        public string GetIOSTeamID => _iOSteamID;

    }
}