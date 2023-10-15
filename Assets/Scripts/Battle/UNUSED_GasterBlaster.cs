using System.Collections;
using UnityEngine;

// IS THIS BASED ON THE F))(ING ANIMATOR????
public class UNUSED_GasterBlaster : MonoBehaviour
{
    private Vector2 playerPos;
    /// <summary>
    /// The Position the blaster will move to
    /// </summary>
    private Vector2 moveToPos;
    private bool toApproach = true;

    private struct Stats
    {
        public float approachSpeed;
        public float timeTillFire;
        public int flySpeed;
        public float lifetime;
    };
    private Stats stats = new()
    { 
        approachSpeed = 0.15f, 
        flySpeed = 40 
    };
    private Vector3 velo;
    [SerializeField] AudioClip charge;
    [SerializeField] AudioClip shoot;
    private IEnumerator Start()
    {
        // Play charge clip

        playerPos = Player_Battle.Player.player.transform.position;

        moveToPos = playerPos + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        yield return new WaitForSeconds(stats.lifetime);
        Destroy(gameObject);
    }

    private void FixedUpdate() // Why is this in Update? We don't wanna do this every frame right? Only in the start right???
        // Oh wait... It gets a position near the player at the start and it just lerps to that... Sorry!
    {
        Approach();
    }

    private void Approach()
    {
        if (toApproach)
            transform.SetPositionAndRotation
                (
                    Vector3.SmoothDamp(transform.position, moveToPos, ref velo, stats.approachSpeed),
                    Quaternion.LookRotation(transform.forward, transform.position - (Vector3)playerPos)
                );
    }


    private void Fire()
    {
        var beam = transform.Find("Beam");
        StartCoroutine(AnimateBeam(beam));
    }

    private IEnumerator AnimateBeam(Transform beam)
    {
        while (true)
        {
            float GodForgiveMeForWhatIAmAboutToDo = Mathf.Sign(Time.time * transform.localScale.x);
            Vector2 newScale = transform.localScale + new Vector3(GodForgiveMeForWhatIAmAboutToDo, 0);
            beam.transform.localScale = newScale;
            yield return new WaitForFixedUpdate();
        }
    }


    private IEnumerator Flyup()
    {
        //AudioSource.PlayOneShot(shoot);
        yield return new WaitForSeconds(.05f);
        toApproach = false;
        Rigidbody2D m_rb = GetComponent<Rigidbody2D>();
        for (; ; )
        {
            m_rb.AddForce(transform.up * stats.flySpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!toApproach)
            Player_Battle.Player.player.TakeDamage_NoIFrames(1);
        // Because this will be damaging the player every frame, the player is pretty much dead as soon as it hits.
    }
}
