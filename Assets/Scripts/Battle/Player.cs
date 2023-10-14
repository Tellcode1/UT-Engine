using System.Collections;
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

        void FixedUpdate()
        {
            Move();
        }


        public bool canMove;
        [SerializeField] private float speed;
        private void Move()
        {
            if (canMove) rb.velocity = inputMgr.GetWASD().normalized * speed;
            else rb.velocity = Vector3.zero;
        }


        [SerializeField] private bool canTakeDamage = true;
        public void TakeDamage(int damage)
        {
            if (canTakeDamage)
            {
                dataFile.Hp -= damage;
                RepaintHealthbar();
                StartCoroutine(Invincibility());
            }
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

        [SerializeField] private Color defaultColor;
        [SerializeField] private Color damagedColor;
        [SerializeField, Tooltip("( Frames are calculated in FixedUpdate() )")] private int IFrames;

        private IEnumerator Invincibility()
        {
            Coroutine pipis = StartCoroutine(Invicibility_AnimateSprite());
            canTakeDamage = false;
            for (int i = 0; i < IFrames; i++)
            {
                yield return new WaitForFixedUpdate();

                //spr.color = damagedColor;
                //yield return new WaitForSeconds(.125f);
                //spr.color = defaultColor;
                //yield return new WaitForSeconds(.125f);
            }
            StopCoroutine(pipis);
            spr.color = defaultColor;
            canTakeDamage = true;
        }

        private IEnumerator Invicibility_AnimateSprite()
        {
            for (int i = 0; i < IFrames; i++)
            {
                spr.color = damagedColor;
                yield return new WaitForSeconds(.125f);
                spr.color = defaultColor;
                yield return new WaitForSeconds(.125f);
            }
        }

    }
}
