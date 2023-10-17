using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Player_Battle.Player;

public abstract class Bullet : MonoBehaviour
{
    protected bool active = false;
    protected UnityAction onBoundsExit;

    protected bool toDestroyOnHit;

    protected UnityAction onReachedTarget;

    [SerializeField] private float epsilon;

    [SerializeField] private Vector2 bounds = new(20, 20);

    /// <summary>
    /// Implement this into OnCollisionEnter or OnCollisionStay depending on the needs.
    /// </summary>
    /// <param name="damage">
    /// <br>The damage sent to the player.</br> 
    /// <br>Do I really have to tell you that buddy? </br>
    /// </param>
    protected void SendDamage(int damage, Rigidbody2D collider)
    {
        collider.WakeUp();
        player.TakeDamage(damage);
        if (toDestroyOnHit)
            Destroy(gameObject);
    }

    protected void HandleBounds()
    {
        if (Mathf.Abs(transform.position.x) > bounds.x || Mathf.Abs(transform.position.y) > bounds.y)
        {
            active = false;
            onBoundsExit();
        }
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

    /// <summary>
    /// A method to return the direction to an object as a Quaternion.
    /// </summary>
    /// <param name="targetObject">The object to face.</param>
    /// <returns>Direction To the object</returns>
    protected Quaternion GetDirectionTo(Vector2 targetObject) => Quaternion.LookRotation(Vector3.forward, targetObject);


    Coroutine moveAndRotateTo;
    /// <summary>
    /// You don't need to know how this works. It just does. Okay?
    /// </summary>
    /// <param name="targetPosition">The position the blaster will move to.</param>
    /// <param name="targetObject">The position the blaster will face.</param>
    /// <param name="duration">How long it'll take the blaster to reach both the position and rotation.</param>
    protected void MoveAndRotateTo(Vector2 targetPosition, Vector2 targetObject, float duration)
    {
        if (moveAndRotateTo != null)
            StopCoroutine(moveAndRotateTo);

        Quaternion finalRotation = GetDirectionTo(targetObject);
        transform.rotation = GetDirectionTo(-targetObject);

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
                if (t >= 1 - epsilon)
                {
                    onReachedTarget();
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
   }

    Coroutine moveAndRotateTo2;
    /// <summary>
    /// You don't need to know how this works. It just does. Okay?
    /// </summary>
    /// <param name="targetPosition">The position the blaster will move to.</param>
    /// <param name="targetObject">The position the blaster will face.</param>
    /// <param name="duration">How long it'll take the blaster to reach both the position and rotation.</param>
    protected void MoveAndRotateTo(Vector2 from, Vector2 targetPosition, Vector2 targetObject, float duration)
    {
        if (moveAndRotateTo2 != null)
            StopCoroutine(moveAndRotateTo2);

        Quaternion finalRotation = GetDirectionTo(targetObject - from);
        transform.rotation = GetDirectionTo(-targetObject - from);

        float timer = 0f;
        moveAndRotateTo2 = StartCoroutine(enumerator2());
        IEnumerator enumerator2()
        {
            //while (!Compare((Vector2)transform.position, targetPosition) || !Compare(transform.rotation, finalRotation))
            while (true)
            {
                timer += Time.fixedDeltaTime;
                float t = timer / duration;
                transform.SetPositionAndRotation
                    (
                        Vector2.Lerp((Vector2)transform.position, targetPosition, t),
                        Quaternion.Slerp(transform.rotation, finalRotation, t)
                    );
                // YOU FU#$G IDIOT. WHY AM I SO STUPIDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
                if ((Vector2)transform.position == targetPosition && transform.rotation == finalRotation)
                {
                    onReachedTarget();
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
