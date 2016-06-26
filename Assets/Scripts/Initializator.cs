using SmartLocalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Initializator : MonoBehaviour
{
    public Texture2D cursor;
    public Vector2 hotspot;
    public CursorMode cursorMode;
    public Text textMouseHover;

    private Ray ray;
    private RaycastHit hit;
    private string lastHoverName;
    private Transform textParent;

    void Awake()
    {
        Cursor.SetCursor(cursor, hotspot, cursorMode);

        textParent = textMouseHover.transform.parent;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            string name = hit.collider.name;
            EventManager.InvokeEvent("hover.hoverCollider", name, hit);
            textMouseHover.enabled = true;
            switch (hit.collider.tag)
            {
                case "Door":
                    bool open = hit.collider.gameObject.GetComponent<DoorActuator>().Open;
                    textMouseHover.text = LanguageManager.Instance.GetTextValue(open ? "hover_closedoor" : "hover_opendoor");
                    break;
                case "Player":
                    break;
                default:
                    textMouseHover.enabled = false;
                    break;
            }
        }
        else
        {
            textMouseHover.text = "";
        }

        if (textMouseHover)
        {
            Vector3 pos = Input.mousePosition - textParent.localPosition;
            pos.x += 90;
            textMouseHover.transform.localPosition = pos;
        }
    }
}