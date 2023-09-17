namespace Common
{
    public class CommonDefaultExecutionOrder
    {
        /// <summary>
        /// スクリプトの実行順
        /// </summary>
        public enum ExecutionOrder : int
        {
            
            //
            // 単独で動くスクリプトの初期化
            //
            FirstOne=-100,
            
            //
            // Unity標準
            //
            Default=0,
            
            /// <summary>
            /// 標準より１つ遅く処理
            /// </summary>
            DefaultDelayOne = 1,
            
            /// <summary>
            /// 通常の管理クラス
            /// </summary>
            DefaultManager=20,
            
            //
            // シーン管理
            //
            SceneManager=500,
        }
    }
}