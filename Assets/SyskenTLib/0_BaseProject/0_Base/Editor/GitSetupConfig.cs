using System.Collections.Generic;
using UnityEngine;

namespace SyskenTLib.BaseProject.Base.Editor
{
    public class GitSetupConfig : ScriptableObject
    {
               
        [Header("Git")]
        [TextArea(minLines:10,maxLines:100)] [SerializeField] private string _gitignoreTxt ="";
        public string GetGitIgnore => _gitignoreTxt;
        
                
        [Header("GitLFS")]
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _baseGitLFS ="";
        public string GetBaseGitLFS => _baseGitLFS;
        
        
        [Space(30)]
        [SerializeField] private List<string> _gitLFS3DModelDirPathList = new List<string>();
        public List<string> GetGitLFS3DModelDirPathList => _gitLFS3DModelDirPathList;
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _modelGitLFS ="";
        public string GetModelGitLFS => _modelGitLFS;
        
        [Space(30)]
        [SerializeField] private List<string> _gitLFSImageDirPathList = new List<string>();
        public List<string> GetGitLFSImageDirPathList => _gitLFSImageDirPathList;
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _imageGitLFS ="";
        public string GetImageGitLFS => _imageGitLFS;
        
        [Space(30)]
        [SerializeField] private List<string> _gitLFSVideoDirPathList = new List<string>();
        public List<string> GetGitLFSVideoDirPathList => _gitLFSVideoDirPathList;
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _videoGitLFS ="";
        public string GetVideoGitLFS => _videoGitLFS;
        
        [Space(30)]
        [SerializeField] private List<string> _gitLFSAudioDirPathList = new List<string>();
        public List<string> GetGitLFSAudioDirPathList => _gitLFSAudioDirPathList;
        [TextArea(minLines:10,maxLines:100)][SerializeField] private string _audioGitLFS ="";
        public string GetAudioGitLFS => _audioGitLFS; 
    }
}