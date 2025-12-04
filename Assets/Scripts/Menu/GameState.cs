using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static bool IsPaused = false;
    public static bool IsDead = false;
    public static bool IsVictorious = false;

    public static bool MenuOpen => IsPaused || IsDead || IsVictorious;
}
