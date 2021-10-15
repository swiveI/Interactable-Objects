
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;


public class MirrorManager : UdonSharpBehaviour
{
    [SerializeField] GameObject NormalMirror;
    [SerializeField] GameObject OptimisedMirror;
    [SerializeField] GameObject TransparentMirror;
    GameObject currentMirror;

    [SerializeField] AudioClip click;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void ToggleMirror(GameObject Mirror)
    {
        if (Mirror != null)
        {
            audio.PlayOneShot(click);
            if (NormalMirror != null && Mirror != NormalMirror) NormalMirror.SetActive(false);
            if (OptimisedMirror != null && Mirror != OptimisedMirror) OptimisedMirror.SetActive(false);
            if (TransparentMirror != null && Mirror != TransparentMirror) TransparentMirror.SetActive(false);
            Mirror.SetActive(!Mirror.activeSelf);
            if (Mirror.activeSelf)
            {
                currentMirror = Mirror;
            }
            else currentMirror = null;
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer && currentMirror != null)
        {
            audio.PlayOneShot(click);
            currentMirror.SetActive(true);
        }
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer && currentMirror != null)
        {
            audio.PlayOneShot(click);
            currentMirror.SetActive(false);
        }
    }
}
