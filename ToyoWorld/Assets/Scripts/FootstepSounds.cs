using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource footStep1;
    [SerializeField] private AudioSource footStep2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstepOne()
    {
        footStep1.Play();
    }

    public void PlayFootstepTwo()
    {
        footStep2.Play();
    }
}
