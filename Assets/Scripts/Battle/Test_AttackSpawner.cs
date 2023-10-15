//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//public class BulletPooler : MonoBehaviour
//{
//    // I've rewritten this script 3 times. I am going to go haywire.
//    private List<Bullet> bulletsToPool;

//    public void Pool()
//    {
//        Bullet[] FoundBullets = FindObjectsOfType<Bullet>(true);

//        bulletsToPool = new List<Bullet>(FoundBullets.Length);

//        foreach(Bullet bullet in FoundBullets)
//        {
//            bulletsToPool.Add(bullet);
//            bullet.gameObject.SetActive(false);
//            StartCoroutine(Spawn(bullet.activationTime, bullet));
//        }
//    }

//    private IEnumerator Spawn(float time, Bullet bullet)
//    {
//        yield return new WaitForSeconds(time);
//        bullet.gameObject.SetActive(true);
//        // hot potato moment
//        StartCoroutine(Despawn(bullet.deactivationTime, bullet));
//    }

//    private IEnumerator Despawn(float time, Bullet bullet)
//    {
//        yield return new WaitForSeconds(time);
//        bullet.gameObject.SetActive(false);
//    }
//}

//#if UNITY_EDITOR
//[CanEditMultipleObjects(), CustomEditor(typeof(BulletPooler))]
//public class CustomEditorDrawer : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        bool button = GUILayout.Button("Pool Objects");

//        BulletPooler spawner = (BulletPooler)target;

//        if (button)
//            spawner.Pool();
//    }
//}
//#endif


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackSpawner : MonoBehaviour
//{
//    private List<Bullet> bullets;

//    private void Pool()
//    {
//        Bullet[] FoundBullets = FindObjectsOfType<Bullet>(true);
//        bullets = new List<Bullet>(FoundBullets.Length);

//        foreach(Bullet bullet in FoundBullets)
//        {
//            bullets.Add(bullet);
//            StartCoroutine(Spawn(bullet.activationTime, bullet));
//        }
//    }

//    private IEnumerator Spawn(float time, Bullet bullet)
//    {
//        yield return new WaitForSeconds(time);
//        bullet.gameObject.SetActive(true);
//        // Place the object on dea#@ row.
//        StartCoroutine(Despawn(bullet.deactivationTime, bullet));
//    }

//    private IEnumerator Despawn(float time, Bullet bullet)
//    {
//        yield return new WaitForSeconds(time);
//        bullet.gameObject.SetActive(false);
//    }
//}

using BattleSystems;
using System.Collections;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    [SerializeField] BulletPacker_SObject pack;

    private void Start()
    {
        FightManager.FManager.OnTurnEnter += SpawnPack;
    }

    private void SpawnPack()
    {
        Bullet[] bulletpack = FindObjectsOfType<Bullet>();
        SpawnPack(pack, bulletpack);
    }

    public void SpawnPack(BulletPacker_SObject self, Bullet[] objects)
    {
        foreach (Bullet bullet in objects)
        {
            bullet.gameObject.SetActive(false);
            StartCoroutine(Spawn(self, bullet));
        }
    }
    private IEnumerator Spawn(BulletPacker_SObject self, Bullet bullet)
    {
        // The FManager's turn time will be 5 seconds while the bullet's required time will be 7.5 seconds or something.
        // FManager.enemyTurnTime > bullet.activationTime, Right? But we only need to do this once... 
        // Maybe it will only execute once regardless. Hmm.
        if (FightManager.FManager.turn != self.turn) throw new System.Exception("Tried to call a pack with a different turn value than the FightManager's.");
            yield return new WaitUntil(() => FightManager.FManager.enemyTurnTime >= bullet.activationTime);
        bullet.gameObject.SetActive(true);
        // Place the object on dea#@ row.
        StartCoroutine(Despawn(bullet.deactivationTime, bullet));
    }

    private IEnumerator Despawn(float time, Bullet bullet)
    {
        yield return new WaitForSeconds(time);
        bullet.gameObject.SetActive(false);
    }

}