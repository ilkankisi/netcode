using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Game.Data
{
    public class LobbyData
    {
        public int _mapIndex;
        public int MapIndex { get; set; }

        public void Initialize(int mapIndex)
        {
            _mapIndex = mapIndex;
        }
        public void Initialize(Dictionary<string, DataObject> lobbyData)
        {
            UpdateState(lobbyData);
        }

        public void UpdateState(Dictionary<string, DataObject> lobbyData)
        {
            if (lobbyData.ContainsKey("MapIndex"))
            {
                _mapIndex = Int32.Parse(lobbyData["MapIndex"].Value);
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>()
            {
                {"MapIndex",_mapIndex.ToString() }
            };

        }
    }


}
