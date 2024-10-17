using System;
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

    private void OnEnable()
    {
        var player = GetComponent<PlayerController>();
        var animator = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("PlayerGender") == 0)
        {
            maleModel.SetActive(true);
            femaleModel.SetActive(false);

            animator.avatar = maleAvatar;
            player.HandOffset = maleHandOffset;
        }
        else
        {
            femaleModel.SetActive(true);
            maleModel.SetActive(false);

            animator.avatar = femaleAvatar;
            player.HandOffset = femaleHandOffset;
        }

        animator.WriteDefaultValues();
    }
}
