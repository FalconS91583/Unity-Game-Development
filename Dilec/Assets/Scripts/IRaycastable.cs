using BramaBadura.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaycastable
{
    public CursorType GetCursorType();
    bool HandleRaycast(PlayerController callingController);
}
