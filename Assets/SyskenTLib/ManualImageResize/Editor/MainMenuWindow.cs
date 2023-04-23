using System.Collections.Generic;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SyskenTLib.ManualImageResize.Editor
{
    public class MainMenuWindow : EditorWindow
    {
        private EditorCoroutine _coroutine;

        void OnGUI()
        {
            GUILayout.Label("ディレクトリを選択するとリサイズを開始します。", EditorStyles.boldLabel);
 
        }
        
        [MenuItem("SyskenTLib/ManualImage/Resize - Max256", priority = 30)]
        private static void ResizeMax256()
        {
            MainResizeOnWindow(256);

        }

        [MenuItem("SyskenTLib/ManualImage/Resize - Max512", priority = 30)]
        private static void ResizeMax512()
        {
            MainResizeOnWindow(512);

        }

        [MenuItem("SyskenTLib/ManualImage/Resize - Max1024", priority = 30)]
        private static void ResizeMax1024()
        {
            MainResizeOnWindow(1024);

        }

        [MenuItem("SyskenTLib/ManualImage/Resize - Max2048", priority = 30)]
        private static void ResizeMax2048()
        {
            MainResizeOnWindow(2048);

        }

        [MenuItem("SyskenTLib/ManualImage/Resize - Max4096", priority = 30)]
        private static void ResizeMax4096()
        {
            MainResizeOnWindow(4096);

        }


        private static void MainResizeOnWindow(float maxSize)
        {
            GetWindow<MainMenuWindow>().MainResize(maxSize);
        }

        public void MainResize(float maxSize)
        {
          _coroutine=  EditorCoroutineUtility.StartCoroutineOwnerless(MainResizeProgress(maxSize));
        }

        private IEnumerator MainResizeProgress(float maxSize)
        {
            yield return null;
            yield return null;

            //フォルダ選択
            var selectDirpath = EditorUtility.OpenFolderPanel("Target Root Directory Max " + maxSize,
                Application.dataPath, string.Empty);
            if (string.IsNullOrEmpty(selectDirpath))
            {
                GetWindow<MainMenuWindow>().Close();
                yield break;
            }
                

            Debug.Log ("＝＝＝＝＝＝＝＝＝＝画像リサイズ開始＝＝＝＝＝＝＝＝＝＝");

            //画像ファイルのパス抽出
            List<string> fileAllPathList = Directory.GetFiles(selectDirpath, "*.*", SearchOption.AllDirectories)
                .ToList()
                .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".tga") || s.EndsWith(".exr"))
                .ToList();
            
            for(int i=0;i<fileAllPathList.Count;i++)
            {
                string imageFilePath = fileAllPathList[i];
                                
                EditorUtility.DisplayProgressBar(
                    "画像リサイズ中",
                    string.Format("リサイズ No : "+i),
                    (float)i / (float)fileAllPathList.Count);
                
                
                byte[] newTexutreBytes = File.ReadAllBytes(imageFilePath);
                Texture2D srcTexture = new Texture2D(10, 10);
                srcTexture.LoadImage(newTexutreBytes);


                int srcSizeWidth = srcTexture.width;
                int srcSizeHeight = srcTexture.height;
                int targetSizeWidth = srcTexture.width;
                int targetSizeHeight = srcTexture.height;


                if (targetSizeWidth < maxSize && targetSizeHeight < maxSize)
                {
                    //小さいサイズだった場合

                    //とくになにもしない

                }
                else if (targetSizeWidth < targetSizeHeight)
                {
                    // 大きな画像
                    //縦長の画像だった場合
                    targetSizeHeight = (int)maxSize;
                    targetSizeWidth = (int)(srcSizeWidth * (maxSize / srcSizeHeight));

                    //リサイズと保存
                    ResizeAndSaveToFile(srcTexture
                        , imageFilePath
                        , srcSizeWidth
                        , srcSizeHeight
                        , targetSizeWidth
                        , targetSizeHeight);
                }
                else
                {
                    // 大きな画像
                    //横長の画像だった場合
                    targetSizeHeight = (int)(srcSizeHeight * (maxSize / srcSizeWidth));
                    targetSizeWidth = (int)(maxSize);

                    //リサイズと保存
                    ResizeAndSaveToFile(srcTexture
                        , imageFilePath
                        , srcSizeWidth
                        , srcSizeHeight
                        , targetSizeWidth
                        , targetSizeHeight);

                }
                yield return null;
                yield return null;
                yield return null;

            };

            EditorUtility.ClearProgressBar();
            GetWindow<MainMenuWindow>().Close();
            
            Debug.Log ("<color=green>画像リサイズ終了！！！</color>");

            
            yield return null;

        }
    


    private static Texture2D ResizeTexture(Texture2D srcTexture, int newWidth, int newHeight)
        {

            Texture2D newTexture = new Texture2D(newWidth,newHeight);
            
            //GPU側の処理
            Graphics.ConvertTexture(srcTexture, newTexture);
            
            //GPUからCPUへコピー
            newTexture.ReadPixels(new Rect(Vector2.zero,new Vector2(newTexture.width,newTexture.height)),0,0);
            return newTexture;
        }

        private static void ResizeAndSaveToFile(Texture2D srcTexture,string filePath,int srcWidth, int srcHeight, int newWidth, int newHeight)
        {

            Texture2D newTexture = ResizeTexture(srcTexture, newWidth, newHeight);

            if (Path.GetExtension(filePath) == ".jpg")
            {
               byte[] saveImageBytes = newTexture.EncodeToJPG(100);
               File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".png")
            {
                byte[] saveImageBytes = newTexture.EncodeToPNG();
                File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".tga")
            {
                byte[] saveImageBytes = newTexture.EncodeToTGA();
                File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".exr")
            {
                byte[] saveImageBytes = newTexture.EncodeToEXR();
                File.WriteAllBytes(filePath,saveImageBytes);
            }

            

        }

    }
}