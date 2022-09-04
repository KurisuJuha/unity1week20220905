namespace Unixel.Core.Input 
{
    public class UnixelInput
    {
        /// <summary>
        /// ADや←→を-1から1の間の値
        /// </summary>
        public float Horizontal;
        /// <summary>
        /// ADや←→を-1から1の間の値
        /// </summary>
        public float Vertical;
        /// <summary>
        /// Zが押されているか
        /// </summary>
        public bool A;
        /// <summary>
        /// Xが押されているか
        /// </summary>
        public bool B;
        /// <summary>
        /// Zを押した瞬間か
        /// </summary>
        public bool A_Down;
        /// <summary>
        /// Xを押した瞬間か
        /// </summary>
        public bool B_Down;
        /// <summary>
        /// Zを離した瞬間か
        /// </summary>
        public bool A_Up;
        /// <summary>
        /// Xを押した瞬間か
        /// </summary>
        public bool B_Up;
    }
}
