using FightHandler;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    SpriteRenderer spr;

    [SerializeField] int id;
    [SerializeField] Sprite interactingSprite;
    [SerializeField] Sprite staticSprite;

    private void Update()
    {
        if (InputManager.inputMgr.GetConfirm() && FightManager.FManager.IsPlayerTurn() && FightManager.FManager.IsInteracting(id))
        {
            switch (id)
            {
                case 0:
                    {
                        FightManager.FManager.CallFIGHT();
                        break; ;
                    }
                case 1:
                    {
                        FightManager.FManager.CallACT();
                        break;
                    }
                case 2:
                    {
                        FightManager.FManager.CallITEM();
                        break;
                    }
                case 3:
                    {
                        FightManager.FManager.CallMERCY();
                        break;
                    }
            }
        }
    }

    public void Initialize()
    {
        spr = GetComponent<SpriteRenderer>();
        staticSprite = (Sprite)Resources.Load(id.ToString() + "_0", typeof(Sprite));
        interactingSprite = (Sprite)Resources.Load(id.ToString() + "_1", typeof(Sprite));
    }

    public void EnterInteraction() => spr.sprite = interactingSprite;


    public void LeaveInteraction() => spr.sprite = staticSprite;
}
