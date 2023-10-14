using UnityEditor;
using UnityEngine;

public class UNUSED_AttackSpawner : MonoBehaviour
{
    [HideInInspector] public GameObject[] Attacks;
    [HideInInspector] public float[] AttackTime;
    [SerializeField] int poolSize;
    private void Start()
    {
        Attacks = new GameObject[100]; AttackTime = new float[100];
        for (int i = 0; i < Attacks.Length; i++)
        {
            Attacks[i] = gameObject;
            AttackTime[i] = Time.time;

            print($"Appended: {Attacks}, Appended: {Time.time}");
        }
    }

    public bool PoolObjects(int poolSize)
    {

        for(int i = 0; i < poolSize; i++)
        {
            Attacks = new GameObject[poolSize];
            AttackTime = new float[poolSize];
        }
        return Attacks.Length == poolSize && AttackTime.Length == poolSize;
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects, CustomEditor(typeof(UNUSED_AttackSpawner))]
public class DrawEditor : Editor
{
    UNUSED_AttackSpawner parent;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        parent = (UNUSED_AttackSpawner)target;
        for (int i = 0; i < parent.Attacks.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25));
            EditorGUILayout.ObjectField(parent.Attacks[i], typeof(GameObject), false);
            EditorGUILayout.FloatField(parent.AttackTime[i]);
            EditorGUILayout.EndHorizontal();
        }

    }
}

#endif