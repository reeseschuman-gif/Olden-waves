using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject player;
    private Vector3 offset = new Vector3(0,10,0);
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
