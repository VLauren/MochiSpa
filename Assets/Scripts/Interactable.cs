using System;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : ZSortedObject
{
    public virtual void Action()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == MainChar.Instance.gameObject)
            MainChar.SetPlayerInteractable(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == MainChar.Instance)
            MainChar.ClearPlayerInteractable();
    }
}
