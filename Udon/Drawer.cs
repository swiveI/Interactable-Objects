
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace Karet.InteractableObjects
{
    [RequireComponent(typeof(VRCPickup))]
    public class Drawer : UdonSharpBehaviour
    {
        bool updateInteract = false;
        bool eventCooldown = false;
        bool customEventCooldown = false;

        [Header("Drawer Value")]
        [Tooltip("When limited, this is the output value of the drawer from 0-1")]
        public float OutputValue;

        [Header("Limits")]
        [Tooltip("When limited, the drawer will only move between these two distances from its origin")]
        public bool limit = false;
        public float minDistance;
        public float maxDistance;

        [Header("Haptics")]
        [Tooltip("Attempt to play haptics on the controler interacting with the drawer")]
        [SerializeField] bool PlayHaptics;

        [Header("Objects")]
        [SerializeField] Transform handlePos;
        [SerializeField] GameObject drawer;

        [Header("Events")]
        [Tooltip("Enter a value between 0-1")]
        [Range(0, 1)]
        public float customTriggerValue;
        [Tooltip("SendCustomTriggerEvent will get called on this UdonBehavior once when the output value is greater than the customEventTrigger value")]
        [SerializeField] UdonBehaviour customTriggerEvent;
        [Tooltip("SendMaxEvent will get called on this UdonBehavior when the drawer is fully open")]
        [SerializeField] UdonBehaviour maxEvent;
        [Tooltip("SendMinEvent will get called on this UdonBehavior when the drawer is fully closed")]
        [SerializeField] UdonBehaviour minEvent;

        float handleOffset;
        Vector3 newPosition;
        VRC_Pickup pickup;
        VRCPlayerApi localplayer;

        private void Start()
        {
            handleOffset = handlePos.transform.localPosition.z;
            pickup = (VRC_Pickup)GetComponent(typeof(VRC_Pickup));
            localplayer = Networking.LocalPlayer;
            newPosition = drawer.transform.localPosition;
            SendCustomEventDelayedSeconds("updateHandlePos", 1);
        }

        public void updateHandlePos()
        {
            gameObject.transform.position = handlePos.transform.position;
            gameObject.transform.rotation = handlePos.transform.rotation;
        }
        public override void OnPickup()
        {
            updateInteract = true;
            Networking.SetOwner(localplayer, drawer);
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
                float distanceToMove = gameObject.transform.localPosition.z - handleOffset;
                if (limit)
                {
                    distanceToMove = Mathf.Clamp(distanceToMove, minDistance, maxDistance);

                    if (distanceToMove >= maxDistance && !eventCooldown)
                    {
                        if (maxEvent != null && !eventCooldown)
                        {
                            maxEvent.SendCustomEvent("SendMaxEvent");
                            eventCooldown = true;
                            if (PlayHaptics)
                            {
                                localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                            }
                        }
                    }
                    if (distanceToMove <= minDistance && !eventCooldown)
                    {
                        if (minEvent != null && !eventCooldown)
                        {
                            minEvent.SendCustomEvent("SendMinEvent");
                            eventCooldown = true;
                            if (PlayHaptics)
                            {
                                localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                            }
                        }
                    }
                    OutputValue = Mathf.Abs((distanceToMove - minDistance) / (maxDistance - minDistance));
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
                    if (distanceToMove > minDistance && distanceToMove < maxDistance) eventCooldown = false;
                    if (OutputValue < customTriggerValue) customEventCooldown = false;
                }
                newPosition.z = distanceToMove;
                drawer.transform.localPosition = newPosition;
                drawer.transform.localRotation = Quaternion.identity;
            }
        }
    }
}