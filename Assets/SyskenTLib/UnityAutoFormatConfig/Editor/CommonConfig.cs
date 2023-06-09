using System.Collections.Generic;

namespace SyskenTLib.UnityAutoFormatConfig.Editor
{
    public static class CommonConfig
    {
        //
        // テクスチャ
        //
        public static readonly List<string> textureNormalUIDirectoryPathList = new List<string>()
        {
            "Assets/Main/Textures/UI",
            "Assets/Client/Textures/UI",
        };

        public  static readonly List<string> textureDotUIDirectoryPathList = new List<string>()
        {
            "Assets/Main/Textures/DotUI",
             "Assets/Client/Textures/DotUI",
        };
        
        //アプリアイコン
        public  static readonly List<string> textureCustom1DirectoryPathList = new List<string>()
        {
            "Assets/Main/Textures/AppIcon",
            "Assets/Client/Textures/AppIcon",
        };
        public  static readonly List<string> textureCustom2DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Texture/Custom2"
        };
        public  static readonly List<string> textureCustom3DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Texture/Custom3"
        };
        
        
        //
        // Audio
        //
        public static readonly List<string> audioBGMDirectoryPathList = new List<string>()
        {
            "Assets/Main/Audio/BGM",
            "Assets/Client/Audio/BGM",
        };

        public  static readonly List<string> audioSEDirectoryPathList = new List<string>()
        {
            "Assets/Main/Audio/SE",
            "Assets/Client/Audio/SE",
        };
        
        public  static readonly List<string> audioCustom1DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Sound/Custom1"
        };
        
        public  static readonly List<string> audioCustom2DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Sound/Custom2"
        };
        
        public  static readonly List<string> audioCustom3DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Sound/Custom3"
        };
        
        //
        // Video
        //
        public static readonly List<string> videoNormalDirectoryPathList = new List<string>()
        {
            "Assets/Main/Video",
            "Assets/Client/Video",
        };
        
        public static readonly List<string> videoCustom1DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Video/Custom1"
        };
        
        public static readonly List<string> videoCustom2DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Video/Custom2"
        };
        
        public static readonly List<string> videoCustom3DirectoryPathList = new List<string>()
        {
            "Assets/Sample/Video/Custom3"
        };


    }
}