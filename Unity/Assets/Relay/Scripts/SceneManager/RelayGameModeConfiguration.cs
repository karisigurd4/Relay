using System;
using UnityEngine;

namespace BitterShark.Relay
{
  [Serializable]
  public class RelayGameModeConfiguration
  {
    [SerializeField]
    public string SceneName = "";
    [SerializeField]
    public string ModeName = "";
    [SerializeField]
    public int MaxPossiblePlayers = 16;
  }
}
