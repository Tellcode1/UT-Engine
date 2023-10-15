using UnityEngine;

[CreateAssetMenu(fileName = "BulletPack", menuName = "New Bullet Pack")]
public class BulletPacker_SObject : ScriptableObject
{
    [Tooltip("The pack of objects to be used. These will be used in one turn only")] public Bullet[] objectPack;
    [Tooltip("The turn in which to execute this pack.")] public int turn;

    private void OnValidate()
    {
        objectPack = FindObjectsOfType<Bullet>();
    }
}
