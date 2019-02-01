using UnityEngine;

public interface IPlayer{

    /// <summary>
    /// defines properties and methods related to player  
    /// </summary>

    Vector2 ReturnPosition { set; } //set reset postion for checkpoints
    HeartbeatPower Power { get; } //ref to heartbeatPower Component
    Animator Animator { get; }

    //player states
    bool InFallZone { set; }
    //bool Frozen { get; set; }

    //movement speeds
    float MoveSpeed { get; set; }
    float JumpSpeed { get; set; }

    /// <summary>
    /// occurs when the player falls off the map
    /// </summary>
    void OnPlayerFall();
}
