using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : CinemachineImpulseSource
{
    public void Shake()
    {
        GenerateImpulseWithVelocity(new Vector3(1, 1, 1));
    }
}
