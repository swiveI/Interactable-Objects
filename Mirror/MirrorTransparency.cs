
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;
public class MirrorTransparency : UdonSharpBehaviour
{
    [SerializeField] PushSlider Slider;
    [SerializeField] Drawer Drawer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip slide;
    float oldValue = .5f;
    bool usingSlider = true;
    bool usingDrawer = false;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
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
        mat.SetFloat("_Transparency", 1 - value);
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