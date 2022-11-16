
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
public class Kpushcollider : UdonSharpBehaviour
{
    public int boneNumber = 0;
    VRCPlayerApi localplayer;
    HumanBodyBones TrackedBone;
    SphereCollider collider;
    void Start()
    {
        localplayer = Networking.LocalPlayer;
        if (!localplayer.IsUserInVR())
        {
            gameObject.SetActive(false);
            return;
        }
        SetupCollider();
    }
    private void SetupCollider()
    {
        collider = GetComponent<SphereCollider>();
        switch (boneNumber)
        {
            case 0:
                TrackedBone = HumanBodyBones.LeftIndexDistal;
                collider.radius = .01f;
                break;
            case 1:
                TrackedBone = HumanBodyBones.RightIndexDistal;
                collider.radius = .01f;
                break;
            case 2:
                TrackedBone = HumanBodyBones.LeftFoot;
                collider.radius = .02f;
                break;
            case 3:
                TrackedBone = HumanBodyBones.RightFoot;
                collider.radius = .02f;
                break;
            case 4:
                TrackedBone = HumanBodyBones.Hips;
                collider.radius = .07f;
                break;
        }
    }
    private void FixedUpdate()
    {
        transform.position = localplayer.GetBonePosition(TrackedBone);
    }
}
