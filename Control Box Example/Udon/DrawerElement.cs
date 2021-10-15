
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;

public class DrawerElement : UdonSharpBehaviour
{
    public Text value;
    public Text maxevent;
    public Text minevent;
    public Text customevent;
    public GameObject drawerObject;

    //audio
    public AudioSource audioSource;
    public AudioClip endSound;

    Drawer drawer;
    bool eventCooldown;
    float cooldowndelay = 3;

    private void Start()
    {
        drawer = drawerObject.GetComponent<Drawer>();
        maxevent.color = Color.red;
        minevent.color = Color.red;
        customevent.color = Color.red;
    }
    private void Update()
    {
        value.text = drawer.OutputValue.ToString();
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
        audioSource.PlayOneShot(endSound);
    }

    public void SendMinEvent()
    {
        minevent.color = Color.green;
        eventCooldown = true;
        audioSource.PlayOneShot(endSound);
    }
    public void SendCustomTriggerEvent()
    {
        customevent.color = Color.green;
        eventCooldown = true;
    }
}
