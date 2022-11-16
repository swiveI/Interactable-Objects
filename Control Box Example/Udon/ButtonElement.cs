
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;

public class ButtonElement : UdonSharpBehaviour
{
    public GameObject pushableButton;
    PushButton button;

    public Text value;
    public Text minevent;
    public Text maxevent;
    public Text customevent;
    public Text isheld;

    public AudioSource audioSource;
    public AudioClip clickIn;
    public AudioClip clickOut;

    bool eventCooldown = false;
    float cooldowndelay = 3;

    private void Start()
    {
        button = pushableButton.GetComponent<PushButton>();
        maxevent.color = Color.red;
        minevent.color = Color.red;
        customevent.color = Color.red;
        isheld.color = Color.red;        
    }
    private void Update()
    {
       value.text = button.OutputValue.ToString();
       if (button.IsHeld)
        {
            isheld.color = Color.green;
        }
        else
        {
            isheld.color = Color.red;
        }
       if (eventCooldown)
        {
            if (cooldowndelay > 0)
            {
                cooldowndelay -= Time.deltaTime;
            }
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
        audioSource.PlayOneShot(clickIn);
    }

    public void SendMinEvent()
    {
        minevent.color = Color.green;
        eventCooldown = true;
        audioSource.PlayOneShot(clickOut);
    }
    public void SendCustomTriggerEvent()
    {
        customevent.color = Color.green;
    }
}
