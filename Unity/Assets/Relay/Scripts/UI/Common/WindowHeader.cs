using UnityEngine;

namespace BitterShark.Relay
{
  public class WindowHeader : MonoBehaviour
  {
    [Header("References")]
    public RectTransform WindowReference;

    private bool moveWindowToCursor = false;

#if (ENABLE_LEGACY_INPUT_MANAGER)
    public void OnGrab()
    {
      MessageBusManager.Instance.Publish(new WindowClickedMessage() { });

      if (GetComponentInParent<Window>() is var w && w != null)
      {
        w.BringToTop();
      }

      moveWindowToCursor = true;
    }

    public void OnRelease()
    {
      moveWindowToCursor = false;
    }

    private void Update()
    {
      if (moveWindowToCursor)
      {
        var centerOffset = -(Input.mousePosition.x - WindowReference.position.x);

        WindowReference.position = Input.mousePosition - (Vector3.up * (WindowReference.sizeDelta.y / 2.0f)) + new Vector3(centerOffset + Input.GetAxis("Mouse X") * 28, GetComponent<RectTransform>().sizeDelta.y);
      }
    }
#elif (ENABLE_INPUT_SYSTEM)
  private void Start() {
    Debug.LogWarning("Relay UI implementation for new input system pending. Moving windows is not supported.");
  }

  public void OnGrab()
  {
  }

  public void OnRelease()
  {
  }

  private void Update()
  {
  }
#endif
  }

}
