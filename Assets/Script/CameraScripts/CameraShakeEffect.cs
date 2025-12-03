using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin channelPerlin;

    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimerTotal;

    [SerializeField] private float intensity;
    [SerializeField] private float time;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        ShakeNoLerp();
        //ShakeWithLerp();
    }

    public void ShakeEffect()
    {
        channelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void ShakeNoLerp()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.unscaledDeltaTime;

            if (shakeTimer < 0)
            {
                channelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    private void ShakeWithLerp()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.unscaledDeltaTime;

            if (shakeTimer < 0)
            {
                channelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTimerTotal));

            }
        }
    }

    private void OnEnable()
    {
        EventManager.OnCameraShake += ShakeEffect;
    }

    private void OnDisable()
    {
        EventManager.OnCameraShake -= ShakeEffect;
    }
}
