using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetToyosDefeated : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI defeated;
    // Start is called before the first frame update
    void Start()
    {
        defeated.text = $"{GameController.instance.toyosDefeated}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
