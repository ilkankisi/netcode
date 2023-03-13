using Assets.Scripts.Game;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    private void Start()
    {
        _lobbyCodeText.text= $"Lobby Code: {GameLobbyManager.Instance.GetLobbyCode()}";
    }
}
