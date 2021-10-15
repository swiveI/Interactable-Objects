
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ParentPickups : UdonSharpBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
            other.transform.parent = transform.parent;
    }
    public void OnTriggerExit(Collider other)
    {
            other.transform.parent = null;
    }
}
