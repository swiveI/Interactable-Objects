
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MirrorToggle : UdonSharpBehaviour
{
    [SerializeField] MirrorManager manager;
    [SerializeField] GameObject MirrorToToggle;
    [SerializeField] BoxCollider buttonCollider;
    private void Start()
    {

        if (!Networking.LocalPlayer.IsUserInVR())
        {
            buttonCollider.enabled = true;
        }
    }

    public void Desktopbutton()
    {
        manager.ToggleMirror(MirrorToToggle);
    }
    public void SendMaxEvent()
    {
        manager.ToggleMirror(MirrorToToggle);
    }
}
