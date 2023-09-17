using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEditor;
using UnityEngine;

namespace SyskenTLib.SimpleUIUtil
{
    /// <summary>
    /// Canvasのオーダーを上書きします
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [DefaultExecutionOrder((int)CommonDefaultExecutionOrder.ExecutionOrder.DefaultDelayOne)]
    public class AutoChangeUIOrder : MonoBehaviour
    {

        [SerializeField] private CommonUISortOrder.SortOrder _order=CommonUISortOrder.SortOrder.Default;


        void Start()
        {
            ChangeOrder();
        }

        private void ChangeOrder()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.sortingOrder = (int)_order;
        }

        
        #if UNITY_EDITOR
        public void ManualChangeOnEditor()
        {
            ChangeOrder();

            
            
            //
            //シーン上のオブジェクト階層取得
            //
            Canvas canvas = GetComponent<Canvas>();

            string objectPath = this.gameObject.name;
            Transform firstParent = this.gameObject.transform.parent;
            Transform nextParent= firstParent;
            
            int loopMaxCount = 10;
            for (int i=0;i<loopMaxCount;i++)
            {
                if (nextParent != null)
                {
                    objectPath = nextParent.gameObject.name + " > " + objectPath;
                    if (nextParent.gameObject.transform.parent != null)
                    {
                        nextParent = nextParent.gameObject.transform.parent;
                    }
                    else
                    {
                        nextParent = null;
                    }
                    
                }
                else
                {
                    break;
                }   
            }
            Debug.Log(""+objectPath+":変更後："+canvas.sortingOrder);
        }
        #endif
        
    }
    
    
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(AutoChangeUIOrder))]//拡張するクラスを指定
    public class AutoChangeUIOrderEditor : Editor {

      public override void OnInspectorGUI(){
        //元のInspector部分を表示
        base.OnInspectorGUI ();


        if (GUILayout.Button("Overwrite Now!"))
        {
            AutoChangeUIOrder thisScript = target as AutoChangeUIOrder;
            thisScript.ManualChangeOnEditor();
        }  


        if (GUILayout.Button("Overwrite All On Current Scene"))
        {
            Debug.Log("現在のシーンの設定をすべて変更します。");

            List<AutoChangeUIOrder> targetScriptList = GameObject.FindObjectsByType<AutoChangeUIOrder>(FindObjectsSortMode.None).ToList();
            targetScriptList.ForEach(targetScript =>
            {
                targetScript.ManualChangeOnEditor();
            });



        }
      }

    }
    #endif
    


}