using System.Collections;
using UnityEngine;

namespace BattleSystems
{
    public class BattleBoxHandler : MonoBehaviour
    {
        #region Variable Decleration
        Transform walls;

        // Only use upWall and leftWall for measurements because I said so.
        public Transform upWall { get; private set; }
        public Transform leftWall { get; private set; }

        private Transform downWall;
        private Transform rightWall;

        // The position of the battle box (useless).
        // Why do I still have this?
        [SerializeField]
        private Vector2 BattleBoxPosition;

        public bool isChangingSize;

        public readonly Vector2 dialogueWidth = new(15, .2f);
        public readonly Vector2 dialogueHeight = new(4, .2f);
        private const float defaultWidth = .2f;

        #endregion

        private void Start()
        {
            walls = GameObject.FindWithTag("Wall").transform;

            upWall = walls.GetChild(0);
            downWall = walls.GetChild(1);
            leftWall = walls.GetChild(2);
            rightWall = walls.GetChild(3);
        }


        [SerializeField] float timeToFinish;
        /// /// <summary>
        /// <br> Defaults the width to .2f or basically the set default value</br>
        /// </summary>
        /// <param name="UpBottomScale"> The scale for the up and bottom scales. (Controls the width of the box)</param>
        /// <param name="LeftRightScale"> The scale for the left and right scales. (Controls the height of the box)</param>
        private IEnumerator ChangeSize(float UpBottomScale, float LeftRightScale)
        {
            isChangingSize = true;
            //float ep = changeSpeed / 25000;
            //float sqrChangeSpeed = changeSpeed * changeSpeed;

            float timer = 0f;
            while (upWall.localScale.x != UpBottomScale || leftWall.localScale.x != LeftRightScale)
            {
                timer += Time.fixedDeltaTime;
                float t = timer / timeToFinish;

                upWall.localScale = downWall.localScale = new(Mathf.Lerp(upWall.localScale.x, UpBottomScale, t), defaultWidth);
                leftWall.localScale = rightWall.localScale = new Vector2(Mathf.Lerp(leftWall.localScale.x, LeftRightScale, t), defaultWidth);
                //upWall.localScale = downWall.localScale = new Vector2(Mathf.MoveTowards(upWall.localScale.x, UpBottomScale, ep + (Vector2.SqrMagnitude((Vector2)upWall.localScale - Vector2.one * UpBottomScale) / sqrChangeSpeed)), dWidth);
                //leftWall.localScale = rightWall.localScale = new Vector2(Mathf.MoveTowards(leftWall.localScale.x, LeftRightScale, ep + (Vector2.SqrMagnitude((Vector2)leftWall.localScale - Vector2.one * LeftRightScale) / sqrChangeSpeed)), dWidth);

                AdjustPositions();
                yield return new WaitForFixedUpdate();
            }
            AdjustPositions();
            isChangingSize = false;
        }

        private Coroutine coroutine;
        public void ParseChangeSize(float width, float height)
        {
            // Maybe the coroutine will never be null as isChangingSize will only be active if the coroutine calls it?
            // or maybe the coroutine is exited before the value may be set to false. Better be safe than sorry.

            // I mean... doesn't hurt to try, right?

            // I mean it works fine. Can't imagine a situation where this doesn't work.

            // If you don't do this the fps will die.
            if (isChangingSize)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(ChangeSize(width, height));
        }

        void AdjustPositions()
        {
            // Ah yes. Numbers. Totally makes sense.

            Vector2 currentUpBottomScale = upWall.localScale;
            Vector2 currentLeftRightScale = leftWall.localScale;

            Vector2 UpDownPosition = new(0, (currentLeftRightScale.x - currentUpBottomScale.y) / 2);
            Vector2 LeftRightPosition = new((currentUpBottomScale.x - currentLeftRightScale.y) / 2, 0);

            upWall.localPosition = UpDownPosition; downWall.localPosition = -UpDownPosition;
            rightWall.localPosition = LeftRightPosition; leftWall.localPosition = -LeftRightPosition;

        }
    }
}