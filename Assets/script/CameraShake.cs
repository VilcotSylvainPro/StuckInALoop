using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float Intensity)
    {
        CinemachineBasicMultiChannelPerlin  camShake = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        camShake.m_AmplitudeGain = camShake.m_AmplitudeGain + Intensity;
    }

    public void ChangeCameraRotation(float value)
    {
        if (cam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y < 20)
        {
            cam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y += value;
        }
    }

    public void ResetCameraRotation()
    {
        cam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
