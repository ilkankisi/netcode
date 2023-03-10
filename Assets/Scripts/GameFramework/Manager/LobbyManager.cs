using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.GameFramework.Manager
{
    public class LobbyManager : Core.Singleton<LobbyManager>
    {
        private Lobby _lobby;
        private Coroutine _heartBeatCoroutine;
        private Coroutine _refreshCoroutine;

        public async Task<bool> CreateLobby(int maxPlayer,bool isPrivate, Dictionary<string,string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            Player player = new Player(AuthenticationService.Instance.PlayerId,connectionInfo:null,playerData);
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = player
            };
            
            
            try
            {
                _lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayer);
            }
            catch (System.Exception)
            {

                return false;
            }
            UnityEngine.Debug.Log(message: $"Lobby created with lobby id{_lobby.Id}");
            _heartBeatCoroutine=StartCoroutine(HearthBeatLobbyCoroutine(_lobby.Id,6f));
            _refreshCoroutine = StartCoroutine(RefreshLobbyCoroutine(_lobby.Id, 1f));
            return true;
        }

        private IEnumerator HearthBeatLobbyCoroutine(string lobbyID, float waitTimeSeconds)
        {
 
                UnityEngine.Debug.Log(message: "HearthBeat");
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyID);
                yield return new WaitForSeconds(waitTimeSeconds); 
        }

        private IEnumerator RefreshLobbyCoroutine(string lobbyID, float waitTimeSeconds)
        {
  
                Task<Lobby> task=LobbyService.Instance.GetLobbyAsync(lobbyID);
                yield return new WaitUntil(() => task.IsCompleted);
                Lobby newLobby = task.Result;
                if (newLobby.LastUpdated > _lobby.LastUpdated) 
                {
                    _lobby = newLobby;
                }
        }

        private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
            foreach (var(key,value) in data )
            {
                playerData.Add(key, new PlayerDataObject(visibility: PlayerDataObject.VisibilityOptions.Member, value: value));
            }
            return playerData;
        }
        public void OnApplicationQuit()
        {
            if(_lobby!=null && _lobby.HostId==AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
            }
        }

        internal string GetLobbyCode()
        {
            return _lobby?.LobbyCode;
        }
    }
}
