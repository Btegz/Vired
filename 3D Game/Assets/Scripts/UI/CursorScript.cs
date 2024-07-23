using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    bool cursorNormal;

    [SerializeField]Texture2D normalCursor;
    [SerializeField]Texture2D pressCursor;

    [SerializeField] InputActionReference leftMouse;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        leftMouse.action.Enable();
        leftMouse.action.performed += ToggleCursorPress;
        cursorNormal = true;
        Cursor.SetCursor(normalCursor, Vector2.one * 16,CursorMode.Auto);
    }

    public void ToggleCursorPress(InputAction.CallbackContext obj)
    {
        cursorNormal = !cursorNormal;
        Cursor.SetCursor(cursorNormal?normalCursor:pressCursor, Vector2.one * 16, CursorMode.Auto);
    }
}
