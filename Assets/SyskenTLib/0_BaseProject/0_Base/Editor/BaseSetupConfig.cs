using System.Collections.Generic;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{

    public class BaseSetupConfig : ScriptableObject
    {
        [Header("自動インストールするパッケージのID")]
        [SerializeField] private List<string> _targetPackageIDList = new List<string>();
        public List<string> GetTargetPackageIDList => _targetPackageIDList;
        
        
        [Header("作成するフォルダのパス")]
        [SerializeField] private List<string> _createDirectoryPathList = new List<string>();
        public List<string> GetCreateDirectoryPathList => _createDirectoryPathList;


        [Header("レイヤー")] [TextArea(minLines:10,maxLines:100)] [SerializeField] private string _defaultLayerSetting = "";
        public string GetDefaultLayerSetting => _defaultLayerSetting;
    }
}