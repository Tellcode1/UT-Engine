using UnityEngine;

public class Test_FollowPlayer : Bullet
{
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player_Battle.Player.player.transform.position, .025f);
        transform.up = Player_Battle.Player.player.transform.position - transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SendDamage(1);
    }
}
