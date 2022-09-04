using System;
using Unixel.Core.Input;
using Unixel.Core.Vector;

namespace Unixel.Core
{
    [Serializable]
    public class UnixelCore
    {
        public Image Display { get; private set; }

        public Action Start;
        public Action Update;

        public UnixelInput Input;

        public UnixelCore(Vector2Int size)
        {
            Display = new Image(size);

            Input = new UnixelInput();

            Display.Clear();
        }
    }
}
