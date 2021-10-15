

using UdonSharp;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SocialPlatforms;
using VRC.SDKBase;
using VRC.Udon;

namespace Karet.InteractableObjects
{
    public class PushButton : UdonSharpBehaviour
    {
        Vector3 origin;
        float buttonMax;
        Vector3 contactpoint;

        public float OutputValue;
        public bool IsHeld;

        [SerializeField]
        GameObject ButtonMesh;

        [Tooltip("This is how far you want your button to be pushed back. Should be a negitive value.")]
        public float PushDistance;

        [Header("Events")]
        [Range(0, 1)]
        public float customTriggerValue;
        [Tooltip("SendCustomTriggerEvent will get called on this UdonBehavior once when the button value is greater than the customEventTrigger value")]
        public UdonBehaviour customTriggerEvent;
        [Tooltip("SendMaxEvent will get called on this UdonBehavior once when the button is fully pressed")]
        public UdonBehaviour maxEvent;
        [Tooltip("SendMinEvent will get called on this UdonBehavior once when the button is reset")]
        public UdonBehaviour minEvent;
        bool eventCooldown = false;
        bool customEventCooldown = false;

        float distanceToMove = 0;

        private void Start()
        {
            origin = ButtonMesh.transform.localPosition;
            buttonMax = origin.z - PushDistance;
        }
        public void OnTriggerEnter(Collider other)
        {
            contactpoint = transform.InverseTransformPoint(other.transform.position);
        }
        public void OnTriggerStay(Collider other)
        {
            float fingerPos = transform.InverseTransformPoint(other.transform.position).z;
            float buttonpos = ButtonMesh.transform.localPosition.z;
            if (contactpoint.z < origin.z)
            {
                return;
            }
            distanceToMove = contactpoint.z - fingerPos;
            ButtonMesh.transform.localPosition = new Vector3(ButtonMesh.transform.localPosition.x, ButtonMesh.transform.localPosition.y, -distanceToMove);
            if (ButtonMesh.transform.localPosition.z < buttonMax)
            {
                ButtonMesh.transform.localPosition = new Vector3(ButtonMesh.transform.localPosition.x, ButtonMesh.transform.localPosition.y, buttonMax);
                IsHeld = true;
                if (!eventCooldown && maxEvent != null)
                {
                    maxEvent.SendCustomEvent("SendMaxEvent");
                    eventCooldown = true;
                }
            }
            if (ButtonMesh.transform.localPosition.z > origin.z)
            {
                ButtonMesh.transform.localPosition = origin;
                if (!eventCooldown && minEvent != null)
                {
                    minEvent.SendCustomEvent("SendMinEvent");
                    eventCooldown = true;
                }
            }
            OutputValue = Mathf.Abs((ButtonMesh.transform.localPosition.z - origin.z) / (buttonMax - origin.z));
            if (OutputValue > 0.05 && OutputValue < .95 && eventCooldown)
            {
                eventCooldown = false;
                IsHeld = false;
            }
            if (OutputValue > customTriggerValue && !customEventCooldown && customTriggerEvent != null)
            {
                customTriggerEvent.SendCustomEvent("SendCustomTriggerEvent");
                customEventCooldown = true;
            }
            if (OutputValue < customTriggerValue && customEventCooldown)
            {
                customEventCooldown = false;
            }
        }
        public void OnTriggerExit(Collider other)
        {
            ButtonMesh.transform.localPosition = origin;
            if (eventCooldown == false && minEvent != null)
            {
                minEvent.SendCustomEvent("SendMinEvent");
                eventCooldown = true;
            }
            OutputValue = 0;
            customEventCooldown = false;
            IsHeld = false;
        }
    }
}