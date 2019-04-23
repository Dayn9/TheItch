public interface IHealthObject {
    float Health { get; } //current health of the object
    int MaxHealth { get; } //maximum amount of health the object can have
    bool Invulnerable { get; set; }

    /// <summary>
    /// Occurs when object takes damage
    /// </summary>
    /// <param name="amount"></param>
    void Damage(float amount);

    /// <summary>
    /// Occurs when object gains health
    /// </summary>
    /// <param name="amount"></param>
    void Heal(float amount);

    /// <summary>
    /// Occurs when object regains full health
    /// </summary>
    void FullHeal();

}
