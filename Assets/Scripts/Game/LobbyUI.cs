using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.Events;
using Assets.Scripts.GameFramework.Core;

namespace Assets.Scripts.Game
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Image _mapImage;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private TextMeshProUGUI _mapName;
        [SerializeField] private MapSelectionData _mapSelectionData;
        private int _currentMapIndex = 0;
        private void OnEnable()
        {
            if (GameLobbyManager.Instance.IsHost)
            {
                LobbyEvents.onLoadRedy += onLoadRedy;
                _readyButton.onClick.AddListener(onReadyPressed);
                _leftButton.onClick.AddListener(onLeftButton);
                _rightButton.onClick.AddListener(onRightButton);
                _startButton.onClick.AddListener(onStartButtonClicked);
            }


            LobbyEvents.onLobbyUpdated += onLobbyUpdates;   

        }
        private void OnDisable()
        {
            if (GameLobbyManager.Instance.IsHost)
            {
                _readyButton.onClick.RemoveAllListeners();
                _leftButton.onClick.RemoveAllListeners();
                _rightButton.onClick.RemoveAllListeners();
                _startButton.onClick.RemoveAllListeners();
            }

            LobbyEvents.onLoadRedy -= onLoadRedy;
            LobbyEvents.onLobbyUpdated -= onLobbyUpdates;

        }

        private void onLobbyUpdates()
        {
            GameLobbyManager.Instance.GetMapIndex();
            UpdateMap();
        }

        private async void onRightButton()
        {
            _currentMapIndex = (_currentMapIndex + 1 < _mapSelectionData.maps.Count-1) ? _currentMapIndex+1 : _mapSelectionData.maps.Count - 1;
            UpdateMap();
            GameLobbyManager.Instance.SetSelectMap(_currentMapIndex);
        }

        private async void onLeftButton()
        {
            _currentMapIndex = (_currentMapIndex - 1 > 0) ? _currentMapIndex-1 : 0;
            UpdateMap();
            GameLobbyManager.Instance.SetSelectMap(_currentMapIndex);
        }

        private void UpdateMap()
        {
            _mapImage.color = _mapSelectionData.maps[_currentMapIndex].mapThumbnail;
            _mapName.text = _mapSelectionData.maps[_currentMapIndex].mapName;
        }



        private async void onStartButtonClicked()
        {
            await GameLobbyManager.Instance.StartGame("Lobby");
        }
        private void Start()
        {
            _lobbyCodeText.text = $"Lobby Code: {GameLobbyManager.Instance.GetLobbyCode()}";
            if (!GameLobbyManager.Instance.IsHost)
            {
                _leftButton.gameObject.SetActive(false);
                _rightButton.gameObject.SetActive(false);

            }
            UpdateMap();
        }
        private async void onReadyPressed()
        {
            bool succeded = await GameLobbyManager.Instance.SetPlayerReady();
            if(succeded)
            {
                _readyButton.gameObject.SetActive(false);
            }
        }
        private void onLoadRedy()
        {
            _startButton.gameObject.SetActive(true);

        }
    }
}