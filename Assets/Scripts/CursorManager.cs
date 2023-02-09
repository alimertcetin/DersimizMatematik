using UnityEngine;

public class CursorManager
{
    private static CursorManager instance;
    public static CursorManager Instance
    {
        get
        {
            if (instance == null)
                instance = new CursorManager();
            return instance;
        }
    }

    /// <summary>
    /// Returns true if the Cursor is locked
    /// </summary>
    public static bool IsCursorLocked(out CursorLockMode CursorLockState)
    {
        CursorLockState = Cursor.lockState;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LockCursor()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void LockCursor(CursorLockMode lockMode)
    {
        //Cursor.visible = false;
        //Cursor.lockState = lockMode;
    }

    public void UnlockCursor()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnlockCursor(CursorLockMode lockMode)
    {
        //Cursor.visible = true;
        //Cursor.lockState = lockMode;
    }
}
