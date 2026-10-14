using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3.5f;
    // stopDistance measured in XZ plane
    public float stopDistance = 0f;

    // If true movement is applied in LateUpdate (after other transform changes).
    // Helps when other scripts change transforms earlier in the frame.
    public bool useLateUpdate = true;

    void Start()
    {
        if (player == null)
        {
            var found = GameObject.FindWithTag("Player");
            if (found != null)
            {
                player = found.transform;
            }
        }
    }

    void Update()
    {
        if (!useLateUpdate)
        {
            DoMove();
        }
    }

    void LateUpdate()
    {
        if (useLateUpdate)
        {
            DoMove();
        }
    }

    private void DoMove()
    {
        if (player == null)
        {
            return;
        }

        Vector3 current = transform.position;
        Vector3 target = player.position;

        // Only consider X and Z for movement; preserve current Y
        float dx = target.x - current.x;
        float dz = target.z - current.z;
        float distanceXZ = new Vector2(dx, dz).magnitude;

        if (distanceXZ <= stopDistance || distanceXZ <= Mathf.Epsilon)
        {
            return;
        }

        // Move along the XZ plane using a single vector so diagonal movement has the same magnitude
        // as straight movement. Vector3.MoveTowards will clamp the movement to moveStep and avoid overshoot.
        float moveStep = speed * Time.deltaTime;
        Vector3 currentXZ = new Vector3(current.x, 0f, current.z);
        Vector3 targetXZ = new Vector3(target.x, 0f, target.z);
        Vector3 newXZ = Vector3.MoveTowards(currentXZ, targetXZ, moveStep);

        transform.position = new Vector3(newXZ.x, current.y, newXZ.z);
    }
}
