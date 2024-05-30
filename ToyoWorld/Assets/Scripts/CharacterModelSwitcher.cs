using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelSwitcher : MonoBehaviour
{
    [Header("Male Character")]
    [SerializeField] GameObject maleModel;
    [SerializeField] Avatar maleAvatar;
    [SerializeField] Vector3 maleHandOffset;

    [Header("Female Character")]
    [SerializeField] GameObject femaleModel;
    [SerializeField] Avatar femaleAvatar;
    [SerializeField] Vector3 femaleHandOffset;

    private void Start()
    {
        //PlayerPrefs.SetInt("PlayerGender", 1);

        var player = GetComponent<PlayerController>();

        if (PlayerPrefs.GetInt("PlayerGender") == 0)
        {
            maleModel.SetActive(true);
            femaleModel.SetActive(false);

            player.animator.avatar = maleAvatar;
            player.HandOffset = maleHandOffset;
        }
        else
        {
            femaleModel.SetActive(true);
            maleModel.SetActive(false);

            player.animator.avatar = femaleAvatar;
            player.HandOffset = femaleHandOffset;
        }

        player.animator.WriteDefaultValues();
    }
}
