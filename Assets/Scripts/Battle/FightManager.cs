using Player_Battle;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FightHandler
{
    public class FightManager : MonoBehaviour
    {
        public static FightManager FManager { get; private set; }
        public UnityAction OnTurnEnter;

        private Transform player;
        private Player_Battle.Player playerMovement;
        private Battle_Dialogue dialogue;
        private BattleBoxHandler boxHandler;
        private ButtonHandler[] btnHandlers;

        public enum PossibleFightStates
        {
            PlayerTurn,
            EnemyTurn,
        };

        // i = 0 FIGHTPos = new(-5.4f, -4.4f);
        // i = 1 ACTPos = new(-2.4f, -4.4f);
        // i = 2 ITEMPos = new(.6f, -4.4f);
        // i = 3 MERCYPos = new(3.6f, -4.4f);
        //defaultPos = new(0, -1);
        private readonly Vector2 defaultPos = new(0, -1f);
        private readonly Vector2[] buttonPos = new Vector2[4] { new Vector2(-5.4f, -4.4f), new Vector2(-2.4f, -4.4f), new Vector2(.6f, -4.4f), new Vector2(3.6f, -4.4f) };

        private int playerOnID;
        public int turn;

        public PossibleFightStates currentState = new();

        [System.Serializable]
        public struct FightData
        {
            [Range(1, 100)] public float enemyTurnTime;
        }
        [SerializeField] FightData fightData = new() { enemyTurnTime = 5f };


        private void Start()
        {
            // Declare singleton
            if (FManager != null && FManager != this) Destroy(gameObject);
            else FManager = this;

            player = GameObject.Find("Player").transform;
            playerMovement = player.GetComponent<Player_Battle.Player>();

            boxHandler = GameObject.FindWithTag("BattleBoxHandler").GetComponent<BattleBoxHandler>();
            dialogue = GameObject.FindWithTag("DialogueHolder").GetComponent<Battle_Dialogue>();


            Transform buttonParent = GameObject.Find("Buttons").transform;

            btnHandlers = new ButtonHandler[4] 
            {
                buttonParent.GetChild(0).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(1).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(2).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(3).GetComponent<ButtonHandler>(),
            };

            foreach(ButtonHandler handler in btnHandlers)
            {
                handler.Initialize();
            }
            playerOnID = 0;
            RefreshButtons();

            OnTurnEnter += RefreshButtons;
            OnTurnEnter += dialogue.ClearDialogueHolder;
        }

        public bool IsPlayerTurn() => currentState == PossibleFightStates.PlayerTurn;
        public bool IsInteracting(int id) => playerOnID == id;

        void Update()
        {
            if (currentState == PossibleFightStates.PlayerTurn) Debug();
        }

        private bool running;
        private IEnumerator EnterEnemyAttack()
        {
            if (running) yield break;

            EnterEnemyTurn();
            yield return new WaitForSeconds(fightData.enemyTurnTime);
            
            EnterPlayerTurn();
        }

        private void EnterPlayerTurn()
        {
            playerMovement.canMove = false;
            
            StartCoroutine(boxHandler.ChangeSize(boxHandler.dialogueWidth.x, boxHandler.dialogueHeight.x));
            currentState = PossibleFightStates.PlayerTurn;

            player.position = buttonPos[0];
            
            
            RefreshButtons();

            running = false;
            OnTurnEnter();
            turn++;
        }

        private void EnterEnemyTurn()
        {
            running = true;
            StartCoroutine(boxHandler.ChangeSize(4, 4));
            currentState = PossibleFightStates.EnemyTurn;
            playerMovement.canMove = true;
        }

        private void Debug()
        {
            
            if (InputManager.inputMgr.GetLeftArrowKey())
            {
                playerOnID--; if (playerOnID < 0) playerOnID = 3;
                RefreshButtons();
            }
            
            if (InputManager.inputMgr.GetRightArrowKey())
            {
                playerOnID++; if (playerOnID > 3) playerOnID = 0;
                RefreshButtons();
            }
        }

        private void RefreshButtons()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == playerOnID)
                {
                    player.position = buttonPos[i];
                    btnHandlers[i].EnterInteraction();
                }
                else btnHandlers[i].LeaveInteraction();
            }
        }


        public void CallFIGHT()
        {
            print("FIGHT!!!");
            player.position = defaultPos;
            StartCoroutine(EnterEnemyAttack());
        }

        public void CallACT()
        {
            print("ACT!!!");
            player.position = defaultPos;
            StartCoroutine(EnterEnemyAttack());
        }

        public void CallITEM()
        {
            print("ITEM!!!");
            player.position = defaultPos;
            StartCoroutine(EnterEnemyAttack());
        }

        public void CallMERCY()
        {
            print("MERCY!!!");
            player.position = defaultPos;
            StartCoroutine(EnterEnemyAttack());
        }
    }
}
