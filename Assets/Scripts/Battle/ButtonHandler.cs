using UnityEngine;
using static BattleSystems.FightManager;

namespace BattleSystems
{
    public class ButtonHandler : MonoBehaviour
    {
        private SpriteRenderer spr;

        [SerializeReference] private int id;
        [SerializeReference] private Sprite interactingSprite;
        [SerializeReference] private Sprite staticSprite;

        private void Update()
        {
            if (InputManager.inputMgr.GetConfirm() && FManager.IsPlayerTurn() && FManager.IsInteracting(id))
                FManager.Call(id);
        }

        public void Initialize()
        {
            spr = GetComponent<SpriteRenderer>();
        }

        public void EnterInteraction() => spr.sprite = interactingSprite;


        public void LeaveInteraction() => spr.sprite = staticSprite;
    }
}
