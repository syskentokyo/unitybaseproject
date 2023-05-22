namespace Common
{
    public class CommonLayer
    {
        public enum Layer:int
        {
            //Unity標準
            Default=0,
            TransparentFX=1,
            IgnoreRaycast=2,
            Water=4,
            UI=5,
            
            //オリジナル
            Player=10,
            PlayerItem=11,
            
            Enemy=15,
            EnemyItem=16,
            
            FreeItem=22,
            BoundArea=23,
            
            
            
            ResultRender=27,
            
        }
    }
}