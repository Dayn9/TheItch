using UnityEngine;

//Legacy Heathbar System
public class Healthbar : Global {

    [SerializeField] private Digit digitOne; //ref to first digit in healthbar readout
    [SerializeField] private Digit digitTwo; //ref to second digit in healthbar number readout

    [SerializeField] private GameObject tickPrefab; //prefab for individual ticks
    [SerializeField] private Sprite tick1;
    [SerializeField] private Sprite tick2;

    private const int numTicks = 64; //pixel width of the actual healthbar
    private int playerHealth; //ref to player object's health
    private int playerMaxHealth; //ref to player object's max health

    private void Start()
    {
        //get the player's health and maxHealth
        playerHealth = Player.GetComponent<IHealthObject>().Health;
        playerMaxHealth = Player.GetComponent<IHealthObject>().MaxHealth;

        GameObject tick;
        //fill the healthbar with ticks
        for (int i = 0; i < numTicks; i++)
        {
            tick = Instantiate(tickPrefab, transform);
            tick.name = "healthBarTick" + i;
            tick.GetComponent<SpriteRenderer>().sprite = (i < 5 || (i > 5 && i < 8) || i > 62 ) ? tick1 : tick2; //custom design
            tick.transform.position = transform.position + new Vector3(2.0625f + (i*0.125f), 0); //needs modifying to be more generalized <<<
        }
        //healthbar should always start filled
        SetActive();
    }

    // Update is called once per frame
    void Update () {
        //get the player's health
        playerHealth = Player.GetComponent<IHealthObject>().Health;

        //set the digits
        digitOne.SetNumber(playerHealth < 10 ? 10 : (playerHealth - (playerHealth % 10)) / 10);
        digitTwo.SetNumber(playerHealth % 10);

        //Adjust ticks on the healthbar 
        SetActive();
    }

    /// <summary>
    /// set the appropriate number of health ticks active and inactive based on player health / players max health
    /// </summary>
    private void SetActive()
    {
        int childOffset = transform.childCount - numTicks;
        float convertedHealth = playerHealth * numTicks / playerMaxHealth;

        //loop through all gameobjects
        for (int i = 1; i <= numTicks; i++)
        {
            transform.GetChild(i + childOffset - 1).gameObject.SetActive(i <= convertedHealth);
        }
    }
}
