using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Cinemachine;

public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    [Range(0f, 1f)]
    public float range;
    public AnimationCurve ease;

    [Header("Targets")]
    public GameObject InventoryObj;
    public GameObject TitleObj;
    public GameObject PressAnyKeyObj;
    public PixelPerfectCamera CameraObj;
    public SpriteRenderer CursorObj;
    public CinemachineVirtualCamera VCameraObj;

    [Header("Param")]
    public Vector2 InventoryStartPos;
    public Vector2 InventoryEndPos;
    public Vector2 TitleStartPos;
    public Vector2 TitleEndPos;
    public Vector2 PressAnyKeyStartPos;
    public Vector2 PressAnyKeyEndPos;
    public int CameraStartPPU;
    public int CameraEndPPU;
    public float CursorStartAlpha;
    public float CursorEndAlpha;
    public Vector2 VCameraStartPos;
    public Vector2 VCameraEndPos;

    private CinemachineFramingTransposer transposer;

    void Start()
    {
        transposer = VCameraObj.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        float per = ease.Evaluate(range);
        InventoryObj.transform.localPosition = Vector2.Lerp(InventoryStartPos, InventoryEndPos, per);
        TitleObj.transform.localPosition = Vector2.Lerp(TitleStartPos, TitleEndPos, per);
        PressAnyKeyObj.transform.localPosition = Vector2.Lerp(PressAnyKeyStartPos, PressAnyKeyEndPos, per);
        CameraObj.assetsPPU = Mathf.FloorToInt(Mathf.Lerp(CameraStartPPU, CameraEndPPU, per));
        Color c = CursorObj.color;
        CursorObj.color = new Color(c.r, c.g, c.b, Mathf.Lerp(CursorStartAlpha, CursorEndAlpha, per));
        transposer.m_TrackedObjectOffset = Vector2.Lerp(VCameraStartPos, VCameraEndPos, per);
        if (per != 1)
        {
            transposer.m_SoftZoneHeight = 0;
            transposer.m_SoftZoneWidth = 0;
        }
        else
        {
            transposer.m_SoftZoneHeight = 0.8f;
            transposer.m_SoftZoneWidth = 0.8f;
        }
    }
}