using UnityEngine;
using System.Collections;

public class BattleBoxHandler : MonoBehaviour
{
    Transform player;
    Transform walls;

    public Transform upWall { get; private set; }
    public Transform downWall { get; private set; }
    public Transform leftWall {  get; private set; }
    public Transform rightWall { get; private set; }

    [HideInInspector] public Vector3 upBottomScale { get; set ; }
    [HideInInspector] public Vector3 leftRightScale { get ; set ; }

    [SerializeField] 
    Vector2 BattleBoxPosition;
    
    [SerializeField] 
    float changeSpeed = 12500;

    [SerializeField]
    private bool isChangingSize;

    public readonly Vector2 defaultHeight = new(3, .2f);
    public readonly Vector2 defaultWidth = new(3, .2f);
    public readonly Vector2 dialogueWidth = new(15, .2f);
    public readonly Vector2 dialogueHeight = new(4, .2f);
    const float dWidth = .2f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        walls = GameObject.FindWithTag("Wall").transform;

        upWall = walls.GetChild(0);
        downWall = walls.GetChild(1);
        leftWall = walls.GetChild(2);
        rightWall = walls.GetChild(3);
    }

    private void Update()
    {
        if (upWall.localScale != upBottomScale || leftWall.localScale != leftRightScale)
        {
            walls.position = BattleBoxPosition;
        }
    }

    /// <param name="UpBottomScale"> The scale for the up and bottom scales. (Controls the width of the box)</param>
    /// <param name="LeftRightScale"> The scale for the left and right scales. (Controls the height of the box)</param>
    public IEnumerator ChangeSize(Vector3 UpBottomScale, Vector3 LeftRightScale)
    {
        if (isChangingSize) yield break;
        isChangingSize = true;
        float ep = changeSpeed / 25000;
        float sqrChangeSpeed = changeSpeed * changeSpeed;


        while ((Vector2)upWall.localScale != (Vector2)UpBottomScale || (Vector2)leftWall.localScale != (Vector2)LeftRightScale)
        {
            upWall.localScale = downWall.localScale = Vector2.MoveTowards(upWall.localScale, UpBottomScale, ep + (Vector3.SqrMagnitude(upWall.localScale - UpBottomScale) / sqrChangeSpeed));
            leftWall.localScale = rightWall.localScale = Vector2.MoveTowards(leftWall.localScale, LeftRightScale, ep + (Vector3.SqrMagnitude(leftWall.localScale - LeftRightScale) / sqrChangeSpeed));

            AdjustPositions();

            yield return new WaitForFixedUpdate();
        }

        AdjustPositions();
        isChangingSize = false;
    }
    
    /// /// <summary>
    /// At first checks whether the handler has uniformScaling enabled and sets the left right and up down scales to the same value to ensure uniformity
    /// Or sets them to the editor set upBottomScales and the leftRightScale's respectively. Then Vector3.MoveTowards() them;
    /// OVERLOAD: This method does not take a bool and instead it defaults it to scaleUniformly
    /// </summary>
    /// <param name="UpBottomScale"> The scale for the up and bottom scales. (Controls the width of the box)</param>
    /// <param name="LeftRightScale"> The scale for the left and right scales. (Controls the height of the box)</param>
    public IEnumerator ChangeSize(float UpBottomScale, float LeftRightScale)
    {
        if (isChangingSize) yield break;
        isChangingSize = true;
        float ep = changeSpeed / 25000;
        float sqrChangeSpeed = changeSpeed * changeSpeed;

        while (upWall.localScale.x != UpBottomScale || leftWall.localScale.x != LeftRightScale)
        {
            upWall.localScale = downWall.localScale = new Vector2(Mathf.MoveTowards(upWall.localScale.x, UpBottomScale, ep + (Vector2.SqrMagnitude((Vector2)upBottomScale - Vector2.one * UpBottomScale) / sqrChangeSpeed)), dWidth);
            leftWall.localScale = rightWall.localScale = new Vector2(Mathf.MoveTowards(leftWall.localScale.x, LeftRightScale, ep + (Vector2.SqrMagnitude((Vector2)leftWall.localScale - Vector2.one * LeftRightScale) / sqrChangeSpeed)), dWidth);

            AdjustPositions();

            yield return new WaitForFixedUpdate();
        }

        AdjustPositions();
        isChangingSize = false;
    }


    void AdjustPositions()
    {
        Vector2 currentUpBottomScale = upWall.localScale;
        Vector2 currentLeftRightScale = leftWall.localScale;

        Vector2 UpDownPosition = new(0, (currentLeftRightScale.x - currentUpBottomScale.y) / 2);
        Vector2 LeftRightPosition = new((currentUpBottomScale.x - currentLeftRightScale.y) / 2, 0);

        upWall.localPosition = UpDownPosition; downWall.localPosition = -UpDownPosition;
        rightWall.localPosition = LeftRightPosition; leftWall.localPosition = -LeftRightPosition;

    }
}