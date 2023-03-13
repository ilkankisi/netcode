using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.GameFramework.Events
{
    public static class LobbyEvents
    {
        public delegate void LobbyUpdated(Lobby lobby);
        public static LobbyUpdated onLobbyUpdated;
    }
}