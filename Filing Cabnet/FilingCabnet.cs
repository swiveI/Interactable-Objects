
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FilingCabnet : UdonSharpBehaviour
{
    public AudioSource audiosource;
    public AudioClip opensound;
    public AudioClip closesound;
    public AudioClip fulllyextendedsound;

    public void PlayOpenSound()
    {
        audiosource.PlayOneShot(opensound);
    }

    public void PlayCloseSound()
    {
        audiosource.PlayOneShot(closesound);
    }

    public void PlayfullSound()
    {
        audiosource.PlayOneShot(fulllyextendedsound);
    }

    public void SendCustomTriggerEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayOpenSound");
    }

    public void SendMinEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayCloseSound");
    }

    public void SendMaxEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayfullSound");
    }
}
