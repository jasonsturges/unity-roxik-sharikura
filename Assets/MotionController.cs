using System;
using UnityEngine;
using Random = System.Random;


public class MotionController : MonoBehaviour
{
    private MotionType _motionType = MotionType.Cube;
    private int _frame;
    private int _sceneLimit = 100;
    private float _cutoff;
    private float _r;
    private float _r0;
    private float _rp;
    private float _rl;

    private void ChangeMotion(MotionType motionType, int limit = -1)
    {
        Random random = new Random();
        _motionType = motionType;
        _cutoff = 0;
        _frame = 0;

        _sceneLimit = limit < 0 ? random.Next(3, 143) : limit;

        switch (motionType)
        {
            case MotionType.Cylinder:
                Cylinder();
                break;
            case MotionType.Sphere:
                Sphere();
                break;
            case MotionType.Cube:
                Cube();
                break;
            case MotionType.Tube:
                Tube();
                break;
            case MotionType.Wave:
                Wave();
                break;
            case MotionType.Gravity:
                Gravity();
                break;
            case MotionType.Antigravity:
                Antigravity();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(motionType), motionType, null);
        }
    }

    void Cylinder()
    {
        var random = new Random();
        var n = 0.0f;
        _r = (float)Math.PI * 2 / Roxik.Models.Count;
        var d = _r * Math.Floor(random.NextDouble() * 40 + 1);

        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            GameObject m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)random.NextDouble() * 0.05f + 0.022f;
            p.animate = false;
            p.dest = new Vector3();

            if (i < Roxik.Models.Count - 50)
            {
                p.dest.x = (float)Math.Cos(n) * 4;
                p.dest.y = i * 0.008f - (Roxik.Models.Count - 50) * 0.004f;
                p.dest.z = (float)Math.Sin(n) * 4;
            }
            else
            {
                p.dest.x = (float)random.NextDouble() * 14 - 7;
                p.dest.y = (float)random.NextDouble() * 14 - 7;
                p.dest.z = (float)random.NextDouble() * 14 - 7;
            }

            n = (float)(n + d);
        }
    }


    void Sphere()
    {
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
            p.dest = new Vector3();

            var d1 = (float)(Math.Cos(s) * d2);

            if ((float)random.NextDouble() > 0.06f)
            {
                p.dest.x = (float)Math.Cos(c) * d1;
                p.dest.y = (float)Math.Sin(s) * d2;
                p.dest.z = (float)Math.Sin(c) * d1;
            }
            else
            {
                p.dest.x = (float)random.NextDouble() * 7 - 7;
                p.dest.z = (float)random.NextDouble() * 7 - 7;
                p.dest.y = (float)random.NextDouble() * 7 - 7;
            }

            s += (float)r;
            c += (float)d;
        }
    }

    void Cube()
    {
        var random = new Random();
        var a = random.NextDouble() * 0.05f + 0.022f;
        var n = 0;
        var l = 1;

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
                    p.dest = new Vector3
                    {
                        x = i * 0.8f + -(l - 1) * 0.8f * 0.5f,
                        y = j * 0.8f + -(l - 1) * 0.8f * 0.5f,
                        z = k * 0.8f + -(l - 1) * 0.8f * 0.5f
                    };
                }
            }
        }
    }

    void Tube()
    {
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
                p.dest = new Vector3
                {
                    x = (float)(i * v + dx),
                    y = (float)(random.NextDouble() * d - d * 0.5f),
                    z = (float)(random.NextDouble() * d - d * 0.5f)
                };
            }
            else
            {
                p.dest = new Vector3
                {
                    x = (float)(random.NextDouble() * 14 - 7),
                    y = (float)(random.NextDouble() * 14 - 7),
                    z = (float)(random.NextDouble() * 14 - 7)
                };
            }
        }
    }

    void Wave()
    {
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
                p.dest = new Vector3();
                p.dir = new Vector3();

                p.dir.x = p.dir.y = p.dir.z = 0;
                p.dest.x = (float)(i * 0.55f + d);
                p.dest.y = (float)ty;
                p.dest.z = (float)(j * 0.55f + d);
            }
        }

        while (n < Roxik.Models.Count)
        {
            m = Roxik.Models[n];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = (float)a;
            p.animate = false;
            p.dest = new Vector3
            {
                x = (float)random.NextDouble() * 14 - 7,
                y = (float)random.NextDouble() * 14 - 7,
                z = (float)random.NextDouble() * 14 - 7
            };

            n++;
        }
    }

    void Gravity()
    {
        var random = new Random();
        _sceneLimit = 60;

        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            var m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.dir = new Vector3();
            p.speed = 0;
            p.acceleration = 0.5f;
            p.animate = false;
            p.dir.y = (float)random.NextDouble() * -0.2f;
        }
    }

    void Antigravity()
    {
        var random = new Random();
        for (var i = 0; i < Roxik.Models.Count; i++)
        {
            var m = Roxik.Models[i];
            MotionProperties p = m.GetComponent<MotionProperties>();
            p.speed = 0;
            p.acceleration = 0.5f;
            p.animate = false;
            p.dir = new Vector3
            {
                x = (float)random.NextDouble() * 0.25f - 0.125f,
                y = (float)random.NextDouble() * 0.25f - 0.125f,
                z = (float)random.NextDouble() * 0.25f - 0.125f
            };

        }
    }

    // Update is called once per frame
    void Update()
    {
        Random random = new Random();
        float maxp;

        switch (_motionType)
        {
            case MotionType.Cylinder:
            case MotionType.Sphere:
            case MotionType.Cube:
            case MotionType.Tube:
                for (int i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();

                    if (!p.animate)
                    {
                        if (p.speed < 0.8)
                        {
                            p.speed += p.acceleration;
                        }

                        Vector3 modelPosition = m.transform.position;
                        float c0 = p.dest.x - modelPosition.x;
                        float c1 = p.dest.y - modelPosition.y;
                        float c2 = p.dest.z - modelPosition.z;
                        
                        m.transform.position = new Vector3(
                            modelPosition.x + c0 * p.speed,
                            modelPosition.y + c1 * p.speed,
                            modelPosition.z + c2 * p.speed
                        );
                        if (Math.Abs(c0) < 0.05 && Math.Abs(c1) < 0.05 && Math.Abs(c2) < 0.05)
                        {
                            p.animate = true;
                            m.transform.position = new Vector3(
                                p.dest.x,
                                p.dest.y,
                                p.dest.z
                            );
                        }
                    }
                }

                maxp = (float)Math.Floor(Roxik.Models.Count / 40.0f);
                _cutoff += maxp;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;
            
            case MotionType.Wave:
                var max = Math.Floor(Math.Sqrt(Roxik.Models.Count));
                var cc = 0;

                for (var i = 0; i < max; i++)
                {
                    var cos = Math.Cos(_r) * _rl;
                    _r += _rp;
                    for (var j = 0; j < max; j++)
                    {
                        GameObject m = Roxik.Models[cc++];
                        MotionProperties p = m.GetComponent<MotionProperties>();
                        p.dest.y = (float)cos;
                    }
                }

                _r0 += 0.11f;
                _r = _r0;

                for (var i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    if (p.speed < 0.5)
                    {
                        p.speed += p.acceleration;
                    }

                    Vector3 modelPosition = m.transform.position;
                    m.transform.position = new Vector3 { 
                        x = modelPosition.x + (p.dest.x - modelPosition.x) * p.speed,
                        y = modelPosition.y + (p.dest.y - modelPosition.y) * p.speed,
                        z = modelPosition.z + (p.dest.z - modelPosition.z) * p.speed
                    };
                }

                maxp = (float)Math.Floor(Roxik.Models.Count / 40.0f);
                _cutoff += maxp;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;

            case MotionType.Gravity:
                for (var i = 0; i < Roxik.Models.Count; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    var y = m.transform.position.y + p.dir.y;
                    p.dir.y -= 0.06f;

                    if (y < -9)
                    {
                        y = -9;
                        p.dir.y *= -p.acceleration;
                        p.acceleration *= 0.9f;
                    }

                    m.transform.position = new Vector3
                    {
                        x = m.transform.position.x, 
                        y = y, 
                        z = m.transform.position.z
                    };
                }

                break;

            case MotionType.Antigravity:
                for (var i = 0; i < _cutoff; i++)
                {
                    GameObject m = Roxik.Models[i];
                    MotionProperties p = m.GetComponent<MotionProperties>();
                    Vector3 modelPosition = m.transform.position;

                    m.transform.position = new Vector3
                    {
                        x = modelPosition.x + p.dir.x,
                        y = modelPosition.y + p.dir.y,
                        z = modelPosition.z + p.dir.z
                    };
                }

                _cutoff += 30;
                if (_cutoff > Roxik.Models.Count)
                    _cutoff = Roxik.Models.Count;

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (++_frame > _sceneLimit)
            ChangeMotion((MotionType)Enum.GetValues(typeof(MotionType)).GetValue(random.Next(0, 7)));
    }
}
