using Unixel.Unity;
using Unixel.Core;
using Unixel.Core.Input;

public class Main
{
    public UnixelCore Core;
    public UnixelInput Input;

    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        var s = new Main();

        UnixelUnity.Init(256, 256);
        s.Core = UnixelUnity.core;
        s.Input = s.Core.Input;
        UnixelUnity.core.Start += s.Start;
        UnixelUnity.core.Update += s.Update;
    }

    public void Start()
    {

    }

    public void Update()
    {

    }
}
