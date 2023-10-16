using System.Collections;
using UnityEngine;

// TODO: This needs a lota lot of refactoring.
public class GasterBlaster : Bullet
{
    #region Variable Decleration.
    // Public fields are marked so, so that the pooler can change these values and reuse them if it likes. DO NOT CHANGE THEM!

    /// <summary>
    /// Whether the blaster is active or not. This will be used by the pooler or whatever.
    /// </summary>
    [Tooltip("Whether the blaster is active or not. This will be used by the pooler or whatever.")]
    public bool active = false;

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
    /// The time the blaster will be active for. Should generally be a little high to prevent shenanigans. Calculated after firing.
    /// </summary>
    [Tooltip("The time the blaster will be active for. Should generally be a little high to prevent shenanigans. Calculated after firing.")]
    public float lifeTime = 5f;

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

    /// <summary>
    /// The initial scale for the beam. Specifically the x component.
    /// </summary>
    [SerializeField, Tooltip("The initial scale for the beam. Specifically the x component.")] private float initialScale = 1f;

    /// <summary>
    /// Used internally. Do not modify.
    /// </summary>
    Coroutine changePosRot = null;

#endregion

    // Use on enable you stupid idiot so it can be re used for pooling.
    private void Update()
    {
        // Fpr debugging.
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Initialize();
    }

    private void ResetValues()
    {
        active = true;
        transform.position = defaultPos;
        beam.gameObject.SetActive(false);
        beam.GetComponent<SpriteRenderer>().color = Color.white;
    }


    /// <summary>
    /// To activate the blaster, you only need to initialize it. If you want custom values such as a custom position, set this beforehand.
    /// </summary>
    public void Initialize()
    {
        if (active) throw new System.Exception("Object is already active!");
        // This is why, do not change your children.
        beam = transform.GetChild(0);

        ResetValues();


        // TODO: Get the player's position only if "pos" hasn't been set in the editor.
        pos = (Vector2)Player_Battle.Player.player.transform.position;

        dirToPos = (moveToPos - pos).normalized;
        rot = Quaternion.LookRotation(transform.forward, dirToPos);

        transform.up = -dirToPos;

        //OnReadyToFire += Fire;
        StopAllCoroutines();

        Prefire();



        // Before firing, handle the job here then pass it on to fire.
        void Prefire()
        {
            if (changePosRot != null)
            {
                StopCoroutine(changePosRot);
                changePosRot = null;
            }
            changePosRot ??= StartCoroutine(HandlePositionAndRotation());
        }

        IEnumerator HandlePositionAndRotation()
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
            while (!Compare(transform.position, moveToPos) || !CompareQuaternion(transform.rotation, rot));

            Invoke(nameof(this.Fire), chargeTime);
        }


        bool Compare(Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.x - b.x) < epsilon &&
                   Mathf.Abs(a.y - b.y) < epsilon;
        }
        bool CompareQuaternion(Quaternion a, Quaternion b)
        {
            return Mathf.Abs(a.x - b.x) < epsilon &&
                   Mathf.Abs(a.y - b.y) < epsilon &&
                   Mathf.Abs(a.z - b.z) < epsilon &&
                   Mathf.Abs(a.w - b.w) < epsilon;
        }

    }

    /// <summary>
    /// Get the notification from Prefire() and then pass it on to whatever.
    /// </summary>
    private void Fire()
    {
        StartCoroutine(Fly());
        StartCoroutine(HandleBeam());
        print("Fired!!!");
    }


    private IEnumerator Fly()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float force = initialFlySpeed;

        // 60 Frames of nitro
        // INFINITY FRAMES OF NITRO YOOOOOOOOOOOOOOOOOOOOO
        // Did not work :<

        // TODO: You may wanna find a new way to do this (Fly). The current method freezes sometimes due to unknown circumstances.
        for (int i = 0; i < 60; i++)
        {
            print("Trying to fly!");
            force += initialFlySpeed;
            // This is already localised??? Somebody could've told me that???
            // ... I'm an idiot.
            // I am just too stupid for this project. Somebody get a replacement please.
            rb.AddRelativeForce(Vector2.up * force);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HandleBeam());
    }

    [SerializeField] float test;
    [SerializeField] float beam_Speed;
    [SerializeField] float beam_Amplitude;
    // TODO: Implement the beam. Add the animations later.
    private IEnumerator HandleBeam()
    {
        beam.gameObject.SetActive(true);
        SpriteRenderer spr = beam.GetComponent<SpriteRenderer>();
        spr.color = Color.white;

        Vector2 initScale = new(initialScale, beam.localScale.y);
        for (int i = 0; i < Mathf.RoundToInt(beamLifetime * 2 / 3) * 60; i++)
        {
            float GodForgiveMeForWhatIAmAboutToDo = Mathf.Sin(Time.time * beam_Speed) * beam_Amplitude;
            beam.localScale = new Vector2(initScale.x + (GodForgiveMeForWhatIAmAboutToDo / test), initScale.y);
            yield return new WaitForFixedUpdate();
        }

        float timer = 0f;
        while (spr.color.a != 0)
        {
            timer += Time.fixedDeltaTime;

            float t = timer / (beamLifetime * 1 / 3);
            //float betterT = timer / (beamLifetime * .9f);

            beam.localScale = new(Mathf.Lerp(initScale.x, initScale.x * .3f, t), initScale.y);
            
            Color col = spr.color;
            col.a = Mathf.Lerp(col.a, 0, t);
            spr.color = col;

            //spr.color = Vector4.Lerp(Color.white, Color.clear, betterT);
            yield return new WaitForFixedUpdate();
            if (t >= 1f) yield break;
        }
        beam.gameObject.SetActive(false);
        yield return new WaitForSeconds(lifeTime);
        active = false;
    }

    [SerializeField] private float initialFlySpeed = 0.01f;
}
