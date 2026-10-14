using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 5.0f;

    public InputAction MoveAction;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = MoveAction.ReadValue<Vector2>();

        // Build a single movement vector and clamp its magnitude to 1 so diagonal input
        // does not result in faster movement than horizontal/vertical.
        Vector3 input = new Vector3(moveInput.x, 0f, moveInput.y);
        if (input.sqrMagnitude > 1f)
        {
            input = input.normalized;
        }

        // Apply movement in local space (forward/right) at constant speed.
        transform.Translate(input * speed * Time.deltaTime, Space.Self);
    }
}

