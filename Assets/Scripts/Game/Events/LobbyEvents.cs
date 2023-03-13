
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Game.Events
{
    public static class LobbyEvents
    {
        public delegate void LobbyUpdated();
        public static LobbyUpdated onLobbyUpdated;

        public delegate void LobbyReady();
        public static LobbyReady onLoadRedy;
    }
}
