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

        transform.Translate(Vector3.forward * moveInput.y * speed * Time.deltaTime);
        transform.Translate(Vector3.right * moveInput.x * speed * Time.deltaTime);
    }
}
