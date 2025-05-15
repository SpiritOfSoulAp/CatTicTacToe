using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int index;
    public Image icon;

    private PlayerData owner;

    public bool IsEmpty() => owner == null;
    public PlayerData GetOwner() => owner;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (IsEmpty() && GameBoard.Instance.GameActive)
        {
            GameBoard.Instance.MakeMove(index);
        }
    }

    public void SetState(PlayerData player)
    {
        if (icon == null) return;

        owner = player;
        icon.sprite = player.avatar;
            
        icon.enabled = true;
    }

    public void ResetCell()
    {
        owner = null;
        icon.enabled = false;
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }
}
