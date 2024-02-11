using System;
using UnityEngine;
using Random = System.Random;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _target = new Vector3(0, 0, 0);
    private GameObject _targetModel;
    private float bl = 6.0f;
    private float rp = 0.03f;
    private float cs;
    private float gy;
    private float l;
    private float ts;
    private float r;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        ChangeCamera();
        Invoke("OnChangeCamera", UnityEngine.Random.Range(0.5f, 1.5f));
    }

    void OnChangeCamera()
    {
        ChangeCamera();
        Invoke("OnChangeCamera", UnityEngine.Random.Range(0.5f, 1.5f));
    }

    void ChangeCamera()
    {
        Random random = new Random();

        _targetModel = Roxik.Models[random.Next(Roxik.Models.Count)];
        ts = 0.0f;
        cs = 0.0f;
        gy = (float)(random.NextDouble() * 8) - 4;
        rp = (float)(random.NextDouble() * 0.06f) - 0.03f;
        bl = (float)(random.NextDouble() * 4) + 7;
    }

    void Update()
    {
        var delta = Time.deltaTime * 25;

        if (ts < 0.05f)
            ts += 0.005f * delta;

        if (cs < 0.5f)
            cs += 0.005f * delta;

        r += rp * delta;
        l += (bl - l) * 0.1f * delta;

        var targetPosition = _targetModel.transform.position;
        _target.x += (targetPosition.x - _target.x) * ts * delta;
        _target.y += (targetPosition.y - _target.y) * ts * delta;
        _target.z += (targetPosition.z - _target.z) * ts * delta;

        var cameraPosition = _camera.transform.position;
        cameraPosition = new Vector3
        {
            x = (float)(cameraPosition.x + (Math.Cos(r) * l + targetPosition.x - cameraPosition.x) * cs),
            y = cameraPosition.y + (targetPosition.y + gy - cameraPosition.y) * cs,
            z = (float)(cameraPosition.z + (Math.Sin(r) * l + targetPosition.z - cameraPosition.z) * cs)
        };

        Transform cameraTransform = _camera.transform;
        cameraTransform.LookAt(_target);
        cameraTransform.position = cameraPosition;
    }
}
