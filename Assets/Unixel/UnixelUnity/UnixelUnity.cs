using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unixel.Core;

namespace Unixel.Unity
{
    public class UnixelUnity : MonoBehaviour
    {
        public Mesh mesh;
        public Material material;
        public Texture2D texture;
        public static UnixelCore core;

        /// <summary>
        /// UnixelUnityを初期化します。
        /// </summary>
        public static void Init(int x, int y)
        {
            core = new UnixelCore(new Core.Vector.Vector2Int(x, y));
            GameObject gameObject = Instantiate((GameObject)Resources.Load("Unixel"));
            gameObject.name = "Unixel";
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            mesh = new Mesh();
            texture = new Texture2D(core.Display.size.x, core.Display.size.y);
            texture.filterMode = FilterMode.Point;
            material.color = Color.white;
            material.mainTexture = texture;
            StartCoroutine(MainLoop());
        }

        public void Update()
        {
            MeshGenerate();
            TextureGenerate();
            SetInput();
            Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
        }

        /// <summary>
        /// UnityのUpdateとは別でUnixelCoreのUpdateを60fpsで動かす
        /// </summary>
        public IEnumerator MainLoop()
        {
            core.Start();

            while (true)
            {
                yield return new WaitForSeconds(1 / 60f);

                core.Update();
            }
        }

        /// <summary>
        /// 画面のサイズに合わせてメッシュを生成する
        /// </summary>
        public void MeshGenerate()
        {
            float height = core.Display.size.y / (float)core.Display.size.x;
            float width = 1;

            float m = width / height > Camera.main.aspect ? Camera.main.orthographicSize * Camera.main.aspect : Camera.main.orthographicSize / height;

            width *= m;
            height *= m;

            mesh.Clear();
            mesh.SetVertices(new Vector3[]
            {
                new Vector3(-width,-height),
                new Vector3(-width,height),
                new Vector3(width,height),
                new Vector3(width,-height),
            });
            mesh.SetTriangles(new int[]
            {
                0,1,2,
                0,2,3,
            }, 0);
            mesh.SetUVs(0, new List<UnityEngine.Vector2>()
            {
                new UnityEngine.Vector2(0,0),
                new UnityEngine.Vector2(0,1),
                new UnityEngine.Vector2(1,1),
                new UnityEngine.Vector2(1,0),
            });
        }

        /// <summary>
        /// UnixelCoreのカラー配列を使用してテクスチャを生成する
        /// </summary>
        public void TextureGenerate()
        {
            var pixelData = texture.GetPixels32();
            for (int y = 0; y < core.Display.size.y; y++)
            {
                for (int x = 0; x < core.Display.size.x; x++)
                {
                    var c = core.Display.image[x, y];
                    texture.SetPixel(x, y, new Color32(c.R, c.G, c.B, c.A));
                }
            }
            texture.Apply();
        }

        /// <summary>
        /// UnixelCoreのInputに値をセットしておく
        /// </summary>
        public void SetInput()
        {
            var input = core.Input;
            input.Horizontal = Input.GetAxisRaw("Horizontal");
            input.Vertical = Input.GetAxisRaw("Vertical");
            input.A = Input.GetKey(KeyCode.Z);
            input.B = Input.GetKey(KeyCode.X);
            input.A_Down = Input.GetKeyDown(KeyCode.Z);
            input.B_Down = Input.GetKeyDown(KeyCode.X);
            input.A_Up = Input.GetKeyUp(KeyCode.Z);
            input.B_Up = Input.GetKeyUp(KeyCode.X);
        }

        /// <summary>
        /// Resources内の指定された名前のSpriteを取得し、Imageにして返します。
        /// </summary>
        public static Image LoadImage(string path)
        {
            Texture2D texture = Resources.Load<Sprite>(path).texture;
            Image image = new Image(new Core.Vector.Vector2Int(texture.width, texture.height));
            Color32[] cs = texture.GetPixels32();

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    var c = cs[y * texture.width + x];
                    image.SetPixelLow(new Core.Vector.Vector2Int(x, y), System.Drawing.Color.FromArgb(c.a, c.r, c.g, c.b));
                }
            }

            return image;
        }
    }
}
