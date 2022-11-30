
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class _MirrorToggle : UdonSharpBehaviour
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

    public void _Desktopbutton()
    {
        manager.ToggleMirror(MirrorToToggle);
    }
    public void _SendMaxEvent()
    {
        manager.ToggleMirror(MirrorToToggle);
    }
}
