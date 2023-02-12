using UnityEngine;

public static class CursorManager
{
    /// <summary>
    /// Returns true if the Cursor is locked
    /// </summary>
    public static bool IsCursorLocked(out CursorLockMode CursorLockState)
    {
        CursorLockState = Cursor.lockState;

        return Cursor.lockState == CursorLockMode.Locked;
    }

    public static void LockCursor()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public static void LockCursor(CursorLockMode lockMode)
    {
        //Cursor.visible = false;
        //Cursor.lockState = lockMode;
    }

    public static void UnlockCursor()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public static void UnlockCursor(CursorLockMode lockMode)
    {
        //Cursor.visible = true;
        //Cursor.lockState = lockMode;
    }
}
