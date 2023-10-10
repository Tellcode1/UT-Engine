using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(InputManager))]
    public class Player_Movement : MonoBehaviour
    {
        Rigidbody2D rb;
        InputManager inputManager;

        Vector2 keyInput;

        [SerializeField] float moveSpeed;

        private void Start() => CollectComponents();

        void CollectComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            ReceiveInput();
            //if (Input.GetKeyDown(KeyCode.X)) SceneManager.
        }

        private void FixedUpdate() => Move(keyInput);
        void ReceiveInput() => keyInput = inputManager.GetWASD();

        private void Move(Vector2 dir) => rb.velocity = dir * moveSpeed;
    }
}
