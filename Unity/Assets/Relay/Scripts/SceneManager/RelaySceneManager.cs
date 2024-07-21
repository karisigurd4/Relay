using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace BitterShark.Relay
{
  public class RelaySceneManager : MonoBehaviour
  {
    private static RelaySceneManager _instance;
    public static RelaySceneManager Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;
    }


    [Header("Configuration")]
    public RelaySceneConfiguration OnStartSceneConfiguration;

    [HideInInspector]
    public string ActiveScenePath;

    private Dictionary<string, Action> onSceneLoadedActions = new Dictionary<string, Action>();

    private List<string> loadedScenes = new List<string>();

    private RelaySceneType[] sceneTypePriority = new RelaySceneType[]
    {
    RelaySceneType.UI,
    RelaySceneType.World
    };

    void Start()
    {
      if (OnStartSceneConfiguration != null)
      {
        LoadSceneConfiguration(OnStartSceneConfiguration);
      }
      SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
      if (onSceneLoadedActions.ContainsKey(scene.name))
      {
        onSceneLoadedActions[scene.name]();
        onSceneLoadedActions.Remove(scene.name);
      }

      if (scene.path == ActiveScenePath)
      {
        SceneManager.SetActiveScene(scene);
      }
    }

    public void LoadSceneConfiguration(RelaySceneConfiguration sceneConfiguration)
    {
      if (!sceneConfiguration.Additive)
      {
        UnloadScenesBySceneConfiguration(sceneConfiguration);
      }

      LoadScenesBySceneConfiguration(sceneConfiguration);
    }

    public void LoadSceneByPath(string scenePath, bool setActive, Action onSceneLoaded)
    {
      if (SceneUtility.GetBuildIndexByScenePath(scenePath) == -1)
      {
        Debug.LogError($"The scene {scenePath} is missing from build settings. Make sure to add it to the build.");
        return;
      }


      SceneManager.LoadScene(scenePath, LoadSceneMode.Additive);
      if (onSceneLoaded != null)
      {
        onSceneLoadedActions.Add(scenePath, onSceneLoaded);
      }

      if (setActive)
      {
        ActiveScenePath = scenePath;
      }

      loadedScenes.Add(scenePath);
    }

    private List<Scene> GetScenes()
    {
      var scenes = new List<Scene>();
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {
        scenes.Add(scenes[i]);
      }
      return scenes;
    }

    private void LoadScenesBySceneConfiguration(RelaySceneConfiguration sceneConfiguration)
    {
      var scenesToLoad = sceneConfiguration.Scenes.Where(x => !loadedScenes.Contains(x.Name)).ToArray();
      foreach (var sceneType in sceneTypePriority)
      {
        var scenesToLoadOfType = scenesToLoad.Where(x => x.SceneType == sceneType).ToArray();
        foreach (var scene in scenesToLoadOfType)
        {
          if (SceneUtility.GetBuildIndexByScenePath(scene.Name) == -1)
          {
            Debug.LogError($"The scene {scene.Name} is missing from build settings. Make sure to add it to the build.");
            continue;
          }

          SceneManager.LoadScene(scene.Name, LoadSceneMode.Additive);

          loadedScenes.Add(scene.Name);
        }
      }
    }

    public void UnloadScenesBySceneConfiguration(RelaySceneConfiguration sceneConfiguration)
    {
      if (sceneConfiguration.Scenes.Any(x => x.SceneType == RelaySceneType.World))
      {
        LightmapSettings.lightmaps = new LightmapData[0];
        Resources.UnloadUnusedAssets();
      }

      var scenesToUnload = loadedScenes.Where(y => sceneConfiguration.Scenes.Any(x => x.Name == y)).ToArray();
      for (int i = 0; i < scenesToUnload.Length; i++)
      {
        UnloadSceneByName(scenesToUnload[i]);
      }
    }

    public void UnloadSceneByName(string sceneNameOrPath)
    {
      try
      {
#pragma warning disable CS0618 // Type or member is obsolete
        var scene = SceneManager.GetAllScenes().FirstOrDefault(x => x.name == sceneNameOrPath || x.path == sceneNameOrPath);
#pragma warning restore CS0618 // Type or member is obsolete

        if (scene != null && scene.isLoaded)
        {
          SceneManager.UnloadSceneAsync(sceneNameOrPath);
        }
      }
      catch (Exception e)
      {
        Debug.LogError(sceneNameOrPath);
        Debug.LogError(e);
      }
    }
  }
}

