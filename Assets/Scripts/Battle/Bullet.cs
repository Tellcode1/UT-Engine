using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    /// <summary>
    /// Implement this into OnCollisionEnter or OnCollisionStay depending on the needs.
    /// </summary>
    /// <param name="damage">The damage sent to the player.</param>
    public void SendDamageInfo(int damage)
    {
        Player_Battle.Player.player.TakeDamage(damage);
    }
}
