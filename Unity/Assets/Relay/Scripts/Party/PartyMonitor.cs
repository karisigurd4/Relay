using UnityEngine;

namespace BitterShark.Relay
{
  public class PartyMonitor : MonoBehaviour
  {
    private bool notInParty = false;
    private static float refreshRate = .5f;
    private float lastPolledPartyTime = 0.0f;

    void Start()
    {
    }

    void Update()
    {
      if (!notInParty)
      {
        if (Time.time > lastPolledPartyTime + refreshRate)
        {
          lastPolledPartyTime = Time.time;

          PartyApiClient.Instance.GetPlayerParty(RelayPlayerManager.Instance.GetPlayerId(), response =>
          {
            if (response == null || response.Party == null)
            {
              notInParty = true;
            }
          });
        }
      }
    }
  }
}
