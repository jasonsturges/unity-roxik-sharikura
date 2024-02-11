using System;
using UnityEngine;
using Random = System.Random;


public class MotionController : MonoBehaviour
{
    private MotionType _motionType = MotionType.Cube;
    private float _cutoff;
    private float _r;
    private float _r0;
    private float _rp;
    private float _rl;

    void Start()
    {
        Invoke("OnChangeMotion", UnityEngine.Random.Range(0.5f, 1.5f));
    }

    void OnChangeMotion()
    {
        ChangeMotion();
        Invoke("OnChangeMotion", UnityEngine.Random.Range(0.5f, 1.5f));
    }

    private void ChangeMotion(MotionType? motionType = null)
    {
        if (motionType == null)
        {
            Random random = new Random();
            motionType = (MotionType)Enum.GetValues(typeof(MotionType)).GetValue(random.Next(0, 7));
        }

        _cutoff = 0;

        switch (motionType)
        {
            case MotionType.Antigravity:
                Antigravity();
                break;
            case MotionType.Cube:
                Cube();
                break;
            case MotionType.Cylinder:
                Cylinder();
                break;
            case MotionType.Gravity:
                Gravity();
                break;
            case MotionType.Sphere:
                Sphere();
                break;
            case MotionType.Tube:
                Tube();
                break;
            case MotionType.Wave:
                Wave();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(motionType), motionType, null);
        }
    }

    void Antigravity()
    {
        _motionType = MotionType.Antigravity;
        var random = new Random();
        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            var m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = 0.5f;
            p.animate = false;
            p.direction = new Vector3 {x = (float)random.NextDouble() * 0.25f - 0.125f, y = (float)random.NextDouble() * 0.25f - 0.125f, z = (float)random.NextDouble() * 0.25f - 0.125f};
        }
    }

    void Cube()
    {
        _motionType = MotionType.Cube;
        var random = new Random();
        var a = random.NextDouble() * 0.05f + 0.022f;
        var n = 0;
        var l = 1;

        // TODO: Update to `Math.Cbrt()` when .NET Standard 2.1 supported
        while (true)
        {
            if (l * l * l > Roxik.Models.Count)
            {
                l--;
                break;
            }

            l++;
        }

        for (var i = 0; i < l; i++)
        {
            for (var j = 0; j < l; j++)
            {
                for (var k = 0; k < l; k++)
                {
                    var m = Roxik.Models[n++];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    p.speed = 0;
                    p.acceleration = (float)a;
                    p.animate = false;
                    p.destination = new Vector3 {x = i * 0.8f + -(l - 1) * 0.8f * 0.5f, y = j * 0.8f + -(l - 1) * 0.8f * 0.5f, z = k * 0.8f + -(l - 1) * 0.8f * 0.5f};
                }
            }
        }
    }

    void Cylinder()
    {
        _motionType = MotionType.Cylinder;
        var random = new Random();
        var n = 0.0f;
        var r = (float)Math.PI * 2 / Roxik.Models.Count;
        var d = r * Math.Floor(random.NextDouble() * 40 + 1);

        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            GameObject m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)random.NextDouble() * 0.05f + 0.022f;
            p.animate = false;
            p.destination = new Vector3();

            if (i < Roxik.Models.Count - 50)
            {
                p.destination.x = (float)Math.Cos(n) * 4;
                p.destination.y = i * 0.008f - (Roxik.Models.Count - 50) * 0.004f;
                p.destination.z = (float)Math.Sin(n) * 4;
            }
            else
            {
                p.destination.x = (float)random.NextDouble() * 14 - 7;
                p.destination.y = (float)random.NextDouble() * 14 - 7;
                p.destination.z = (float)random.NextDouble() * 14 - 7;
            }

            n = (float)(n + d);
        }
    }

    void Gravity()
    {
        _motionType = MotionType.Gravity;
        var random = new Random();

        foreach (var m in Roxik.Models)
        {
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.direction = new Vector3();
            p.speed = 0;
            p.acceleration = 0.5f;
            p.animate = false;
            p.direction.y = (float)random.NextDouble() * -0.2f;
        }
    }

    void Sphere()
    {
        _motionType = MotionType.Sphere;
        var random = new Random();
        var s = 0.0f;
        var c = 0.0f;
        var r = Math.PI * 2 / Roxik.Models.Count;

        var d = r * random.Next(1, 40);
        var d2 = random.Next(3, 8);

        foreach (var m in Roxik.Models)
        {
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)random.NextDouble() * 0.05f + 0.022f;
            p.animate = false;
            p.destination = new Vector3();

            var d1 = (float)(Math.Cos(s) * d2);

            if ((float)random.NextDouble() > 0.06f)
            {
                p.destination.x = (float)Math.Cos(c) * d1;
                p.destination.y = (float)Math.Sin(s) * d2;
                p.destination.z = (float)Math.Sin(c) * d1;
            }
            else
            {
                p.destination.x = (float)random.NextDouble() * 7 - 7;
                p.destination.z = (float)random.NextDouble() * 7 - 7;
                p.destination.y = (float)random.NextDouble() * 7 - 7;
            }

            s += (float)r;
            c += (float)d;
        }
    }

    void Tube()
    {
        _motionType = MotionType.Tube;
        var random = new Random();
        var a = random.NextDouble() * 0.05f + 0.022f;
        var v = random.NextDouble() * 0.025f + 0.02f;
        var dx = -v * Roxik.Models.Count * 0.44f;
        var d = random.NextDouble() + 1.2f;

        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            var m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)a;
            p.animate = false;

            if (random.NextDouble() > 0.05f)
            {
                p.destination = new Vector3 {x = (float)(i * v + dx), y = (float)(random.NextDouble() * d - d * 0.5f), z = (float)(random.NextDouble() * d - d * 0.5f)};
            }
            else
            {
                p.destination = new Vector3 {x = (float)(random.NextDouble() * 14 - 7), y = (float)(random.NextDouble() * 14 - 7), z = (float)(random.NextDouble() * 14 - 7)};
            }
        }
    }

    void Wave()
    {
        _motionType = MotionType.Wave;
        var random = new Random();
        var a = random.NextDouble() * 0.05f + 0.022f;
        var l = Math.Floor(Math.Sqrt(Roxik.Models.Count));
        var d = -(l - 1) * 0.55f * 0.5f;
        var t = random.NextDouble() * 0.3f + 0.05f;
        var s = random.NextDouble() + 1;
        var n = 0;
        GameObject m;
        _r = 0;
        _r0 = 0;
        _rl = (float)random.NextDouble() + 1;
        _rp = (float)random.NextDouble() * 0.3f + 0.1f;

        for (var i = 0; i < l; i++)
        {
            var ty = Math.Cos(_r) * s;
            _r += (float)t;

            for (var j = 0; j < l; j++)
            {
                n += 1;
                m = Roxik.Models[n - 1];
                MotionProperties p = m.GetComponent<MotionProperties>();
                p.speed = 0;
                p.acceleration = (float)a;
                p.animate = false;
                p.destination = new Vector3();
                p.direction = new Vector3();

                p.direction.x = p.direction.y = p.direction.z = 0;
                p.destination.x = (float)(i * 0.55f + d);
                p.destination.y = (float)ty;
                p.destination.z = (float)(j * 0.55f + d);
            }
        }

        while (n < Roxik.Models.Count)
        {
            m = Roxik.Models[n];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)a;
            p.animate = false;
            p.destination = new Vector3 {x = (float)random.NextDouble() * 14 - 7, y = (float)random.NextDouble() * 14 - 7, z = (float)random.NextDouble() * 14 - 7};

            n++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Time.deltaTime * 90;
        float maxp;

        switch (_motionType)
        {
            case MotionType.Cube:
            case MotionType.Cylinder:
            case MotionType.Sphere:
            case MotionType.Tube:
                for (int i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();

                    if (!p.animate)
                    {
                        if (p.speed < 0.8)
                        {
                            p.speed += p.acceleration * delta;
                        }

                        Vector3 modelPosition = m.transform.position;
                        float c0 = p.destination.x - modelPosition.x;
                        float c1 = p.destination.y - modelPosition.y;
                        float c2 = p.destination.z - modelPosition.z;

                        m.transform.position = new Vector3(
                            modelPosition.x + c0 * p.speed * delta,
                            modelPosition.y + c1 * p.speed * delta,
                            modelPosition.z + c2 * p.speed * delta
                        );

                        if (Math.Abs(c0) < 0.05 && Math.Abs(c1) < 0.05 && Math.Abs(c2) < 0.05)
                        {
                            p.animate = true;
                            m.transform.position = new Vector3(
                                p.destination.x,
                                p.destination.y,
                                p.destination.z
                            );
                        }
                    }
                }

                maxp = (float)Math.Floor(Roxik.Models.Count / 40.0f);
                _cutoff += maxp * delta;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;

            case MotionType.Antigravity:
                for (var i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    Vector3 modelPosition = m.transform.position;

                    m.transform.position = new Vector3 {x = modelPosition.x + p.direction.x * delta, y = modelPosition.y + p.direction.y * delta, z = modelPosition.z + p.direction.z * delta};
                }

                _cutoff += 30.0f * delta;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;

            case MotionType.Gravity:
                for (var i = 0; i < Roxik.Models.Count; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    var y = m.transform.position.y + p.direction.y * delta;
                    p.direction.y -= 0.06f * delta;

                    if (y < -9)
                    {
                        y = -9;
                        p.direction.y *= -p.acceleration;
                        p.acceleration *= 0.9f;
                    }

                    var position = m.transform.position;
                    position = new Vector3 {x = position.x, y = y, z = position.z};
                    m.transform.position = position;
                }

                break;

            case MotionType.Wave:
                var max = Math.Floor(Math.Sqrt(Roxik.Models.Count));
                var cc = 0;

                for (var i = 0; i < max; i++)
                {
                    var cos = Math.Cos(_r) * _rl;
                    _r += _rp * delta;
                    for (var j = 0; j < max; j++)
                    {
                        GameObject m = Roxik.Models[cc++];
                        MotionProperties p = m.GetComponent<MotionProperties>();
                        p.destination.y = (float)cos;
                    }
                }

                _r0 += 0.11f * delta;
                _r = _r0;

                for (var i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    if (p.speed < 0.5)
                    {
                        p.speed += p.acceleration * delta;
                    }

                    Vector3 modelPosition = m.transform.position;
                    m.transform.position = new Vector3
                    {
                        x = modelPosition.x + (p.destination.x - modelPosition.x) * p.speed * delta,
                        y = modelPosition.y + (p.destination.y - modelPosition.y) * p.speed * delta,
                        z = modelPosition.z + (p.destination.z - modelPosition.z) * p.speed * delta
                    };
                }

                maxp = (float)Math.Floor(Roxik.Models.Count / 40.0f);
                _cutoff += maxp * delta;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
