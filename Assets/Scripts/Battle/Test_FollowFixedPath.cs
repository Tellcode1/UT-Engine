using UnityEngine;

public class Test_FollowFixedPath : Bullet
{
    [SerializeField] Vector2Int direction;
    [SerializeField] float speed;

    private void OnEnable()
    {
        base.initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.Translate((Vector2)direction * speed);
    }
}
