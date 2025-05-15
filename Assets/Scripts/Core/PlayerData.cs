using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Sprite avatar;           
    public PlayerSymbol symbol;    
}

public enum PlayerSymbol
{
    X,
    O
}
