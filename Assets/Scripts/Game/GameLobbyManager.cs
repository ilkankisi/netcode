using Assets.Scripts.Game.Data;
using Assets.Scripts.GameFramework.Core;
using Assets.Scripts.GameFramework.Events;
using Assets.Scripts.GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Game
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> _lobbyPlayerData = new List<LobbyPlayerData>();
        private LobbyPlayerData _localLobbyPlayerData;

        public bool IsHost => _localLobbyPlayerData.Id == LobbyManager.Instance.GetHostId();
        void OnEnable()
        {
            LobbyEvents.onLobbyUpdated += onLobbyUpdated;
        }

        void OnDisable()
        {
            LobbyEvents.onLobbyUpdated -= onLobbyUpdated;
        }
        private void onLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayerData();
            _lobbyPlayerData.Clear();

            int numberOfPlayerReady = 0;

            foreach (Dictionary<string, PlayerDataObject> data in playerData)
            {
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                //Burada id, gametag, isReady keyleri ve value'ları belirlendi.
                lobbyPlayerData.Initialize(data);

                if (lobbyPlayerData.IsReady)
                {
                    numberOfPlayerReady++;
                }


                //Eğer lobby içindeki player Id ve Authenticationdaki player id eşitse lobbydeki player ile Authenticate etmiş player verileri birbirine eşitlenir. 
                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    _localLobbyPlayerData = lobbyPlayerData;
                }
                //lobbydeki veriler lobbyPlayer listesine eklenir.
                _lobbyPlayerData.Add(lobbyPlayerData);
            }

            //oyunun lobbysi update edilir.
            Events.LobbyEvents.onLobbyUpdated?.Invoke();

            if (numberOfPlayerReady==lobby.Players.Count)
            {
                Events.LobbyEvents.onLoadRedy?.Invoke();
            }
        }
        public async Task<bool> CreateLobby()
        {
            //GamerTag isminde Key oluşturuldu ve Value'su HostPlayer yapıldı.
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, gamertag: "HostPlayer");
            //max 4 kullanýcýya sahip olabilecek özel bir lobby ve oyuncu bilgilerinin bulunduðu
            //bir lobby oluþturulduysa buradan baþarýlý bir þekilde ayrýlýr
            bool succeded = await LobbyManager.Instance.CreateLobby(maxPlayer: 4, isPrivate: true, playerData.Serialize());
            return succeded;
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public async Task<bool> JoinLobby(string code)
        {
            //lobby codu ve oyuncu verileri ile lobby'ye giriþ yapýlýr.
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, gamertag: "JoinPlayer");
            bool succeded = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());
            return succeded;
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPlayerData;
        }

        public async Task<bool> SetPlayerReady()
        {
            _localLobbyPlayerData.IsReady = true;
            return await LobbyManager.Instance.UpdatePlayerData(_localLobbyPlayerData.Id,_localLobbyPlayerData.Serialize());
        }
    }
}