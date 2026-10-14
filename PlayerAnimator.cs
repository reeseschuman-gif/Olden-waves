using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator AnimatorComponent;
    public string IsRunningParameter = "isRunning";
    public float MovementThreshold = 0.01f;

    public bool IsRunning = false;

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
        if (AnimatorComponent == null)
        {
            AnimatorComponent = GetComponent<Animator>();
        }

        if (AnimatorComponent == null)
        {
            Debug.LogWarning($"{name}: No Animator found on this GameObject. Please attach one or assign it in the inspector.");
        }
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float sqrDist = (currentPosition - previousPosition).sqrMagnitude;
        float thresholdSqr = MovementThreshold * MovementThreshold;

        if (sqrDist > thresholdSqr)
        {
            // moving -> explicitly set true
            IsRunning = true;
            if (AnimatorComponent != null)
            {
                AnimatorComponent.SetBool(IsRunningParameter, true);
                AnimatorComponent.Update(0f); // force animator to evaluate immediately
            }
        }
        else
        {
            // stopped -> explicitly set false (always executed when not moving)
            IsRunning = false;
            if (AnimatorComponent != null)
            {
                AnimatorComponent.SetBool(IsRunningParameter, false);
                AnimatorComponent.Update(0f); // force animator to evaluate immediately
            }
        }

        previousPosition = currentPosition;
    }
}
