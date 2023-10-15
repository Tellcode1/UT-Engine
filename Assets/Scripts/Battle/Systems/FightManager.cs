using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static InputManager;

namespace BattleSystems
{
    public class FightManager : MonoBehaviour
    {
        public static FightManager FManager { get; private set; }
        public UnityAction OnTurnEnter;

        private Transform player;
        private Player_Battle.Player playerMovement;
        private Dialogue dialogue;
        private BattleBoxHandler boxHandler;
        private ButtonHandler[] btnHandlers = new ButtonHandler[4];

        private bool isPlayerTurn;

        // i = 0 FIGHTPos = new(-5.4f, -4.4f);
        // i = 1 ACTPos = new(-2.4f, -4.4f);
        // i = 2 ITEMPos = new(.6f, -4.4f);
        // i = 3 MERCYPos = new(3.6f, -4.4f);
        //defaultPos = new(0, -1);
        private readonly Vector2 defaultPos = new(0, -1f);
        // Me and my homies hate the garbage collector.
        private readonly Vector2[] buttonPos = new Vector2[4] { new Vector2(-5.4f, -4.4f), new Vector2(-2.4f, -4.4f), new Vector2(.6f, -4.4f), new Vector2(3.6f, -4.4f) };

        [Range(0, 3)] private int playerOnID = 0;
        public int turn;

        UnityEngine.UI.Slider enemyHealthbar;

        enum Actions
        {
            FIGHT,
            ACT,
            ITEM,
            MERCY,
        };

        [SerializeField] private Actions action = new();
        [SerializeField] private int totalActs = 0;

        public float playerTurnTime;
        public float enemyTurnTime;

        [System.Serializable]
        public struct FightData
        {
            public string enemyName;
            public string description;
            [Min(1)] public float enemyTurnTime;
            public int actThreshold;
        }

        [SerializeField]
        FightData fightData = new()
        {
            enemyName = "sans",
            description = "The easiest enemy.",
            enemyTurnTime = 5f,
            actThreshold = 3,
        };

        private void Start()
        {
            // Declare singleton
            if (FManager != null && FManager != this) Destroy(gameObject);
            else FManager = this;

            player = GameObject.Find("Player").transform;
            playerMovement = player.GetComponent<Player_Battle.Player>();

            boxHandler = GameObject.FindWithTag("BattleBoxHandler").GetComponent<BattleBoxHandler>();

            dialogue = GameObject.FindWithTag("DialogueHolder").GetComponent<Dialogue>();

            Transform buttonParent = GameObject.Find("Buttons").transform;

            enemyHealthbar = GameObject.Find("EnemyHealthbar").GetComponent<UnityEngine.UI.Slider>();

            btnHandlers = new ButtonHandler[4]
            {
                buttonParent.GetChild(0).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(1).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(2).GetComponent<ButtonHandler>(),
                buttonParent.GetChild(3).GetComponent<ButtonHandler>(),
            };

            for (int i = 0; i < 4; i++) btnHandlers[i].Initialize();
            dialogue.Initialize();

            RepaintButtons();

            //OnTurnEnter += RepaintButtons; Don't need this anymore. Rehauled Button repainting to only happen when player moves.
            EnterPlayerTurn();
        }

        public bool IsPlayerTurn() => isPlayerTurn;
        public bool IsInteracting(int id) => playerOnID == id && IsPlayerTurn();

        void Update()
        {
            if (isPlayerTurn)
                MoveInButtons();
        }

        private void FixedUpdate()
        {
            // I love fixed update.
            //if (isPlayerTurn)
            //{
            //    playerTurnTime += Time.fixedDeltaTime;
            //    enemyTurnTime = 0;
            //}
            //else
            //{
            //    enemyTurnTime += Time.fixedDeltaTime;
            //    playerTurnTime = 0;
            //}
            playerTurnTime = isPlayerTurn ? (playerTurnTime + Time.fixedDeltaTime) : 0;
            enemyTurnTime = isPlayerTurn ? 0 : (enemyTurnTime + Time.fixedDeltaTime);
        }

        private IEnumerator EnterEnemyAttack()
        {
            EnterEnemyTurn();
            yield return new WaitForSeconds(fightData.enemyTurnTime);

            EnterPlayerTurn();
        }

        private void EnterPlayerTurn()
        {
            playerMovement.canMove = false;

            boxHandler.ParseChangeSize(15, 4);
            isPlayerTurn = true;

            player.position = buttonPos[0];

            RepaintButtons();

            OnTurnEnter();
            turn++;
        }

        private void EnterEnemyTurn()
        {
            boxHandler.ParseChangeSize(4, 4);
            isPlayerTurn = false;
            playerMovement.canMove = true;

            RepaintButtons();
        }

        private void MoveInButtons()
        {
            // Look at me I am so smart 
            if (inputMgr.GetLeftArrowKey())
            {
                playerOnID--; if (playerOnID < 0) playerOnID = 3;
                RepaintButtons();
            }

            if (inputMgr.GetRightArrowKey())
            {
                playerOnID++; if (playerOnID > 3) playerOnID = 0;
                RepaintButtons();
            }
        }

        private void RepaintButtons()
        {
            // Anti-foreach loop gang. Who's with me?
            for (int i = 0; i < 4; i++)
            {
                if (IsInteracting(i))
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
            action = Actions.FIGHT;
        }

        public void CallACT()
        {
            print("ACT!!!");
            totalActs++;
            action = Actions.ACT;
        }

        public void CallITEM()
        {
            print("ITEM!!!");
            action = Actions.ITEM;
            Player_Battle.Player.player.Heal(3);
        }

        public void CallMERCY()
        {
            print("MERCY!!!");
            action = Actions.MERCY;
            if (totalActs >= fightData.actThreshold) EndBattle();
        }

        public void Call(int id)
        {
            switch (id)
            {
                // Fight button
                case 0:
                    {
                        CallFIGHT();
                        break;
                    }
                // Act button
                case 1:
                    {
                        CallACT();
                        break;
                    }
                // Item button
                case 2:
                    {
                        CallITEM();
                        break;
                    }
                // Mercy button
                case 3:
                    {
                        CallMERCY();
                        break;
                    }
            }
            ButtonCall();
        }

        private void ButtonCall()
        {
            player.position = defaultPos;
            StartCoroutine(EnterEnemyAttack());
        }

        private void EndBattle()
        {
            print("You won! Now go and fix the 12388082038 errors");
        }
    }
}
