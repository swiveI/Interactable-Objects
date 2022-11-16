
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;
public class DialElement : UdonSharpBehaviour
{
    public Text value;
    public Text maxevent;
    public Text minevent;
    public Text customevent;
    public GameObject dialObject;

    //audio
    public AudioSource audioSource;
    public AudioClip tickSound;
    public AudioClip endSound;
    float lastTick = 0;

    Dial dial;
    bool eventCooldown;
    float cooldowndelay = 3;

    private void Start()
    {
        dial = dialObject.GetComponent<Dial>();
        maxevent.color = Color.red;
        minevent.color = Color.red;
        customevent.color = Color.red;
    }
    private void Update()
    {
        value.text = dial.OutputValue.ToString();
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
        if (dial.OutputValue > lastTick +.1 || dial.OutputValue < lastTick - .1)
        {
            audioSource.PlayOneShot(tickSound);
            lastTick = dial.OutputValue;
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
