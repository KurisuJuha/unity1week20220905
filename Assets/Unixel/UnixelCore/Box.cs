using System;
using System.Collections;
using System.Collections.Generic;
using Unixel.Core.Vector;

namespace Unixel.Core.Physics
{
    public struct Box
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Pivot;

        public Box(Vector2 Pos, Vector2 Size, Vector2 Pivot)
        {
            this.Position = Pos;
            this.Size = Size;
            this.Pivot = Pivot;
        }

        /// <summary>
        /// 引数のboxと当たっていればtrueを返し、当たっていなければfalseを返す 
        /// </summary>
        public bool Detect(Box box)
        {
            Vector2 myCenter = Position + Pivot + new Vector2(Size.x / 2f, Size.y / 2f);
            Vector2 otherCenter = box.Position + box.Pivot + new Vector2(box.Size.x / 2f, box.Size.y / 2f);
            Vector2 distance = new Vector2(MathF.Abs(myCenter.x - otherCenter.x), MathF.Abs(myCenter.y - otherCenter.y));
            Vector2 size = new Vector2(Size.x + box.Size.x, Size.y + box.Size.y);

            return distance.x < size.x / 2f
                && distance.y < size.y / 2f;
        }
    }
}
