using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool Shown = false;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

    }

    void Update()
    {
        bool key = Input.GetButtonDown("Pause");

        if (key)
        {
            Shown = !Shown;

            canvas.enabled = Shown;
        }
    }

}