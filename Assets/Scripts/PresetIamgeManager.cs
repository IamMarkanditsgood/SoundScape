using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetIamgeManager : MonoBehaviour
{

    private int maxSize = 1000;
    private string _path;
    private Texture2D _texture;

    public void SetSavedPicture(string savedPath, RawImage image, Texture2D basicSprite)
    {
        if (PlayerPrefs.HasKey("AvatarPath"))
        {
            string path = savedPath;
            image.texture = NativeGallery.LoadImageAtPath(path, maxSize);
        }
        else
        {
            image.texture = basicSprite;
        }
    }

    public string PickFromGallery(RawImage image, Texture2D basicSprite)
    {
        if (NativeGallery.IsMediaPickerBusy())
            return "";

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                _path = path;

                _texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (_texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                }
                image.texture = _texture;
                //PlayerPrefs.SetString("AvatarPath", _path);
            }
        }, "Select an image", "image/*");
        return _path;
    }
}
