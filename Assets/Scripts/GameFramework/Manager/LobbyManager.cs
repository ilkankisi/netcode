using UnityEngine;
using System.Collections;
using Unity.Services.Core;
using System;
using Assets.Scripts.Game.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

namespace Assets.Scripts.GameFramework.Manager
{
    public class LobbyManager : Core.Singleton<LobbyManager>
    {
        private Lobby _lobby;
        private Coroutine _heartBeatCoroutine;
        private Coroutine _refreshCoroutine;
        public async Task<bool> CreateLobby(int maxPlayer, bool isPrivate, Dictionary<string, string> data)
        {
            //Kullanıcının verilerini lobbye tanıtmak için 
            //GamerTag key'ine sahip ve HostPlayer value'su olan data SerializePlayerData'da lobby modeline dönüştürülür.
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            //PlayerId,connectionInfo ve lobby modeline dönüştürülen kullanıcının verileri ile bir lobby player'ı oluşturulur.
            Player player = new Player(AuthenticationService.Instance.PlayerId, connectionInfo: null, playerData);
            //lobby'nin özel olacağı ve lobby'deki oyuncunun bilgisi set edilir.
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = player
            };

            //lobby içindeki oyuncu sayısı ve diğer özellikler belirlenir.
            try
            {
                _lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayer, options);
                UnityEngine.Debug.Log(message: $"Lobby created with lobby id{_lobby.Id}");
            }
            catch (System.Exception)
            {
                return false;
            }
            //lobbye bağlanan oyuncuları realtimeda lobbye bağlanmaları sağlanır.
            _heartBeatCoroutine = StartCoroutine(HearthBeatLobbyCoroutine(_lobby.Id, 6f));
            //ana bilgisayarın değişip değişmediğini veya bağlantı bilgilerinin değişip değişmediğini öğrenir.
            _refreshCoroutine = StartCoroutine(RefreshLobbyCoroutine(_lobby.Id, 1f));
            return true;
        }
        private IEnumerator HearthBeatLobbyCoroutine(string lobbyID, float waitTimeSeconds)
        {

            UnityEngine.Debug.Log(message: "HearthBeat");
            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyID);
                yield return new WaitForSeconds(waitTimeSeconds);
            }
        }
        private IEnumerator RefreshLobbyCoroutine(string lobbyID, float waitTimeSeconds)
        {
            while (true)
            {
                Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyID);
                yield return new WaitUntil(() => task.IsCompleted);
                Lobby newLobby = task.Result;
                    if (newLobby.LastUpdated > _lobby.LastUpdated)
                    {
                        _lobby = newLobby;
                        Events.LobbyEvents.onLobbyUpdated?.Invoke(_lobby);
                    }
                yield return new WaitForSeconds(waitTimeSeconds);
            }
        }
        private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
            foreach (var (key, value) in data)
            {
                playerData.Add(key, new PlayerDataObject(visibility: PlayerDataObject.VisibilityOptions.Member, value: value));
            }
            //PlayerDataObject tipindeki lobby model tipine dönüştürülüp yollandı.
            return playerData;
        }
        public void OnApplicationQuit()
        {
            if (_lobby != null && _lobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
            }
        }
        public string GetLobbyCode()
        {
            return _lobby?.LobbyCode;
        }
        public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
        {
            //lobby ayarlaması yapılır.
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            Player player = new Player(AuthenticationService.Instance.PlayerId,
                                       connectionInfo: null,
                                       SerializePlayerData(playerData));
            options.Player = player;
            try
            {
                _lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
            }
            catch (System.Exception)
            {
                return false;
            }
            _refreshCoroutine = StartCoroutine(RefreshLobbyCoroutine(_lobby.Id, 1f));
            return true;
        }
        public List<Dictionary<string, PlayerDataObject>> GetPlayerData()
        {
            List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();
            foreach (Player player in _lobby.Players)
            {
                data.Add(player.Data);
            }
            return data;
        }

        public async Task<bool> UpdatePlayerData(string playerId, Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> plyerData = SerializePlayerData(data);
            UpdatePlayerOptions options = new UpdatePlayerOptions()
            {
                Data= plyerData,
            };
            try
            {
                _lobby=await LobbyService.Instance.UpdatePlayerAsync(_lobby.Id, playerId,options);
            }
            catch (Exception)
            {
                return false;
            }
            GameFramework.Events.LobbyEvents.onLobbyUpdated(_lobby);
            return true;
        }

        public string GetHostId()
        {
            return _lobby.HostId;
        }
    }
}