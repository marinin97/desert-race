using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTextAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowAnimation()
    {
        _animator.SetTrigger("Barrel");
    }
}
