using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ImagesManager : MonoBehaviour
{
    public static string AvatarURL;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public static Sprite getSpriteFromTexture(Texture2D texture)
    {
        Sprite newSprite = null;
        newSprite = Sprite.Create(texture as Texture2D, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        return newSprite;
    }
    public static Sprite getSpriteFromBytes(byte[] bytes)
    {
        Texture2D texture = new Texture2D(1, 1);
        Sprite newSprite = null;
        texture.LoadImage(bytes);
        Texture2D roundTxt = ImagesManager.RoundCrop(texture);
        newSprite = Sprite.Create(roundTxt as Texture2D, new Rect(0f, 0f, roundTxt.width, roundTxt.height), Vector2.zero);
        return newSprite;
    }
    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
    public static Texture2D RoundCrop(Texture2D sourceTexture)
    {
        int width;
        int height;
        int center;// = (sourceTexture.width < sourceTexture.height) ? (sourceTexture.width) : (sourceTexture.height);
        if (sourceTexture.width > sourceTexture.height)
        {
            center = sourceTexture.height;
        }
        else
        {
            center = sourceTexture.width;
            //sourceTexture.
        }
        width = center;
        height = center;
        if (sourceTexture.width == sourceTexture.height)
        {
            width = sourceTexture.width;
            height = sourceTexture.height;
        }
        float radius = (width < height) ? (width / 2f) : (height / 2f);
        float centerX = width / 2f;
        float centerY = height / 2f;
        Vector2 centerVector = new Vector2(centerX, centerY);
        // pixels are laid out left to right, bottom to top (i.e. row after row)
        Color[] colorArray = sourceTexture.GetPixels(0, 0, width, height);
        Color[] croppedColorArray = new Color[width * height];
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                int colorIndex = (row * width) + column;
                float pointDistance = Vector2.Distance(new Vector2(column, row), centerVector);
                if (pointDistance < radius)
                {
                    croppedColorArray[colorIndex] = colorArray[colorIndex];
                }
                else
                {
                    croppedColorArray[colorIndex] = Color.clear;
                }
            }
        }
        Texture2D croppedTexture = new Texture2D(width, height);
        croppedTexture.SetPixels(croppedColorArray);
        croppedTexture.Apply();
        return croppedTexture;
    }
    public async static Task<string> FixImage(byte[] avatar)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("avatar", avatar);
        var url = Endpoint.classesURL + "/users/avatars/upload";
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        Debug.LogWarning(response);
        var N = JSON.Parse(response);
        //Save The current Session ID
        AvatarURL = N["data"].Value;
        return AvatarURL;
    }
}
