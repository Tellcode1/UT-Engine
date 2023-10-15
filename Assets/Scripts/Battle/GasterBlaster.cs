using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GasterBlaster : Bullet
{
    // Public fields are marked so, so that the pooler can change these values and reuse them if it likes. DO NOT CHANGE THEM!
    /// <summary>
    /// The position the blaster will aim at. Leave this at Vector2.zero if you want it to aim at the player.
    /// </summary>
    [Tooltip("The position the blaster will aim at. Leave this at Vector2.zero if you want it to aim at the player.")]
    public Vector2 pos = new();

    /// <summary>
    /// The rotation the blaster will have (where it'll be aiming). This should not be changed as it is used internally.
    /// </summary>
    private Quaternion rot = new();

    /// <summary>
    /// The direction from "moveToPos" to "pos". This should not be changed as it is used internally.
    /// </summary>
    private Vector2 dirToPos = new();

    /// <summary>
    /// The position the blaster will move to.
    /// </summary>
    [Tooltip("The position the blaster will move to.")]
    public Vector2 moveToPos = new();

    /// <summary>
    /// The default position of the blaster. The position from where the blaster will move from. (Set in Initialize).
    /// </summary>
    [Tooltip("The default position of the blaster. The position from where the blaster will move from. (Set in Initialize).")]
    public Vector2 defaultPos = new();

    /// <summary>
    /// The time the blaster charges for then shoots. Will be evaluated after the blaster has reached its target position.
    /// </summary>
    [SerializeField, Tooltip("The time the blaster charges for then shoots. Will be evaluated after the blaster has reached its target position.")]
    private float chargeTime = .5f;

    /// <summary>
    /// The time the beam will be active for. Should generally be divisible by 2 to preserve animations.
    /// </summary>
    [Tooltip("The time the beam will be active for. Should generally be divisible by 2 to preserve animations.")]
    public float beamLifetime = 1f;

    /// <summary>
    /// The time (in seconds) it takes for the blaster to approach the position specified in "moveToPos".
    /// </summary>
    [SerializeField, Tooltip("The time it takes for the blaster to approach the position specified in \"moveToPos\".")]
    private float approachTime = 1f;

    /// <summary>
    /// The beam that the blaster will fire. This will initially be disabled and enable when the blaster fires.
    /// </summary>
    private Transform beam;

    //// <summary>
    /// The time that the blaster has spent. (Calculated in FixedUpdate)
    //// </summary>
    //private float timeSinceSpawn = 0f;

    // You idiot, this won't be used elsewhere.
    private UnityAction OnReadyToFire;

    // Use on enable you stupid idiot so it can be re used for pooling.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Initialize();
    }
    public void Initialize()
    {
        transform.position = defaultPos;
        // Reset the timeSinceSpawn because I F@$$#G FORGOT TO DO SO.
        //timeSinceSpawn = 0f;

        // This is why, do not change your children.
        beam = transform.GetChild(0);

        // Get the player's position only if "pos" hasn't been set in the editor.

        pos = (Vector2)Player_Battle.Player.player.transform.position;
        
        dirToPos = (pos - moveToPos).normalized;
        rot = Quaternion.LookRotation(transform.forward, dirToPos);

        transform.up = -dirToPos;

        OnReadyToFire += Fire;

        Prefire();
    }

    private void OnDisable() =>
        OnReadyToFire -= Fire;

    //private void FixedUpdate()
    //{
    //    // Update the time since spawn for scripts.
    //    //timeSinceSpawn += Time.fixedDeltaTime;
    //}

    Coroutine changePosRot = null;
    // Before firing, handle the job here then pass it on to fire.
    private void Prefire()
    {
        if (changePosRot != null)
        {
            StopCoroutine(changePosRot);
            changePosRot = null;
        }
        changePosRot ??= StartCoroutine(HandlePositionAndRotation());
    }

    /// <summary>
    /// <br> The epsilon value for comparisons (for assessing whether the player is at the position it need be or not).</br> 
    /// <br> Raising this value may increase the time the blaster spends to get to the position. 
    /// But keeping this value a little high is recommended.</br>
    /// </summary>
    [SerializeField, Tooltip("The epsilon value for comparisons " +
        "(for assessing whether the player is at the position it need be or not)." +
        " \r\n Raising this value may increase the time the blaster spends to get to the position. \r\n " +
        "But keeping this value a little high is recommended.")]
    float epsilon;

    // Because we don't need it to be exact.
    private bool Compare(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) < epsilon &&
               Mathf.Abs(a.y - b.y) < epsilon;
    }
    private bool Compare(Quaternion a, Quaternion b)
    {
        return Mathf.Abs(a.x - b.x) < epsilon &&
               Mathf.Abs(a.y - b.y) < epsilon &&
               Mathf.Abs(a.z - b.z) < epsilon &&
               Mathf.Abs(a.w - b.w) < epsilon;
    }

    private IEnumerator HandlePositionAndRotation()
    {
        float timer = 0;

        do
        {
            timer += Time.fixedDeltaTime;

            float t = timer / approachTime;

            // This should lerp to the rotation and position at the same time right?
            // It does!
            transform.SetPositionAndRotation
                (
                    Vector3.Lerp(transform.position, moveToPos, t),
                    Quaternion.Lerp(transform.rotation, rot, t) // It's slerp? I thought it was slurp bro...
                );

            yield return new WaitForFixedUpdate();
        }
        // While current position or rotation is not equal to target.
        while (!Compare(transform.position, moveToPos) || !Compare(transform.rotation, rot));

        Invoke(nameof(Fire), chargeTime);
    }

    private void Fire()
    {
        print("Fired!!!");
        StartCoroutine(Fly());
    }

    [SerializeField] private float initialFlySpeed = 0.01f;
    private IEnumerator Fly()
    {
        float force = initialFlySpeed;

        // 60 Frames of nitro
        for (int i = 0; i < 60; i++)
        {
            force += initialFlySpeed;
            // This is already localised??? Somebody could've told me that???
            // ... I'm an idiot.
            transform.Translate(0, -force, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator HandleBeam()
    {
        yield break;
        //beam.localScale = new Vector2(transform.localScale.x, beam.transform.localScale.y);
        //float initialScale = beam.localScale.x;

        //while (true)
        //{
        //    beam.localScale = new Vector2(Mathf.Sin(timeSinceSpawn * initialScale), beam.localScale.y);
        //    yield return new WaitForSeconds(1f);
        //}
    }
}
