using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.Events;
namespace Assets.Scripts.Game
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _startButton;

        private void OnEnable()
        {
            if (GameLobbyManager.Instance.IsHost)
            {
                LobbyEvents.onLoadRedy += onLoadRedy;
            }
            _readyButton.onClick.AddListener(OnReadyPressed);
        }
        private void OnDisable()
        {
            _readyButton.onClick.RemoveAllListeners();
            LobbyEvents.onLoadRedy -= onLoadRedy;
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
        private void onLoadRedy()
        {
            _startButton.gameObject.SetActive(true);

        }
    }
}