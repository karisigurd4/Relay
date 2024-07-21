using System.Collections.Generic;
using UnityEngine;

namespace BitterShark.Relay
{
  [CreateAssetMenu(fileName = "RelaySceneConfiguration", menuName = "Relay/Scene Configuration")]
  public class RelaySceneConfiguration : ScriptableObject
  {
    public bool Additive = false;
    public List<RelayScene> Scenes = new List<RelayScene>();
    public string ActiveScene;
  }
}