using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class JoinGameButton : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI ButtonTextReference;
    public Image ButtonBgrImageReference;

    [Space(15)]
    [Header("Configuration")]
    public float StartGameWaitTime = 5.0f;

    private bool activated = false;
    private float timeActivated = 0.0f;
    private string textAtStart;
    private float waitTime = 0.0f;

    private RectTransform rectTransform;

    private void Start()
    {
      rectTransform = GetComponent<RectTransform>();
      textAtStart = ButtonTextReference.text;
      ResetTextAlpha();
    }

    private void ResetTextAlpha()
    {
      ButtonTextReference.color = new Color(ButtonTextReference.color.r, ButtonTextReference.color.g, ButtonTextReference.color.b, 0.0f);
    }

    public void OnClick()
    {
      if (!(activated = !activated))
      {
        ButtonTextReference.text = textAtStart;
      }
      else
      {
        ButtonTextReference.text = $"{(int)StartGameWaitTime}";
      }

      waitTime = 1.0f;

      ResetTextAlpha();
    }

    private void Update()
    {
      if (waitTime <= 0.75f)
      {
        ButtonTextReference.color = Color.Lerp(ButtonTextReference.color, new Color(ButtonTextReference.color.r, ButtonTextReference.color.g, ButtonTextReference.color.b, 1.0f), Time.deltaTime);
      }

      rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, new Vector2(activated ? 150 : 350, 128), Time.deltaTime * 8);

      if (waitTime >= 0.0f)
      {
        waitTime -= Time.deltaTime;
      }

      ButtonBgrImageReference.material.SetFloat("_Static", Mathf.Lerp(ButtonBgrImageReference.material.GetFloat("_Static"), (activated ? 1.0f : 0.0f), Time.deltaTime * 2.0f));

      if (activated)
      {

        if (waitTime <= 0.0f)
        {
          timeActivated += Time.deltaTime;
          ButtonTextReference.text = $"{(int)(StartGameWaitTime - timeActivated)}";
        }

        if (timeActivated >= StartGameWaitTime)
        {
          Debug.Log($"Sending");
          MessageBusManager.Instance.Publish(new ShowFindGameServerWindowMessage()
          {
          });
          MessageBusManager.Instance.Publish(new UpdateCanvasGroupsMessage()
          {
            On = false
          });
          gameObject.SetActive(false);
        }
      }
    }
  }

}
