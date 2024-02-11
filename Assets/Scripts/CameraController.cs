using System;
using UnityEngine;
using Random = System.Random;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private float speed;
    private float offset;
    private float distance = 6.0f;
    private float targetDistance;
    private GameObject targetModel;
    private Vector3 targetPosition = new Vector3(0, 0, 0);
    private float targetSpeed;
    private float rotationAngle;
    private float rotationSpeed = 0.03f;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        ChangeCamera();
        Invoke(nameof(OnChangeCamera), UnityEngine.Random.Range(0.5f, 1.5f));
    }

    void OnChangeCamera()
    {
        ChangeCamera();
        Invoke(nameof(OnChangeCamera), UnityEngine.Random.Range(0.5f, 1.5f));
    }

    void ChangeCamera()
    {
        Random random = new Random();

        targetModel = Roxik.Models[random.Next(Roxik.Models.Count)];
        targetSpeed = 0.0f;
        speed = 0.0f;
        offset = (float)(random.NextDouble() * 8) - 4;
        rotationSpeed = (float)(random.NextDouble() * 0.06f) - 0.03f;
        distance = (float)(random.NextDouble() * 4) + 7;
    }

    void Update()
    {
        var delta = Time.deltaTime * 25;

        if (targetSpeed < 0.05f)
            targetSpeed += 0.005f * delta;

        if (speed < 0.5f)
            speed += 0.005f * delta;

        rotationAngle += rotationSpeed * delta;
        targetDistance += (distance - targetDistance) * 0.1f * delta;

        var mp = targetModel.transform.position;
        targetPosition.x += (mp.x - targetPosition.x) * targetSpeed * delta;
        targetPosition.y += (mp.y - targetPosition.y) * targetSpeed * delta;
        targetPosition.z += (mp.z - targetPosition.z) * targetSpeed * delta;

        var cp = _camera.transform.position;
        cp = new Vector3
        {
            x = (float)(cp.x + (Math.Cos(rotationAngle) * targetDistance + targetPosition.x - cp.x) * speed),
            y = cp.y + (targetPosition.y + offset - cp.y) * speed,
            z = (float)(cp.z + (Math.Sin(rotationAngle) * targetDistance + targetPosition.z - cp.z) * speed)
        };

        Transform cameraTransform = _camera.transform;
        cameraTransform.position = cp;
        cameraTransform.LookAt(targetPosition);
    }
}
