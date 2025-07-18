using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCam;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundsLayers;

    private void Awake()
    {
        mainCam = Camera.main;
        cameraHalfWidth = mainCam.orthographicSize * mainCam.aspect;

        CalculateBackgroundWidht();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCam.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;

        lastCameraPositionX = currentCameraPositionX;

        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;
        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundsLayers)
        {
            layer.Move(distanceToMove);   
            layer.Loopbackgroud(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateBackgroundWidht()
    {
        foreach (ParallaxLayer layer in backgroundsLayers)
        {
            layer.CalculateImageWidht();
        }
    }
}
