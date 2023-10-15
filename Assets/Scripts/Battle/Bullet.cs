using System.Runtime.CompilerServices;
using UnityEngine;
using static Player_Battle.Player;
using static UnityEngine.GraphicsBuffer;

public abstract class Bullet : MonoBehaviour
{
    protected Vector2 initialPosition;
    public bool initalized;
    public float activationTime;
    public float deactivationTime;

    protected bool toDestroyOnHit;
    
    
    /// <summary>
    /// Implement this into OnCollisionEnter or OnCollisionStay depending on the needs.
    /// </summary>
    /// <param name="damage">
    /// <br>The damage sent to the player.</br> 
    /// <br>Do I really have to tell you that buddy? </br>
    /// </param>
    protected void SendDamage(int damage)
    {
        player.TakeDamage(damage);
        if (toDestroyOnHit)
            Destroy(gameObject);
    }

    /// <summary>
    /// Basically SendDamage() but does not trigger the player's IFrames.
    /// </summary>
    /// <param name="damage">You know what this is buddy.</param>
    protected void SendDamage_NoIFrames(int damage)
    {
        player.TakeDamage_NoIFrames(damage);
    }

    // Failed idea ;C

    //private Vector2 lerpedVector;
    //protected Vector2 MoveTowards(Vector2 start, Vector2 target, float duration)
    //{
    //    //StartCoroutine(MoveTowards_Helper(start, target, duration));
    //    return lerpedVector;
    //}
    //protected System.Collections.IEnumerator MoveTowards(Vector2 start, Vector2 target, float duration)
    //{
    //    float timer = 0;
    //    do
    //    {
    //        timer += Time.fixedDeltaTime;
    //        float t = timer / duration;

    //        lerpedVector = Vector2.Lerp(start, target, t);
    //        yield return lerpedVector;
    //        yield return new WaitForFixedUpdate();
    //    }
    //    while (start != target);
    //}
}
