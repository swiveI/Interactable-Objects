﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;
public class LightIntensity : UdonSharpBehaviour
{
    [SerializeField] PushSlider Slider;
    [SerializeField] Drawer Drawer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip slide;
    float oldValue = .5f;
    bool usingSlider = true;
    bool usingDrawer = false;
    Light lit;
    private void Start()
    {
        lit = GetComponent<Light>();
        if (Networking.LocalPlayer.IsUserInVR() == false)
        {
            usingSlider = false;
            usingDrawer = true;
        }
        else Drawer.gameObject.SetActive(false);
    }
    private void OnValueChanged(float value)
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(slide);
        }
        lit.intensity = (1 - value);
    }
    private void FixedUpdate()
    {
        if (usingSlider && Slider.OutputValue != oldValue)
        {
            OnValueChanged(Slider.OutputValue);
            oldValue = Slider.OutputValue;
        }
        if (usingDrawer && Drawer.OutputValue != oldValue)
        {
            OnValueChanged(Drawer.OutputValue);
            oldValue = Drawer.OutputValue;
        }
    }
}