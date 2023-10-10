using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject generalUI;

    private Texture2D capture;
    private bool viewingPhoto;

    // Start is called before the first frame update
    void Start()
    {
        capture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!viewingPhoto)
            {
                generalUI.gameObject.SetActive(false);
                StartCoroutine(CapturePhoto());
            }
            else
            {
                generalUI.gameObject.SetActive(true);
                RemovePhoto();
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0,0, Screen.width, Screen.height);

        capture.ReadPixels(regionToRead, 0, 0, false);
        capture.Apply();
        ShowPhoto();
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(capture, new Rect(0.0f, 0.0f, capture.width, capture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);

    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
        //cameraui true
    }

}
