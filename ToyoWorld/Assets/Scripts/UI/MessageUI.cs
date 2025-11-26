using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] TMP_Text messageTxt;
    
    public static MessageUI i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        Pokeball.OnToyoCaptured += (Toyo toyo) => ShowMessage("Catch Successfull");
    }

    public void ShowMessage(string message, float popUpTime = 0.3f, float showTime = 1f, float fadeOutTime = 2f)
    {
        StartCoroutine(ShowMessageAsync(message));
    }

    public IEnumerator ShowMessageAsync(string message, float popUpTime = 0.3f, float showTime = 1f, float fadeOutTime = 2f)
    {
        messageTxt.gameObject.SetActive(true);
        messageTxt.text = message;

        var ogColor = messageTxt.color;
        messageTxt.color = new Color(ogColor.r, ogColor.g, ogColor.b, 100);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, popUpTime);
        yield return new WaitForSeconds(showTime);
        messageTxt.DOFade(0f, fadeOutTime).OnComplete(() =>
        {
            messageTxt.gameObject.SetActive(false);
        });
    }
}
