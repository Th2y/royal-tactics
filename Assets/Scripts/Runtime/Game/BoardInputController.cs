using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask boardLayer;

    [Header("Mobile UX")]
    [SerializeField] private float minPressTime = 0.05f;
    [SerializeField] private float maxMoveTolerance = 20f;

    private Tile currentHover;
    private Tile pressedTile;

    private float pressStartTime;
    private Vector2 pressStartPos;
    private bool isPressing;

    private void Update()
    {
        HandlePointer();
    }

    #region Pointer (Mouse + Touch)
    private void HandlePointer()
    {
        if (Pointer.current == null)
        {
            ResetTouchState();
            return;
        }

        Vector2 pointerPos = Pointer.current.position.ReadValue();

        SetHover(RaycastTile(pointerPos));

        if (Pointer.current.press.wasPressedThisFrame)
        {
            isPressing = true;
            pressStartTime = Time.time;
            pressStartPos = pointerPos;

            pressedTile = RaycastTile(pointerPos);
            SetHover(pressedTile);
        }

        if (Pointer.current.press.isPressed && isPressing)
        {
            if (Vector2.Distance(pointerPos, pressStartPos) > maxMoveTolerance)
            {
                ResetTouchState();
            }
        }

        if (Pointer.current.press.wasReleasedThisFrame && isPressing)
        {
            float pressDuration = Time.time - pressStartTime;

            if (pressDuration >= minPressTime)
            {
                Tile releasedTile = RaycastTile(pointerPos);

                if (pressedTile != null && pressedTile == releasedTile)
                    pressedTile.OnClick();
            }

            ResetTouchState();
        }
    }
    #endregion

    #region Hover
    private void SetHover(Tile tile)
    {
        if (tile == currentHover) return;

        if (currentHover != null)
            currentHover.OnHoverExit();

        currentHover = tile;

        if (currentHover != null)
            currentHover.OnHoverEnter();
    }
    #endregion

    #region Reset
    private void ResetTouchState()
    {
        isPressing = false;
        pressedTile = null;

        if (currentHover != null)
        {
            currentHover.OnHoverExit();
            currentHover = null;
        }
    }
    #endregion

    #region Raycast
    private Tile RaycastTile(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, boardLayer))
            return hit.collider.GetComponent<Tile>();

        return null;
    }
    #endregion
}
