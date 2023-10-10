using FightHandler;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Battle_Dialogue : MonoBehaviour
{
    Text dialogueHolder;
    
    [System.Serializable]
    struct Dialogue
    {
        public string line;
        public float speed;
        public int textSize;
    }

    [SerializeField] Dialogue[] dialoguePool;

    private readonly Vector2 convertToCanvas = new(37.5f, 37.5f);

    BattleBoxHandler bboxHandler;

    private void Start()
    {
        dialogueHolder = GetComponent<Text>();
        bboxHandler = GameObject.FindWithTag("BattleBoxHandler").GetComponent<BattleBoxHandler>();
        FightManager.FManager.OnTurnEnter += EnterTurn;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) RequestDialogue();

        Vector2 boxSize = new(bboxHandler.upWall.localScale.x, bboxHandler.leftWall.localScale.x);

        dialogueHolder.rectTransform.sizeDelta = boxSize * convertToCanvas;

        if (!FightManager.FManager.IsPlayerTurn()) dialogueHolder.color = Color.clear;
        else dialogueHolder.color = Color.white;
    }

    private void EnterTurn()
    {
        RequestDialogue();
    }

    public void ClearDialogueHolder() => dialogueHolder.text = string.Empty;


    public void RequestDialogue()
    {
        int i = Random.Range(0, dialoguePool.Length);

        if (FightManager.FManager.IsPlayerTurn())
            StartCoroutine(AppendText(i));
    }  
    public void RequestDialogue(int index)
    {
        if (FightManager.FManager.IsPlayerTurn())
            StartCoroutine(AppendText(index));
    }  


    [SerializeField] private bool isRunning;
    IEnumerator AppendText(int index)
    {
        if (isRunning) yield break;
        isRunning = true;
        
        dialogueHolder.fontSize = dialoguePool[index].textSize;
        dialogueHolder.text += '*' + " ";

        for (int i = 0; i < dialoguePool[index].line.Length; i++)
        {
            dialogueHolder.text += dialoguePool[index].line[i];
            yield return new WaitForSeconds(dialoguePool[index].speed);
        }
        isRunning = false;
    }
}
