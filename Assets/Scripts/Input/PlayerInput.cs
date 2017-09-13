using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Tooltip("Current key configuration.")]
    public KeyboardMouseConfig config;
    public Vector3 cursorPosition;
    public bool selected;

    void Update()
    {
        this.SetActions();
        cursorPosition = Input.mousePosition;
    }

    private void SetActions()
    {
        this.selected = Input.GetKeyUp(this.config.select);
    }

    public RaycastHit[] GetCursorOverObjects()
    {
        return Physics.RaycastAll(Camera.main.ScreenToWorldPoint(cursorPosition), Vector3.down);
    }

    public RaycastHit[] GetCursorOverObjects(int layerMask)
    {
        return Physics.RaycastAll(Camera.main.ScreenToWorldPoint(cursorPosition), Vector3.down, layerMask);
    }
}
