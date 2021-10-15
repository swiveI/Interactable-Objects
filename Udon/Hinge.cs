
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace Karet.InteractableObjects
{
    [RequireComponent(typeof(VRCPickup))]
    public class Hinge : UdonSharpBehaviour
    {
        bool updateInteract = false;
        bool eventCooldown = false;
        bool customEventCooldown = false;

        [Header("Hinge Value")]
        [Tooltip("When limited, this is the output value of the hinge from 0-1")]
        public float OutputValue;

        [Header("Limits")]
        [Tooltip("When limited, the hinge will only spin between these numbers as degrees")]
        public bool limit = false;
        public float minAngle;
        public float maxAngle;

        [Header("Haptics")]
        [Tooltip("Attempt to play haptics on the controler interacting with the hinge")]
        [SerializeField] bool PlayHaptics;

        [Header("Objects")]
        [SerializeField] Transform handlePos;
        [SerializeField] GameObject Mesh;

        [Header("Events")]
        [Tooltip("Enter a value between 0-1")]
        [Range(0, 1)]
        public float customTriggerValue;
        [Tooltip("SendCustomTriggerEvent will get called on this UdonBehavior once when the hinge value is greater than the customEventTrigger value")]
        [SerializeField] UdonBehaviour customTriggerEvent;
        [Tooltip("SendMaxEvent will get called on this UdonBehavior once when the hinge is fully open")]
        [SerializeField] UdonBehaviour maxEvent;
        [Tooltip("SendMinEvent will get called on this UdonBehavior once when the hinge is fully closed")]
        [SerializeField] UdonBehaviour minEvent;

        VRC_Pickup pickup;
        VRCPlayerApi localplayer;
        Vector3 newEA;
        private void Start()
        {
            localplayer = Networking.LocalPlayer;
            pickup = (VRC_Pickup)GetComponent(typeof(VRC_Pickup));
            newEA = new Vector3(0f, 0f, 0f);
        }
        public override void OnPickup()
        {
            updateInteract = true;
            Networking.SetOwner(localplayer, Mesh);
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DisablePickup");
        }
        public void DisablePickup()
        {
            if (Networking.GetOwner(pickup.gameObject) != localplayer)
            {
                pickup.pickupable = false;
            }
        }
        public override void OnDrop()
        {
            updateInteract = false;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnablePickup");
        }
        public void EnablePickup()
        {
            gameObject.transform.position = handlePos.transform.position;
            gameObject.transform.rotation = handlePos.transform.rotation;
            pickup.pickupable = true;
        }

        private void Update()
        {

            if (updateInteract)
            {
                float angle = Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * Mathf.Rad2Deg;
                if (limit)
                {
                    angle = Mathf.Clamp(angle, minAngle, maxAngle);
                    OutputValue = Mathf.Abs((angle - minAngle) / (maxAngle - minAngle));
                    if (OutputValue > customTriggerValue)
                    {
                        if (customTriggerEvent != null && !customEventCooldown)
                        {
                            customTriggerEvent.SendCustomEvent("SendCustomTriggerEvent");
                            if (PlayHaptics)
                            {
                                localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                            }
                            customEventCooldown = true;
                        }
                    }
                    if (OutputValue >= 1f)
                    {
                        if (maxEvent != null && !eventCooldown)
                        {
                            maxEvent.SendCustomEvent("SendMaxEvent");
                            if (PlayHaptics)
                            {
                                localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                            }
                        }
                        eventCooldown = true;
                    }

                    if (OutputValue <= 0f)
                    {
                        if (minEvent != null && !eventCooldown)
                        {
                            minEvent.SendCustomEvent("SendMinEvent");
                            if (PlayHaptics)
                            {
                                localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                            }
                            eventCooldown = true;
                        }
                    }
                    if (OutputValue < customTriggerValue) customEventCooldown = false;
                    if (angle > minAngle && angle < maxAngle) eventCooldown = false;
                }
                newEA.z = angle;
                Mesh.transform.localEulerAngles = newEA;
            }
        }





    }
}