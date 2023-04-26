using System.Collections.Generic;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{

    public class SetupConfig : ScriptableObject
    {
        [Header("自動インストールするパッケージのID")]
        [SerializeField] private List<string> _targetPackageIDList = new List<string>();
        public List<string> GetTargetPackageIDList => _targetPackageIDList;
        
        
        [Header("作成するフォルダのパス")]
        [SerializeField] private List<string> _createDirectoryPathList = new List<string>();
        public List<string> GetCreateDirectoryPathList => _createDirectoryPathList;
        
        
        [Header("レイヤー")]
        [SerializeField] private List<string> _layerList = new List<string>();
        public List<string> GetLayerList => _layerList;
        
        
        [Header("Git")]
        [SerializeField] private string _gitignoreTxt ="";
        public string GetGitIgnore => _gitignoreTxt;
        
                
        [Header("GitLFS")]
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _baseGitLFS ="";
        public string GetBaseGitLFS => _baseGitLFS;
        
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _modelGitLFS ="";
        public string GetModelGitLFS => _modelGitLFS;
        
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _imageGitLFS ="";
        public string GetImageGitLFS => _imageGitLFS;
        
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _videoGitLFS ="";
        public string GetVideoGitLFS => _videoGitLFS;
        
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _audioGitLFS ="";
        public string GetAudioGitLFS => _audioGitLFS;
    }
}