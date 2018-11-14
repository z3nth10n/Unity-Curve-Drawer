using System.Collections.Generic;
using UnityEngine;

public static class F
{
    private static Dictionary<Color, Texture2D> dictionary = new Dictionary<Color, Texture2D>();

    /// <summary>
    /// To the texture.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="store">if set to <c>true</c> [store].</param>
    /// <returns></returns>
    public static Texture2D ToTexture(this Color color, int width = -1, int height = -1, bool store = false)
    {
        Texture2D texture = null;

        if (!dictionary.ContainsKey(color) || !store)
        {
            if (width == -1 || height == -1)
            {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, color);
            }
            else
            {
                texture = new Texture2D(width, height);

                for (int i = 0; i < width * height; ++i)
                    texture.SetPixel(i % width, i / width, color);
            }

            texture.Apply();

            if (store) dictionary.Add(color, texture);
        }

        if (store)
            return dictionary[color];
        else
            return texture;
    }

    /// <summary>
    /// Converts the range.
    /// </summary>
    /// <param name="originalStart">The original start.</param>
    /// <param name="originalEnd">The original end.</param>
    /// <param name="newStart">The new start.</param>
    /// <param name="newEnd">The new end.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static float ConvertRange(
        float originalStart, float originalEnd, // original range
        float newStart, float newEnd, // desired range
        float value) // value to convert
    {
        float scale = (newEnd - newStart) / (originalEnd - originalStart);
        return (newStart + ((value - originalStart) * scale));
    }

    /// <summary>
    /// Gets the bounded noise.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="meanHeight">Height of the mean.</param>
    /// <param name="amplitude">The amplitude.</param>
    /// <returns></returns>
    public static float GetBoundedNoise(float value, float meanHeight, float amplitude)
    {
        //if (value < 0)
        //    value = Mathf.Abs(value);

        return Mathf.Clamp01(ConvertRange(0, 1, -amplitude, amplitude, value) + (meanHeight + .5f));
    }
}