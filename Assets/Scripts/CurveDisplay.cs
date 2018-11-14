using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveDisplay : MonoBehaviour
{
    public int width = 300;
    public int height = 50;
    public int seed = 12;

    public float amplitude = .2f,
                 meanHeight = -.25f;

    private Texture2D texture;
    private FastNoise noise;

    private float lastAmplitude, lastMeanHeight;

    // Start is called before the first frame update
    private void Start()
    {
        noise = new FastNoise();
        noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);

        noise.SetSeed(seed);
        noise.SetFrequency(.01f);
        noise.SetFractalOctaves(3);
        noise.SetFractalLacunarity(1.25f);
        noise.SetFractalGain(.5f);

        ClearTexture();

        UpdateMeanLine();

        UpdateNoise();
    }

    private void ClearTexture()
    {
        texture = new Color(0, 0, 0, 0).ToTexture(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float y = (j / 2f) * height;

                texture.SetPixel(i, (int)Mathf.Clamp(y, 10, height - 1), Color.gray);
            }
        }

        texture.Apply();
    }

    private void UpdateMeanLine()
    {
        ClearTexture();

        for (int m = 0; m < width; m++)
        {
            texture.SetPixel(m, height / 2, Color.yellow);
        }

        texture.Apply();
    }

    private void UpdateNoise(bool clear = false)
    {
        if (clear)
            UpdateMeanLine();

        for (int k = 0; k < width; k++)
        {
            float v = F.ConvertRange(-1, 1, 0, 1, noise.GetValueFractal(k, 0));

            v = F.GetBoundedNoise(v, meanHeight, amplitude);

            texture.SetPixel(k, (int)(v * (height - 1)), Color.green);
        }

        texture.Apply();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (texture != null)
            GUI.DrawTexture(new Rect(5, 5, width, height), texture);

        GUILayout.BeginArea(new Rect(5, height + 10, width, 200));

        GUILayout.Label($"Mean Height ({meanHeight.ToString("F2")})");

        meanHeight = GUILayout.HorizontalSlider(meanHeight, -.5f, .5f);

        GUILayout.Label($"Amplitude ({amplitude.ToString("F2")})");

        amplitude = GUILayout.HorizontalSlider(amplitude, 0, 1);

        GUILayout.EndArea();

        if (meanHeight != lastMeanHeight)
        {
            UpdateMeanLine();
            UpdateNoise();
        }

        if (amplitude != lastAmplitude)
            UpdateNoise(true);

        lastMeanHeight = meanHeight;
        lastAmplitude = amplitude;
    }
}