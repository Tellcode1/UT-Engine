using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Player_Battle.Player;

public abstract class Bullet : MonoBehaviour
{
    protected Vector2 initialPosition;
    public bool initalized;
    public float activationTime;
    public float deactivationTime;

    protected bool toDestroyOnHit;

    protected UnityAction onReachedTarget;
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

    private Coroutine moveTo;
    protected void MoveTo(Vector2 to, float duration)
    {
        if (moveTo != null)
            StopCoroutine(moveTo);

        float timer = 0f;
        moveTo = StartCoroutine(enumerator());

        IEnumerator enumerator()
        {
            while (true)
            {
                timer += Time.fixedDeltaTime;
                float t = timer / duration;
                transform.position = Vector2.Lerp(transform.position, to, t);
                if (t >= 1) yield break;
                yield return new WaitForFixedUpdate();
            }
        }
    }
    private Coroutine rotateTo;
    protected void RotateTo(Vector2 to, float duration)
    {
        if (rotateTo != null)
            StopCoroutine(rotateTo);
        Vector2 dir = ((Vector2)transform.position - to).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, dir);

        float timer = 0f;
        rotateTo = StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            while (true)
            {
                timer += Time.fixedDeltaTime;
                float t = timer / duration;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
                if (t >= 1) yield break;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    Coroutine moveAndRotateTo;
    /// <summary>
    /// You don't need to know how this works. It just does. Okay?
    /// </summary>
    /// <param name="targetPosition">The position the blaster will move to.</param>
    /// <param name="targetRotation">The position the blaster will face.</param>
    /// <param name="duration">How long it'll take the blaster to reach both the position and rotation.</param>
    protected void MoveAndRotateTo(Vector2 targetPosition, Vector2 targetRotation, float duration)
    {
        if (moveAndRotateTo != null)
            StopCoroutine(moveAndRotateTo);

        Quaternion finalRotation = Quaternion.LookRotation(transform.forward, targetRotation);

        float timer = 0f;
        moveAndRotateTo = StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            //while (!Compare((Vector2)transform.position, targetPosition) || !Compare(transform.rotation, finalRotation))
            while (true)
            {
                timer += Time.fixedDeltaTime;
                float t = timer / duration;
                print(t);
                transform.SetPositionAndRotation
                    (
                        Vector2.Lerp((Vector2)transform.position, targetPosition, t),
                        Quaternion.Slerp(transform.rotation, finalRotation, t)
                    );
                // YOU FU#$G IDIOT. WHY AM I SO STUPIDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
                if (t >= 1)
                {
                    onReachedTarget();
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }


    //bool Compare(Vector2 a, Vector2 b)
    //{
    //    //return Mathf.Abs(a.x - b.x) < epsilon &&
    //    //       Mathf.Abs(a.y - b.y) < epsilon;
    //    //return Mathf.Abs(Vector2.Distance(a, b)) < epsilon;
    //    return false;
    //}
    //bool Compare(Quaternion a, Quaternion b)
    //{
    //    return Mathf.Abs(a.x - b.x) < epsilon &&
    //           Mathf.Abs(a.y - b.y) < epsilon &&
    //           Mathf.Abs(a.z - b.z) < epsilon &&
    //           Mathf.Abs(a.w - b.w) < epsilon;
    //}

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
