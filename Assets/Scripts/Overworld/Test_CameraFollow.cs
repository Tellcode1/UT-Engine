using UnityEngine;

public class Test_CameraFollow : MonoBehaviour
{
    [SerializeField] float speed;
    Transform player;
    void Start() => player = GameObject.FindWithTag("Player").transform;
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(0, 0, -10), Vector3.Distance(transform.position, player.position) / speed);
    }
}
