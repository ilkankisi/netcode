using Assets.Scripts.GameFramework.Core;
using Assets.Scripts.GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameLobbyManager : Singleton<GameLobbyManager>
{
    public async Task<bool> CreateLobby()
    {
        //GamerTag isminde Key olu�turuldu ve Value'su HostPlayer yap�ld�.
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag","HostPlayer" }
        };
        //max 4 kullan�c�ya sahip olabilecek �zel bir lobby ve oyuncu bilgilerinin bulundu�u
        //bir lobby olu�turulduysa buradan ba�ar�l� bir �ekilde ayr�l�r
        bool succeded =await LobbyManager.Instance.CreateLobby(maxPlayer:4,isPrivate:true, playerData);
        return succeded;
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }

    public async Task<bool> JoinLobby(string code)
    {
        //lobby codu ve oyuncu verileri ile lobby'ye giri� yap�l�r.
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag","JoinPlayer" }
        };
        bool succeded = await LobbyManager.Instance.JoinLobby(code, playerData);
        return succeded;
    }
}
