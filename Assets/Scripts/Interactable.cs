using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Action()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == MainChar.Instance)
            MainChar.SetPlayerInteractable(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == MainChar.Instance)
            MainChar.ClearPlayerInteractable();
    }
}
