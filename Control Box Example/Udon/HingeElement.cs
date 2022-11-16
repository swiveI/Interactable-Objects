
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;
public class HingeElement : UdonSharpBehaviour
{
    public Text value;
    public Text maxevent;
    public Text minevent;
    public Text customevent;
    public GameObject hingeObject;
    Hinge hinge;
    bool eventCooldown;
    float cooldowndelay = 3;

    public GameObject[] controls;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip fullyExtendedSound;

    private void Start()
    {
        hinge = hingeObject.GetComponent<Hinge>();
        maxevent.color = Color.red;
        minevent.color = Color.red;
        customevent.color = Color.red;
    }
    private void Update()
    {
        value.text = hinge.OutputValue.ToString();
        if (eventCooldown)
        {
            if (cooldowndelay >= 0) cooldowndelay -= Time.deltaTime;
            else
            {
                maxevent.color = Color.red;
                minevent.color = Color.red;
                customevent.color = Color.red;
                cooldowndelay = 3;
                eventCooldown = false;
            }
        }
    }

    public void SendMaxEvent()
    {
        maxevent.color = Color.green;
        eventCooldown = true;
        audioSource.PlayOneShot(fullyExtendedSound);
    }

    public void SendMinEvent()
    {
        minevent.color = Color.green;
        eventCooldown = true;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "disableControls");
    }
    public void SendCustomTriggerEvent()
    {
        customevent.color = Color.green;
        eventCooldown = true;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "enableControls");
    }
    public void enableControls()
    {
        audioSource.PlayOneShot(openSound);
        foreach (GameObject obj in controls)
        {
            obj.SetActive(true);
        }
    }
    public void disableControls()
    {
        audioSource.PlayOneShot(closeSound);
        foreach (GameObject obj in controls)
        {
            obj.SetActive(false);
        }
    }

}
