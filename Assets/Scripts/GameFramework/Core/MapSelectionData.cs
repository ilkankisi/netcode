using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameFramework.Core
{

   [CreateAssetMenu(menuName = "Data/MapSelectionData", fileName = "MapSelectionData")]
   public class MapSelectionData: ScriptableObject
   {
       public List<MapInfo> maps;
    }

}
[Serializable]
public struct MapInfo
{
    public Color mapThumbnail;
    public string mapName;
    public string sceneName;
}