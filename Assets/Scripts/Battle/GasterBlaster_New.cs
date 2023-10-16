using System.Collections;
using UnityEngine;

public class GasterBlaster_New : Bullet
{
    private Rigidbody2D rb;

    [SerializeField, Tooltip("How fast the blaster reaches the target position and rotation. Measured in seconds, though it may not be accurate at all.")]
    private float approachTime;

    [SerializeField, Tooltip("Where the blaster will move to.")]
    private Vector2 targetPosition = Vector2.zero;

    [SerializeField, Tooltip("Where in world space the blaster spawns. May be used for animations and such.")]
    private Vector2 spawnPos = Vector2.zero;

    [SerializeField, Tooltip("The amount of frames in which the upwards force will be applied to the blaster after shooting.")]
    private int flyOffFrames = 60;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Initialize();
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        
        transform.position = spawnPos;

        base.MoveAndRotateTo(targetPosition, Player_Battle.Player.player.transform.position, approachTime);

        onReachedTarget += FlyOff;
    }
    Coroutine helper;
    private void FlyOff()
    {
        if (helper != null)
            StopCoroutine(helper);
        helper = StartCoroutine(FlyOff_Helper());
    }
    IEnumerator FlyOff_Helper()
    {
        for (int i = 0; i < flyOffFrames; i++)
        {
            rb.AddForce(-transform.up * i, ForceMode2D.Force);
            //rb.AddRelativeForce(Vector2.down * i);
            //rb.velocity = -transform.up * i;
            //transform.Translate(-Vector2.up / 25 * i);
            yield return new WaitForFixedUpdate();
        }
    }
}
