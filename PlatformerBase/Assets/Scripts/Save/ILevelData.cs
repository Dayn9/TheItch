using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelData { 

    bool State { get; }
    string Name { get; }

    void OnLevelLoad(bool state);
}
