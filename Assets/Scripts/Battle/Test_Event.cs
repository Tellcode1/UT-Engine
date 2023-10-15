using UnityEngine;
using UnityEngine.Events;

public struct Event
{
    public UnityAction Spawn;
    public UnityAction Despawn;
    public UnityAction MoveTowards;
    public UnityAction RotateTowards;
}

public struct BulletData
{
    [Tooltip("Time measured from the start of the turn. 0 means it'll instantly spawn after the turn starts")]
    public float spawnTime;
}
