using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player_Battle
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]

    public class Player : MonoBehaviour
    {
        private UnityAction onDamaged;

        Rigidbody2D rb;
        public bool canMove;
        [SerializeField] float speed;

        [SerializeField] private int health;
        private Slider healthbar;
        Text healthText;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            var healthParent = GameObject.Find("Healthbar");

            healthbar = healthParent.GetComponent<Slider>();
            healthText = healthParent.transform.GetChild(2).GetComponent<Text>();

            InitializeHealth();

            onDamaged += RepaintHealthbar;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(1);
            }
        }

        void FixedUpdate()
        {
            if (canMove) rb.velocity = InputManager.inputMgr.GetWASD().normalized * speed;
            else rb.velocity = Vector3.zero;
        }

        private void TakeDamage(int damage)
        {
            health -= damage;
            RepaintHealthbar();
        }

        private void InitializeHealth()
        {
            healthbar.value = health;
            healthbar.maxValue = health;
            healthbar.minValue = 0;
            healthText.text = (health + "/" + healthbar.maxValue).ToString();
        }

        private void RepaintHealthbar()
        {
            healthbar.value = health;
            healthText.text = (health + "/" + healthbar.maxValue).ToString();
        }
    }
}
