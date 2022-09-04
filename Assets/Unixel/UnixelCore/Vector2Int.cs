using System;
using System.Collections;
using System.Collections.Generic;

namespace Unixel.Core.Vector
{
    [Serializable]
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        public static Vector2Int operator+ (Vector2Int z, Vector2Int w)
        {
            return new Vector2Int(z.x + w.x, z.y + w.y);
        }

        public static Vector2Int operator- (Vector2Int z, Vector2Int w)
        {
            return new Vector2Int(z.x - w.x, z.y - w.y);
        }
    }
}