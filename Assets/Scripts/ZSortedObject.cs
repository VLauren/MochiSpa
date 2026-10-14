using UnityEngine;

public class ZSortedObject : MonoBehaviour
{
    Renderer[] Renderers;

    protected virtual void Start()
    {
        UpdateSortingOrder();
    }
    
    protected void UpdateSortingOrder()
    {
        if(Renderers == null)
            Renderers = GetComponentsInChildren<Renderer>();

        int order = (int)(transform.position.y * -1000f);
        foreach (var renderer in Renderers)
        {
            renderer.sortingOrder = order;
        }
    }
}
