using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer{
    HeartbeatPower Power { get; } //ref to heartbeatPower Component

    /// <summary>
    /// occurs when the player falls off the map
    /// </summary>
    void OnPlayerFall();
}
