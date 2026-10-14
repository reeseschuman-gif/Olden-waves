using UnityEngine;

public class RotateInMovementDirection : MonoBehaviour
{
    // If left null the script will use the GameObject's parent as the moving target.
    public Transform targetToFollow;
    public float rotationSpeed = 10f;
    public float minMoveThreshold = 0.01f;

    private Vector3 lastPosition;

    void Start()
    {
        if (targetToFollow == null && transform.parent != null)
        {
            targetToFollow = transform.parent;
        }

        lastPosition = targetToFollow != null ? targetToFollow.position : transform.position;
    }

    void LateUpdate()
    {
        if (targetToFollow == null)
        {
            return;
        }

        // Calculate how the target moved since last frame
        Vector3 delta = targetToFollow.position - lastPosition;
        lastPosition = targetToFollow.position;

        // Ignore vertical movement and very small movements
        Vector3 moveDir = new Vector3(delta.x, 0f, delta.z);
        if (moveDir.sqrMagnitude < (minMoveThreshold * minMoveThreshold))
        {
            return;
        }

        // Desired rotation so the object's forward points along movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDir.normalized, Vector3.up);

        // Smoothly rotate toward the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Clamp01(rotationSpeed * Time.deltaTime));
    }
}
