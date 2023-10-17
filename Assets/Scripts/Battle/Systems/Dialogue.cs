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
            FManager.OnTurnEnter += AutoDialogue;
        }


        void Update()
        {
            // For debugging
            if (Input.GetKeyDown(KeyCode.D)) RequestDialogue();

            // gamer fps
            if (bboxHandler.isChangingSize)
            {
                Vector2 boxSize = new(bboxHandler.upWall.localScale.x, bboxHandler.leftWall.localScale.x);

                dialogueHolder.rectTransform.sizeDelta = boxSize * convertToCanvas;

            }
            // Set text color to transparent when it isn't the players turn because yes.
            // I don't want to try to find any other solutions to this.
            if (!FManager.IsPlayerTurn()) dialogueHolder.color = Color.clear;
            else dialogueHolder.color = Color.white;
        }

        private void AutoDialogue()
        {
            try
            {
                if (FManager.IsPlayerTurn())
                    StartCoroutine(AppendText(FManager.turn));
            }
            catch (System.IndexOutOfRangeException)
            {
                RequestDialogue();
            }
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


        private IEnumerator AppendText(int index)
        {
            dialogueHolder.text = string.Empty;

            dialogueHolder.fontSize = dialoguePool[index].textSize;

            // UNDERTALE.
            System.Text.StringBuilder sb = new("* ", 256);
            

            for (int i = 0; i < dialoguePool[index].line.Length; i++)
            {
                //dialogueHolder.text += dialoguePool[index].line[i];
                sb.Append(dialoguePool[index].line[i]);
                dialogueHolder.text = sb.ToString();
                yield return new WaitForSeconds(dialoguePool[index].speed);
            }
        }

        private IEnumerator AppendText(string text)
        {
            dialogueHolder.text = string.Empty;
            dialogueHolder.fontSize = 25;
            dialogueHolder.text += "* ";
            for (int i = 0; i < text.Length; i++)
            {
                dialogueHolder.text += text[i];
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void StopDialogue()
        {
            // Directly question the coroutine because it can be broken unlike the bboxHandler's coroutine.
            if(appendText != null)
            {
                StopCoroutine(appendText);
                dialogueHolder.text = string.Empty;
            }
        }

        public void ParseText(string text)
        {
            StopDialogue();
            StartCoroutine(AppendText(text));
        }
    }
}
