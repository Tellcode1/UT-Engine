using UnityEngine;
using static Player_Battle.Player;

public abstract class Bullet : MonoBehaviour
{
    /// <summary>
    /// Implement this into OnCollisionEnter or OnCollisionStay depending on the needs.
    /// </summary>
    /// <param name="damage">The damage sent to the player.</param>
    protected void SendDamage(int damage)
    {
        player.TakeDamage(damage);
    }

    protected void SendDamage_NoIFrames(int damage)
    {
        player.TakeDamage_NoIFrames(damage);
    }
}
