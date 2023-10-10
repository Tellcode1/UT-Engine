using UnityEngine;

public class BattleEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) GameManager.mgr.ChangeScene(1);
    }
}
