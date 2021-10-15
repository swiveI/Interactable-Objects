
using UdonSharp;
using UnityEngine;
using UnityEngine.XR;
using VRC.SDKBase;
using VRC.Udon;

public class Locker : UdonSharpBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] AudioClip fullyOpenSound;

    public void SendMinEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Close");
    }

    public void SendCustomTriggerEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Open");
    }
    public void SendMaxEvent()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OverExtend");
    }

    public void Open()
    {
        audioSource.PlayOneShot(openSound);
    }
    public void Close()
    {
        audioSource.PlayOneShot(closeSound);
    }
    public void OverExtend()
    {
        audioSource.PlayOneShot(fullyOpenSound);
    }

}
