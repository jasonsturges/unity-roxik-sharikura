using System.Collections.Generic;
using UnityEngine;

public class Roxik : MonoBehaviour
{
    public static readonly List<GameObject> Models = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        const float bet = 0.7f;
        const float offset = (8 - 1) * bet * 0.5f;

        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                for (var k = 0; k < 8; k++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = new Vector3
                    {
                        x = i * bet - offset,
                        y = j * bet - offset,
                        z = k * bet - offset
                    };
                    sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    sphere.AddComponent<MotionProperties>();

                    Models.Add(sphere);
                }
            }
        }
    }
}
