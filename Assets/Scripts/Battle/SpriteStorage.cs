using UnityEngine;

public class SpriteStorage : MonoBehaviour
{
    public static SpriteStorage sprStorage;
    public Sprite[] gasterBlaster = new Sprite[6];

    #region Static Decleration
    private void Start()
    {
        if (sprStorage != null && sprStorage != this)
            Destroy(gameObject);
        else sprStorage = this;
    }
    #endregion
}
