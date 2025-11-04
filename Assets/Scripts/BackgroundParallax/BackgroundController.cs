using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backGroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }
    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (ParallaxLayer backgroundLayer in backGroundLayers)
        {
            backgroundLayer.MoveBackground(distanceToMove);
            backgroundLayer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }
    private void InitializeLayers()
    {
        foreach (ParallaxLayer backgroundLayer in backGroundLayers)
            backgroundLayer.CalculateImageWidth();
    }
}
