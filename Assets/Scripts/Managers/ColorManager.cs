using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ColorManager : Singleton<ColorManager>
{

    public List<Color> palette = new List<Color>();
    public  List<Material> materials = new List<Material>();

    public Color GetColor(int idx)
    {
        int n = idx % materials.Count;
        return palette[n];
    }

    public Material GetMaterial(int idx)
    {
        int n = idx % materials.Count;
        return materials[n];
    }

    public void Init()
    {
        TextAsset sourcefile = Resources.Load<TextAsset>("colors");
        StringReader stream = new StringReader(sourcefile.text);
        string line;
        bool endOfFile = false;


        while (!endOfFile)
        {
            line = stream.ReadLine();
            //Debug.Log(line);

            if (line == null)
            {
                endOfFile = true;
                break;
            }

            Color color;
            ColorUtility.TryParseHtmlString('#'+line, out color);
            color.a = 0.7f;
            palette.Add(color);

        }

        Shader shader = Resources.Load<Shader>("BalloonMat/balloon_shader");


        for (int i = 0; i < palette.Count; i++)
        {
           Material material = new Material(shader);
            material.name = $"Material {i}";
           material.color = palette[i];
           materials.Add(material);
        }
    }

}
