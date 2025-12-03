using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleDictionary
{
    [SerializeField] private ModuleType moduleType;
    [SerializeField] private ModuleState moduleState;

    public ModuleType ModuleType { get => moduleType; }
    public ModuleState ModuleState { get => moduleState; }
}
