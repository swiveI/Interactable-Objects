using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Wrapper.Modules;
using VRC.SDK3.Components;

namespace Karet.InteractableObjects
{
    [RequireComponent(typeof(VRCPickup))]
    public class Dial : UdonSharpBehaviour
    {
        bool updateInteract = false;
        bool eventCooldown = false;
        bool customEventCooldown = false;

        [Header("Dial Value")]
        [Tooltip("When limited, this is the output value of the dial from 0-1")]
        public float OutputValue;

        [Header("Limits")]
        [Tooltip("When limited, the dial will only spin between these numbers as degrees")]
        public bool limit = false;
        [Tooltip("Negitive numbers can break things! Min should start at 0 or higher")]
        public float minAngle;
        public float maxAngle;

        [Header("Haptics")]
        [Tooltip("Attempt to play haptics on the controler interacting with the hinge")]
        [SerializeField] bool PlayHaptics;

        [Header("Objects")]
        [SerializeField] Transform handlePos;
        [SerializeField] GameObject dial;

        [Header("Events")]
        [Tooltip("Enter a value between 0-1")]
        [Range(0, 1)]
        public float customTriggerValue;
        [Tooltip("SendCustomTriggerEvent will get called on this UdonBehavior once when the hinge value is greater than the customEventTrigger value")]
        [SerializeField] UdonBehaviour customTriggerEvent;
        [Tooltip("SendMaxEvent will get called on this UdonBehavior when the dial is at 100%")]
        [SerializeField] UdonBehaviour maxEvent;
        [Tooltip("SendMinEvent will get called on this UdonBehavior when the dial is at 0%")]
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
            Networking.SetOwner(localplayer, dial);
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
                float distanceToRotate = transform.localEulerAngles.z;
                if (limit)
                {
                    distanceToRotate = Mathf.Clamp(distanceToRotate, minAngle, maxAngle);
                    OutputValue = Mathf.Abs((distanceToRotate - minAngle) / (maxAngle - minAngle));

                    if (OutputValue > customTriggerValue)
                    {
                        if (customTriggerEvent != null && !customEventCooldown) customTriggerEvent.SendCustomEvent("SendCustomTriggerEvent");
                        if (PlayHaptics)
                        {
                            localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                        }
                        customEventCooldown = true;
                    }
                    if (distanceToRotate >= maxAngle)
                    {
                        newEA.z = maxAngle;
                        gameObject.transform.localEulerAngles = newEA;
                        if (maxEvent != null && !eventCooldown) maxEvent.SendCustomEvent("SendMaxEvent");
                        if (PlayHaptics)
                        {
                            localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                        }
                        eventCooldown = true;
                    }
                    if (distanceToRotate <= minAngle)
                    {
                        newEA.z = minAngle;
                        gameObject.transform.localEulerAngles = newEA;
                        if (minEvent != null && !eventCooldown) minEvent.SendCustomEvent("SendMinEvent");
                        if (PlayHaptics)
                        {
                            localplayer.PlayHapticEventInHand(pickup.currentHand, .25f, .5f, .5f);
                        }
                        eventCooldown = true;
                    }
                    if (OutputValue < customTriggerValue) customEventCooldown = false;
                    if (distanceToRotate > minAngle && distanceToRotate < maxAngle) eventCooldown = false;
                }
                newEA.z = distanceToRotate;
                dial.transform.localEulerAngles = newEA;
            }
        }
    }
}