﻿using System;
using System.Dynamic;
using UnityEngine.Events;

public interface IDisplayableEffect
{
    public const int NONE = int.MaxValue;

    public string GetEffectName();
    public int GetStack();
    public int GetDuration();

    public bool CanDisplay();
}