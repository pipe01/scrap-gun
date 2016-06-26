using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public enum Actions
    {
        Singleplayer,
        Multiplayer,
        Quit
    }

    public string inGameScene;
    public Actions action;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Act);
    }

    void Act()
    {
        switch (action)
        {
            case Actions.Singleplayer:
                SceneManager.LoadScene(inGameScene);
                print(inGameScene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(inGameScene));
                break;
            case Actions.Multiplayer:
                break;
            case Actions.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
            default:
                break;
        }
    }
}