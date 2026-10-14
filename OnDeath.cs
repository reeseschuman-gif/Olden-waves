using System.Collections;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    [Tooltip("Optional Animator to play a death animation on.")]
    [SerializeField]
    private Animator animator;

    [Tooltip("If provided the Animator will CrossFade/Play this state name. Matches animation clip name when available.")]
    [SerializeField]
    private string deathStateName = "";

    [Tooltip("If no deathStateName is provided this trigger will be sent to the Animator (if set).")]
    [SerializeField]
    private string triggerParameter = "Die";

    [Tooltip("If > 0 this value forces how long to wait before Destroy. If <= 0 the script will attempt to infer clip length or fall back to 0.5s.")]
    [SerializeField]
    private float destroyDelay = -1f;

    [Tooltip("Disable all Colliders on this GameObject and children while death animation plays.")]
    [SerializeField]
    private bool disableColliders = true;

    [Tooltip("Disable other MonoBehaviours on this GameObject (except this) while death animation plays).")]
    [SerializeField]
    private bool disableBehaviours = true;

    /// <summary>
    /// Call this to play the death animation (if any) then destroy the GameObject.
    /// Suitable to wire to HealthTracker.onDeath in the Inspector.
    /// </summary>
    public void PlayDeathAndDestroy()
    {
        StartCoroutine(PlayAndDestroyCoroutine());
    }

    private IEnumerator PlayAndDestroyCoroutine()
    {
        // Optionally disable colliders
        if (disableColliders)
        {
            foreach (var col in GetComponentsInChildren<Collider>(true))
            {
                col.enabled = false;
            }
        }

        // Optionally disable other behaviours
        MonoBehaviour[] disabledBehaviours = null;
        if (disableBehaviours)
        {
            var behaviours = GetComponents<MonoBehaviour>();
            var list = new System.Collections.Generic.List<MonoBehaviour>();
            foreach (var b in behaviours)
            {
                if (b != this && b.enabled)
                {
                    b.enabled = false;
                    list.Add(b);
                }
            }

            disabledBehaviours = list.ToArray();
        }

        // Play animation via Animator if assigned
        if (animator != null)
        {
            if (!string.IsNullOrEmpty(deathStateName))
            {
                // Try to directly play the named state
                animator.CrossFade(deathStateName, 0f);
            }
            else if (!string.IsNullOrEmpty(triggerParameter))
            {
                // Fire a trigger - requires the Animator to have a matching trigger parameter
                animator.SetTrigger(triggerParameter);
            }
        }

        // Decide how long to wait before destroying
        float wait = destroyDelay;
        if (wait <= 0f)
        {
            // Try to infer from animator clip with matching name
            if (animator != null && animator.runtimeAnimatorController != null && !string.IsNullOrEmpty(deathStateName))
            {
                foreach (var clip in animator.runtimeAnimatorController.animationClips)
                {
                    if (clip != null && clip.name == deathStateName)
                    {
                        wait = clip.length;
                        break;
                    }
                }
            }

            if (wait <= 0f)
            {
                // fallback default
                wait = 0.5f;
            }
        }

        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }
}
