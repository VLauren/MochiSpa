using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float PositionMultiplier = 0.5f;

    Transform target;
    Vector2 camVelocity;

    void Start()
    {
        target = FindFirstObjectByType<MainChar>().transform;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector2 newPos = Vector2.SmoothDamp(transform.position, target.position * PositionMultiplier + new Vector3(0, 0, transform.position.z), ref camVelocity, 0.3f);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
