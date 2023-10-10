using UnityEngine;

namespace Player
{
    public class Player_AnimController : MonoBehaviour
    {
        InputManager inputManager;
        Animator anim;
        private void Start()
        {
            inputManager = GetComponent<InputManager>();
            anim = GetComponent<Animator>();
        }
        void Update() => UpdateAnimator();
        void UpdateAnimator()
        {
            Vector2 input = inputManager.GetWASD();
            if (input != Vector2.zero)
            {
                anim.SetFloat("x", input.x);
                anim.SetFloat("y", input.y);
                anim.SetBool("walking", true);
            }
            else anim.SetBool ("walking", false);
        } 
    }
}
