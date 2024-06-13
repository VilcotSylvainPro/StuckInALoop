using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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



    // Update is called once per frame
    void Update()
    {
        
    }
}
