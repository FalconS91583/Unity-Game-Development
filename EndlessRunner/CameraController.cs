using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedUpPR;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 120f;
    [SerializeField] private float zoomDuration = 0.6f;
    [SerializeField] private float zoomSpeedModifier = 5f;

    private CinemachineCamera cinema;

    private void Awake()
    {
        cinema = GetComponent<CinemachineCamera>();
    }
    public void ChangeCameraFOV(float speedAmout)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOV(speedAmout));

        if(speedAmout > 0)
        {
            speedUpPR.Play();
        }
    }

    private IEnumerator ChangeFOV(float speedAmount)
    {
        float startFOV = cinema.Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedAmount  * zoomSpeedModifier, minFOV, maxFOV);

        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            elapsedTime += Time.deltaTime;
            cinema.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        cinema.Lens.FieldOfView = targetFOV;
    }
}
