using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Roxik : MonoBehaviour
{
    public static readonly List<GameObject> Models = new List<GameObject>();
    private static readonly int Metallic = Shader.PropertyToID("_Metallic");

    // Start is called before the first frame update
    private void Awake()
    {
        const int length = 8;
        const float bet = 0.8f;
        const float offset = (length - 1) * bet * 0.5f;
        Random random = new Random();

        Color[] colors =
        {
            new Color(0.592f, 0.207f, 0.043f), 
            new Color(0.149f, 0.431f, 0.647f), 
            new Color(0.000f, 0.517f, 0.498f), 
            new Color(0.184f, 0.505f, 0.556f),
            new Color(0.031f, 0.568f, 0.486f),
            new Color(0.419f, 0.270f, 0.549f),
            new Color(0.478f, 0.270f, 0.149f)
        };

        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < length; j++)
            {
                for (var k = 0; k < length; k++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = new Vector3
                    {
                        x = i * bet - offset,
                        y = j * bet - offset,
                        z = k * bet - offset
                    };
                    sphere.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                    sphere.AddComponent<MotionProperties>();
                    var sphereRenderer = sphere.GetComponent<Renderer>();
                    sphereRenderer.material.color = colors[random.Next(colors.Length)];
                    sphereRenderer.material.SetFloat(Metallic, 0.3f);

                    Models.Add(sphere);
                }
            }
        }
    }
}
