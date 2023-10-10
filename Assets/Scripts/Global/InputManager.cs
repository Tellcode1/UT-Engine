using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputMgr;
    private void Start()
    {
        if (inputMgr != null && inputMgr != this) Destroy(gameObject);
        else inputMgr = this;
    }
    public Vector2 GetWASD() => new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public bool GetRightArrowKey() => Input.GetKeyDown(KeyCode.RightArrow);
    public bool GetLeftArrowKey() => Input.GetKeyDown(KeyCode.LeftArrow);
    public bool GetConfirm() => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return);
}
