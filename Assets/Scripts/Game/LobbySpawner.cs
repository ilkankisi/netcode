using Assets.Scripts.Game.Data;
using Assets.Scripts.Game.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class LobbySpawner : MonoBehaviour
    {
        [SerializeField] private List<LobbyPlayer> _player;

        private void OnEnable()
        {
            LobbyEvents.onLobbyUpdated += onLobbyUpdated;
        }
        private void OnDisable()
        {
            LobbyEvents.onLobbyUpdated -= onLobbyUpdated;
        }
        private void onLobbyUpdated()
        {
            List<LobbyPlayerData> playerData = GameLobbyManager.Instance.GetPlayers();

            for (int i = 0; i < playerData.Count; i++)
            {
                LobbyPlayerData data = playerData[i];
                _player[i].SetData(data);
            }
        }
    }
}