using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineCamera vcam;
    public CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    public void Shake(float amplitudeGain)
    {
        noise.AmplitudeGain = amplitudeGain;
        noise.FrequencyGain = 1f;
        StartCoroutine(EndShake());
    }
    IEnumerator EndShake()
    {
        yield return new WaitForSeconds(0.1f);

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}
