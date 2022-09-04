using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using Unixel.Core.Vector;

namespace Unixel.Core
{
    public class Image
    {
        public Color[,] image;
        public Vector2Int size;

        // シェーダー
        public delegate Color FragmentShader(Vector2Int Pos, Vector2Int Size, Color[,] image);
        /// <summary>
        /// ピクセルごとに処理されるシェーダーです。
        /// </summary>
        public FragmentShader fragmentshader;

        public Image(Vector2Int size)
        {
            image = new Color[size.x, size.y];
            this.size = size;
            fragmentshader = (Vector2Int Pos, Vector2Int Size, Color[,] image) =>
            {
                return image[Pos.x, Pos.y];
            };
        }

        /// <summary>
        /// 指定された位置に指定された色を描きこみます。
        /// 範囲外の場合も描きこもうとするため安全ではありません。
        /// </summary>
        public void SetPixelLow(Vector2Int pos, Color color)
        {
            image[pos.x, pos.y] = color;
        }

        /// <summary>
        /// 指定された位置に指定された色を描きこみます。
        /// 範囲外の場合は無視します。
        /// </summary>
        public void SetPixel(Vector2Int pos, Color color)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y)
            {
                SetPixelLow(pos, color);
            }
        }

        /// <summary>
        /// imageを指定した色で塗りつぶします。
        /// </summary>
        /// <param name="color"></param>
        public void Clear(Color color)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    SetPixelLow(new Vector2Int(x, y), color);
                }
            }
        }

        /// <summary>
        /// imageを白で塗りつぶします。
        /// </summary>
        public void Clear() => Clear(Color.White);

        /// <summary>
        /// imageの中に指定された位置、指定されたimageを描きこみます。
        /// alphaが0の場合は描画しません。
        /// </summary>
        public void SetImage(Vector2Int pos, Image image)
        {
            Vector2Int size = image.size;

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Color c = image.fragmentshader(new Vector2Int(x, y), size, image.image);
                    if (c.A != 0) SetPixel(new Vector2Int(x, y) + pos, c);
                }
            }
        }
    }
}
