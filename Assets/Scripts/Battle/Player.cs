using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static InputManager;

namespace Player_Battle
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]

    public class Player : MonoBehaviour
    {
        #region Variable declaration
        public static Player player { get; private set; }

        private Rigidbody2D rb;
        private SpriteRenderer spr;


        [System.Serializable]
        private struct Data
        {
            [SerializeField] private int _hp;
            public int Hp
            {
                readonly get
                {
                    return _hp;
                }
                set
                {
                    _hp = Mathf.Clamp(value, 0, maxHp);
                }
            }
            public int maxHp;

            [Min(1)] public int ATK;
            [Min(0)] public int DEF;
        }

        [SerializeField] private Data dataFile;



        #endregion

        private void Start()
        {
            // Initialize self as static
            if (player != null && player != this)
            {
                Destroy(gameObject);
                return;
            }
            player = this;

            // Stupid rb goes to sleep and I can't find a way to stop it other than the editor.
            rb = GetComponent<Rigidbody2D>();
            spr = GetComponent<SpriteRenderer>();

            Transform healthParent = GameObject.Find("Healthbar").transform;
            healthbar = healthParent.GetComponent<Slider>();

            healthText = healthParent.Find("HealthText").GetComponent<Text>();

            InitializeHealth();
        }

        private void Update()
        {
            // For debugging
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(1);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Invincibility());
            }
        }

        void FixedUpdate() =>
            Move();


        public bool canMove;
        [SerializeField] private float speed;
        private void Move()
        {
            if (canMove) rb.velocity = inputMgr.GetWASD().normalized * speed;
            else rb.velocity = Vector3.zero;
        }


        [SerializeField] private bool canTakeDamage = true;
        Coroutine invin;
        public void TakeDamage(int damage)
        {
            if (canTakeDamage)
            {
                dataFile.Hp -= damage;
                RepaintHealthbar();
                
                if (invin != null) 
                    StopCoroutine(invin);
                
                invin = StartCoroutine(Invincibility());
            }
        }

        public void TakeDamage_NoIFrames(int damage)
        {
            dataFile.Hp -= damage;
            RepaintHealthbar();
        }

        public void Heal(int points)
        {
            dataFile.Hp += points;
            RepaintHealthbar();
        }

        private Slider healthbar;
        private Text healthText;
        private void InitializeHealth()
        {
            healthbar.value = dataFile.Hp;
            healthbar.maxValue = dataFile.maxHp;
            healthbar.minValue = 0;
            healthText.text = (dataFile.Hp + "/" + healthbar.maxValue).ToString();
        }

        public void RepaintHealthbar()
        {
            healthbar.value = dataFile.Hp;
            healthbar.maxValue = dataFile.maxHp;
            healthText.text = (dataFile.Hp + "/" + healthbar.maxValue).ToString();
        }

        [SerializeField] private Color defaultColor = new(255, 0, 0);
        [SerializeField] private Color damagedColor = new(150, 0, 0);
        [SerializeField, Tooltip("( Frames are calculated in FixedUpdate() )")] private int IFrames;

        private IEnumerator Invincibility()
        {
            canTakeDamage = false;

            Coroutine animate_Sprite = StartCoroutine(Invicibility_AnimateSprite());

            for (int i = 0; i < IFrames; i++)
                yield return new WaitForFixedUpdate();

            StopCoroutine(animate_Sprite);

            // Reset sprite color to avoid stupid shenanigans.
            spr.color = defaultColor;

            canTakeDamage = true;
        }

        private IEnumerator Invicibility_AnimateSprite()
        {
            // While true is true do this
            do
            {
                spr.color = damagedColor;
                yield return new WaitForSeconds(.125f);
                spr.color = defaultColor;
                yield return new WaitForSeconds(.125f);
            }
            while (true);
        }

    }
}

[CanEditMultipleObjects, CustomEditor(typeof(Player_Battle.Player))]
public class newEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("Movement");

    }
}
