using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TFClassify;
using System.Diagnostics;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;


public class PhoneCamera : MonoBehaviour
{
    
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    private Classifier classifier;


    public RawImage background;
    public AspectRatioFitter fitter;
    public TextAsset modelFile;
    public TextAsset labelsFile;
    public Text uiText;


    private void Start()
    {
        LoadTFWorker();

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            this.uiText.text = "No camera detected";
            camAvailable = false;

            return;
        }

        for(int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCam == null)
        {
            this.uiText.text = "Unable to find back camera";
            
            return;
        }


        backCam.Play();
        background.texture = backCam;
        camAvailable = true;

        InvokeRepeating(nameof(TFDetect), 1f, 1f);
    }


    private void Update()
    {
        if(!this.camAvailable)
        {
            return;
        }

        float ratio = (float)backCam.width / (float)backCam.height;
        fitter.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }


    private void LoadTFWorker()
    {
        this.classifier = new Classifier(
            this.modelFile.bytes,
            Regex.Split(this.labelsFile.text, "\n|\r|\r\n" ));
    }


    private async void TFDetect()
    {
        var snap = TakeTextureSnap();
        var scaled = Scale(snap);
        var rotated = await RotateAsync(scaled.GetPixels32(), scaled.width, scaled.height);
        var probabilities = await this.classifier.ClassifyAsync(rotated);
        
        this.uiText.text = String.Empty;

        for(int i = 0; i < 3; i++)
        {
            this.uiText.text += probabilities[i].Key + ": " + String.Format("{0:0.000}%", probabilities[i].Value) + "\n";
        }
    }


    private Texture2D TakeTextureSnap()
    {
        var smallest = backCam.width < backCam.height ?
            backCam.width : backCam.height;
        var snap = TextureTools.CropWithRect(backCam,
             new Rect(0, 0, smallest, smallest),
            TextureTools.RectOptions.Center, 0, 0);

        return snap;
    }

    private Texture2D Scale(Texture2D texture)
    {
        var scaled = TextureTools.scaled(texture, 224, 224, FilterMode.Bilinear);
        
        return scaled;
    }


    private Task<Color32[]> RotateAsync(Color32[] pixels, int width, int height)
    {
        return Task.Run(() =>
        {
            return TextureTools.RotateImageMatrix(
                    pixels, width, height, -90);
        });
    }


    private void SaveToFile(Texture2D texture)
    {
        File.WriteAllBytes(
            Application.persistentDataPath + "/" +
            "snap.png", texture.EncodeToPNG());
    }
}
