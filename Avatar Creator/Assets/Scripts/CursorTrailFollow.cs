using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTrailFollow : MonoBehaviour
{
    RectTransform canvasRect;

    void Start()
    {
        // Get the RectTransform of the canvas
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current position of the cursor in screen space
        Vector3 cursorScreenPosition = Input.mousePosition;

        // Convert the screen position to position within the canvas
        Vector2 localCursorPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, cursorScreenPosition, null, out localCursorPos);

        // Set the local position of the CursorTrail GameObject within the canvas
        transform.localPosition = localCursorPos;
    }
}
