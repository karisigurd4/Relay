using UnityEngine;
using UnityEngine.SceneManagement;

namespace BitterShark.Relay
{
  public class TransitionFinishedMessageConsumer : MessageBusConsumer<TransitionFinishedMessage>
  {
    [Header("Scene configurations")]
    public RelaySceneConfiguration MainMenuOverlaySceneConfiguration;
    public RelaySceneConfiguration TransitionInSceneConfiguration;
    public RelaySceneConfiguration TransitionOutSceneConfiguration;

    public override void OnConsumeMessage(TransitionFinishedMessage message)
    {
      var sceneName = SceneManager.GetActiveScene().name;
      RelaySceneManager.Instance.LoadSceneByPath("Loading", true, () =>
      {
        RelaySceneManager.Instance.UnloadSceneByName(sceneName);
        RelaySceneManager.Instance.UnloadSceneByName("Assets/Relay/Scenes/MainMenuOverlay.unity");

        RelaySceneManager.Instance.LoadSceneByPath(RelaySceneLoaderMemory.LoadSceneName, true, () =>
        {
          MessageBusManager.Instance.Publish(new HideLoaderDialogMessage()
          {
          });
          RelaySceneManager.Instance.UnloadSceneByName("Assets/Relay/Scenes/Loading.unity");
          RelaySceneManager.Instance.UnloadSceneByName("Assets/Relay/Scenes/TransitionIn.unity");
        });
      });
    }
  }
}