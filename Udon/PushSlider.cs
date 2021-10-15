
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace Karet.InteractableObjects
{
    public class PushSlider : UdonSharpBehaviour
    {
        public float OutputValue;
        [Header("Limits")]
        public float minDistance;
        public float maxDistance;
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
        Vector3 contactpoint;

        public void OnTriggerEnter(Collider other)
        {
            contactpoint = transform.parent.InverseTransformPoint(other.transform.position);
        }

        public void OnTriggerStay(Collider other)
        {
            float fingerPos = transform.parent.InverseTransformPoint(other.transform.position).z;
            float distancetomove = (contactpoint.z - fingerPos);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - distancetomove);
            if (transform.localPosition.z > maxDistance)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, maxDistance);
                if (eventCooldown == false && maxEvent != null)
                {
                    maxEvent.SendCustomEvent("SendMaxEvent");
                    eventCooldown = true;
                }
            }
            if (transform.localPosition.z < minDistance)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, minDistance);
                if (eventCooldown == false && minEvent != null)
                {
                    minEvent.SendCustomEvent("SendMinEvent");
                    eventCooldown = true;
                }
            }
            OutputValue = Mathf.Abs((transform.localPosition.z - minDistance) / (maxDistance - minDistance));
            Debug.Log(OutputValue);
            if (OutputValue > 0.05 && OutputValue < .95 && eventCooldown)
            {
                eventCooldown = false;
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
    }
}