using UnityEngine;

namespace BitterShark.Relay
{
  public class LoaderUI : MonoBehaviour
  {
    [Header("References")]
    public Transform OuterLoaderImage;
    public Transform InnerLoaderImage;

    [Space]
    [Header("Configuration")]
    public float OuterChangeDirectionTime = 4.0f;
    public float InnerChangeDirectionTime = 4.0f;
    public bool OuterDirection = false;
    public bool InnerDirection = false;
    public float OuterSpeed = 3.0f;
    public float InnerSpeed = 6.0f;
    public float OuterChangeDirectionSpeed = 40.0f;
    public float InnerChangeDirectionSpeed = 40.0f;

    private float outerStartSpeed;
    private float innerStartSpeed;
    private float outerChangeDirectionTimer;
    private float innerChangeDirectionTimer;

    private bool stopped = true;

    private void Start()
    {
      outerStartSpeed = OuterSpeed;
      innerStartSpeed = InnerSpeed;
      outerChangeDirectionTimer = OuterChangeDirectionTime;
      innerChangeDirectionTimer = InnerChangeDirectionTime;
    }

    private void OnEnable()
    {
      OuterLoaderImage.localScale = Vector3.zero;
      InnerLoaderImage.localScale = Vector3.zero;
    }

    public void StartLoading()
    {
      stopped = false;
    }

    public void StopLoading()
    {
      stopped = true;
    }

    private void Update()
    {
      OuterLoaderImage.localScale = Vector3.Slerp(OuterLoaderImage.localScale, stopped ? Vector3.zero : Vector3.one, Time.deltaTime * 8);
      InnerLoaderImage.localScale = Vector3.Slerp(InnerLoaderImage.localScale, stopped ? Vector3.zero : Vector3.one, Time.deltaTime * 15);

      if (outerChangeDirectionTimer > 0.0f)
      {
        outerChangeDirectionTimer -= Time.deltaTime;
      }

      if (outerChangeDirectionTimer <= 0.0f)
      {
        OuterDirection = !OuterDirection;
        outerChangeDirectionTimer = OuterChangeDirectionTime;
      }

      if (innerChangeDirectionTimer > 0.0f)
      {
        innerChangeDirectionTimer -= Time.deltaTime;
      }

      if (innerChangeDirectionTimer <= 0.0f)
      {
        InnerDirection = !InnerDirection;
        innerChangeDirectionTimer = InnerChangeDirectionTime;
      }

      OuterSpeed = Mathf.Lerp(OuterSpeed, OuterDirection ? outerStartSpeed : -outerStartSpeed, Time.deltaTime * OuterChangeDirectionSpeed);
      InnerSpeed = Mathf.Lerp(InnerSpeed, InnerDirection ? innerStartSpeed : -innerStartSpeed, Time.deltaTime * InnerChangeDirectionSpeed);

      OuterLoaderImage.transform.eulerAngles += Vector3.forward * Time.deltaTime * OuterSpeed;
      InnerLoaderImage.transform.eulerAngles += Vector3.forward * Time.deltaTime * InnerSpeed;
    }
  }
}