using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        [SerializeField] private Button _readyButton;

        private void OnEnable()
        {
            _readyButton.onClick.AddListener(OnReadyPressed);
        }
        private void OnDisable()
        {
            _readyButton.onClick.RemoveAllListeners();
        }
        private async void OnReadyPressed()
        {
            bool succeded = await GameLobbyManager.Instance.SetPlayerReady();
            if(succeded)
            {
                _readyButton.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _lobbyCodeText.text = $"Lobby Code: {GameLobbyManager.Instance.GetLobbyCode()}";
        }
    }
}