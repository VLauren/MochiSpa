using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainChar : MonoBehaviour
{
    public float MovementSpeed = 3;

    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }
    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement *= Time.deltaTime * MovementSpeed;
        transform.Translate(movement);
    }
}
