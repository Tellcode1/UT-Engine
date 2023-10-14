using System.Collections;
using TMPro;
using UnityEngine;
using static BattleSystems.FightManager;

namespace BattleSystems
{
    public class Dialogue : MonoBehaviour
    {
        private TextMeshProUGUI dialogueHolder;

        [System.Serializable]
        private struct DialogueBase
        {
            public string line;
            public float speed;
            public int textSize;
        }

        [SerializeField] DialogueBase[] dialoguePool;

        private readonly Vector2 convertToCanvas = new(37.5f, 37.5f);

        private BattleBoxHandler bboxHandler;

        private Coroutine appendText;

        public void Initialize()
        {
            dialogueHolder = GetComponent<TextMeshProUGUI>();
            bboxHandler = GameObject.FindWithTag("BattleBoxHandler").GetComponent<BattleBoxHandler>();

            FManager.OnTurnEnter += StopDialogue;
            FManager.OnTurnEnter += RequestDialogue;
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D)) RequestDialogue();

            Vector2 boxSize = new(bboxHandler.upWall.localScale.x, bboxHandler.leftWall.localScale.x);

            dialogueHolder.rectTransform.sizeDelta = boxSize * convertToCanvas;

            if (!FManager.IsPlayerTurn()) dialogueHolder.color = Color.clear;
            else dialogueHolder.color = Color.white;
        }
        int i;
        public void RequestDialogue()
        {
            i = Random.Range(0, dialoguePool.Length);
            StopDialogue();

            if (FManager.IsPlayerTurn())
                appendText = StartCoroutine(AppendText(i));
        }
        public void RequestDialogue(int index)
        {
            if (FManager.IsPlayerTurn())
                appendText = StartCoroutine(AppendText(index));
        }


        IEnumerator AppendText(int index)
        {
            dialogueHolder.text = string.Empty;

            dialogueHolder.fontSize = dialoguePool[index].textSize;
            dialogueHolder.text += '*' + " ";

            for (int i = 0; i < dialoguePool[index].line.Length; i++)
            {
                dialogueHolder.text += dialoguePool[index].line[i];
                yield return new WaitForSeconds(dialoguePool[index].speed);
            }
        }

        private void StopDialogue()
        {
            if (appendText != null)
            {
                StopCoroutine(appendText);
                dialogueHolder.text = string.Empty;
            }
        }
    }
}
