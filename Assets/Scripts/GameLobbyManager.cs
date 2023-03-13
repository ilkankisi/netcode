using Assets.Scripts.GameFramework.Core;
using Assets.Scripts.GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameLobbyManager : Singleton<GameLobbyManager>
{
    public async Task<bool> CreateLobby()
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag","HostPlayer" }
        };
        bool succeded =await LobbyManager.Instance.CreateLobby(maxPlayer:4,isPrivate:true, playerData);
        return succeded;
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }
}
