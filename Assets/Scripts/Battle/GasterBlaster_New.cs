using System.Collections;
using UnityEditor;
using UnityEngine;

public class GasterBlaster_New : Bullet
{
    [HideInInspector]
    private Rigidbody2D rb;

    [HideInInspector]
    public float approachTime;

    [HideInInspector]
    public Vector2 targetPosition = Vector2.zero;

    [HideInInspector]
    public Vector2 targetObject = Vector2.zero;

    [HideInInspector]
    public Vector2 spawnPos = Vector2.zero;

    [HideInInspector]
    public int thrustFrames = 45;

    [HideInInspector]
    public float timeToShoot;

    [HideInInspector]
    public bool aimAtPlayer;

    private void Update()
    {
        HandleBounds();
        if (!active)
            Initialize();
    }

    private void OnEnable()
    {
        onReachedTarget += FlyOff;
        onBoundsExit += Deinitialize;
    }

    private void OnDisable()
    {
        Deinitialize();
        onReachedTarget -= FlyOff;
        onBoundsExit -= Deinitialize;
    }

    private void Deinitialize()
    {
        transform.position = spawnPos;
        active = false;
        rb.velocity = Vector2.zero;
        StopAllCoroutines();
    }

    public void Initialize()
    {
        if (active) throw new System.Exception("Object is already active!");
        
        active = true;
        
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = Vector2.zero;

        // Debugging
        targetPosition = Random.insideUnitCircle * 5;
        //end Debugging

        // If aim at player is set to true, then feed in the player's position as the target object.
        // Otherwise pass in targetObject as serialized in the inspector.
        if (aimAtPlayer) base.MoveAndRotateTo(targetPosition, targetPosition, Player_Battle.Player.player.transform.position, approachTime);
        else base.MoveAndRotateTo(targetPosition, targetPosition, targetObject, approachTime);

        transform.position = spawnPos;
    }
    Coroutine helper;
    private void FlyOff()
    {
        if (helper != null)
            StopCoroutine(helper);
        helper = StartCoroutine(FlyOff_Helper());

        IEnumerator FlyOff_Helper()
        {
            yield return new WaitForSeconds(timeToShoot);
            for (int i = 0; i < thrustFrames; i++)
            {
                rb.AddForce(-transform.up * i);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

[CustomEditor(typeof(GasterBlaster_New)), CanEditMultipleObjects]
public class NewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUIContent content = new();

        DrawDefaultInspector();
        
        GasterBlaster_New parent = (GasterBlaster_New)target;

        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space(3f);
        EditorGUILayout.LabelField("Movement And Follow", EditorStyles.boldLabel);
        EditorGUILayout.Space(1.5f);

        EditorGUILayout.BeginHorizontal();
        content.text = "Approach Time";
        content.tooltip = "How fast the blaster reaches the target position and rotation. Measured in seconds, though it may not be accurate at all.";
        parent.approachTime = EditorGUILayout.FloatField(content, parent.approachTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        content.tooltip = "Where the blaster will move to.";
        content.text = "Target position";
        parent.targetPosition = EditorGUILayout.Vector2Field(content, parent.targetPosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        content.tooltip = "The object that the blaster will aim at.";
        content.text = "Target Object";
        parent.targetObject = EditorGUILayout.Vector2Field(content, parent.targetObject);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        content.tooltip = "Where in world space the blaster spawns. May be used for animations and such.";
        content.text = "Origin";
        parent.spawnPos = EditorGUILayout.Vector2Field(content, parent.spawnPos);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(3f);
        EditorGUILayout.LabelField("Shooting and flying.", EditorStyles.boldLabel);
        EditorGUILayout.Space(1.5f);

        EditorGUILayout.BeginHorizontal();
        content.tooltip = "The amount of frames in which the upwards force will be applied to the blaster after shooting.";
        content.text = "Thrust Frames";
        parent.thrustFrames = EditorGUILayout.IntField(content, parent.thrustFrames);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        content.tooltip = "The time intervening between after arriving and before shooting. Measured in seconds.";
        content.text = "Time to shoot";
        parent.timeToShoot = EditorGUILayout.FloatField(content, parent.timeToShoot);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        content.tooltip = "Whether to shoot directly at the player or a position set in the editor.";
        content.text = "To aim at player?";
        EditorGUILayout.Toggle(content, parent.aimAtPlayer);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}
