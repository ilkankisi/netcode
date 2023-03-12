using Assets.Scripts.GameFramework.Core;
using Assets.Scripts.GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameLobbyManager : Singleton<GameLobbyManager>
{
    public async Task<bool> CreateLobby()
    {
        //GamerTag isminde Key oluþturuldu ve Value'su HostPlayer yapýldý.
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag","HostPlayer" }
        };
        //max 4 kullanýcýya sahip olabilecek özel bir lobby ve oyuncu bilgilerinin bulunduðu
        //bir lobby oluþturulduysa buradan baþarýlý bir þekilde ayrýlýr
        bool succeded =await LobbyManager.Instance.CreateLobby(maxPlayer:4,isPrivate:true, playerData);
        return succeded;
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }

    public async Task<bool> JoinLobby(string code)
    {
        //lobby codu ve oyuncu verileri ile lobby'ye giriþ yapýlýr.
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag","JoinPlayer" }
        };
        bool succeded = await LobbyManager.Instance.JoinLobby(code, playerData);
        return succeded;
    }
}
