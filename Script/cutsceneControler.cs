using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class cutsceneControler : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string nextSceneName;

    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnCutsceneStopped;
            StartCoroutine(PrepareAndPlayCutscene());
        }
    }

    IEnumerator PrepareAndPlayCutscene()
    {
        yield return StartCoroutine(WaitForAssetsToBeReady());
        playableDirector.time = 0;
        playableDirector.RebuildGraph();

        playableDirector.Play();
    }

    IEnumerator WaitForAssetsToBeReady()
    {
        yield return new WaitUntil(() => AreAllAssetsLoaded());
    }

    bool AreAllAssetsLoaded()
    {
        return true;
    }

    void OnCutsceneStopped(PlayableDirector director)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
