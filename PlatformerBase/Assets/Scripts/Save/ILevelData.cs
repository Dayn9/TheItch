using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelData { 

    bool State { get; }

    void OnLevelLoad(bool state);
}
