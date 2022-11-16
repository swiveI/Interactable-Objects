using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using Karet.InteractableObjects;
public class SliderElement : UdonSharpBehaviour
{
    [SerializeField]
    GameObject pushableSlider;
    PushSlider slider;

    public Text value;
    public Text minevent;
    public Text maxevent;
    public Text customevent;

    public AudioSource audioSource;
    public AudioClip clickEnd;
    public AudioClip slide;

    float oldSliderValue = 0;
    bool eventCooldown = false;
    float cooldowndelay = 3;
    private void Start()
    {
        slider = pushableSlider.GetComponent<PushSlider>();
        maxevent.color = Color.red;
        minevent.color = Color.red;
        customevent.color = Color.red;
    }

    private void OnValueChanged()
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(slide);
        }
        
    }
    private void Update()
    {
        float slidervalue = slider.OutputValue;
        if (oldSliderValue != slidervalue)
        {
            value.text = slidervalue.ToString();
            OnValueChanged();
            oldSliderValue = slidervalue;
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
        audioSource.Stop();
        audioSource.PlayOneShot(clickEnd);
    }

    public void SendMinEvent()
    {
        minevent.color = Color.green;
        eventCooldown = true;
        audioSource.Stop();
        audioSource.PlayOneShot(clickEnd);
    }
    public void SendCustomTriggerEvent()
    {
        customevent.color = Color.green;
        eventCooldown = true;
    }
}
