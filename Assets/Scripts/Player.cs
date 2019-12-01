using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Keys for player   
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;
    public KeyCode UpKey = KeyCode.W;
    public KeyCode DownKey = KeyCode.S;
    public KeyCode PickKey = KeyCode.E;
    public KeyCode DropKey = KeyCode.R;
    //player speed
    public int playerSpeed;

    //playerPickedVegetables
    public List<char> playerPickedVegetables;

    //playerChoppedVegetables
    public List<char> playerChoppedVegetables;

    public int playerScore, playertime;

    //speed powerUp count
    private int playerPowerUpSpeed=20;

    [SerializeField]
    private TextMesh playerVegetablesUI, choppedVegetablesUI;

    [SerializeField]//vegetableChoppingProgress bar
    private GameObject vegetableChoppingProgress;

    [SerializeField]//choppingtime
    private float vegetableChoppingTime;

    [SerializeField]//maxVegetablesPlayer an hold
    private int maxVegetablesPlayerHolds = 2; //maxChoppedVegetables = 3;

    //float timeToChopVegetable;
    public bool canPlayerMove;
    bool ischopping = false;

    //Player Directions changed input
    Vector3 direction;
    private void OnEnable()
    {
        InvokeRepeating("PlayerTimer", 0.01f, 1f);
    }
    void Update()
    {
        if (canPlayerMove)
            MovePlayer();
    }
    /// <summary>
    /// Plyer Movements
    /// </summary>
    void MovePlayer()
    {
        direction = Vector3.zero;
        if (Input.GetKey(UpKey))
        {
            direction = Vector3.up;
            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(DownKey))
        {
            direction = Vector3.down;
            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(LeftKey))
        {
            direction = Vector3.left;
            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(RightKey))
        {
            direction = Vector3.right;
            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }

    }
    /// <summary>
    /// Get Interaction with All intractives in scene.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(PickKey))
        {
            #region Vegetables
            if (other.gameObject.tag == "Vegetables")
            {
                if (playerPickedVegetables.Count < maxVegetablesPlayerHolds)
                    AddVegetables(other.gameObject.GetComponent<Vegetable>().vegetableName);
            }
            #endregion

            #region Chopping
            if (other.gameObject.tag == "Chopping")
            {
                if (!ischopping && playerPickedVegetables.Count > 0)
                    StartCoroutine(StartChoppingVegetable());
            }
            #endregion

            #region ServingCustomers
            if (other.gameObject.tag == "Customer"&&playerChoppedVegetables.Count>0)
            {
                servingFoodToCustomer(other.gameObject);
            }
            #endregion

            #region DustBin
            if (other.gameObject.tag == "DustBin" && playerChoppedVegetables.Count > 0)
            {
                ThrownFoodInDustBin();
            }
            #endregion

            #region Plate
            if (other.gameObject.tag == "Plate" )
            {
                Plate plate = other.gameObject.GetComponent<Plate>();
                if (!plate.isPlateFilled && playerPickedVegetables.Count > 0)
                {
                    plate.isPlateFilled = true;
                    plate.vegetableName = playerPickedVegetables[0];
                    plate.placedVegetableName.text = playerPickedVegetables[0].ToString();
                    playerPickedVegetables.Remove(playerPickedVegetables[0]);
                    UpdatePlayerVegetableDetails();
                }
                else
                {
                    plate.isPlateFilled = false;
                    plate.placedVegetableName.text = "--";
                    playerPickedVegetables.Add(plate.vegetableName);
                    plate.vegetableName ='*';
                    UpdatePlayerVegetableDetails();
                }
            }
            #endregion
        }
        #region PowerUps
        if (other.gameObject.tag == "PowerUp")
        {
            if (other.gameObject.name == "TimerPowerUp")
            {
                PlayerManager.instance.ActivateTimerPower(this);
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.name == "SpeedPowerUp")
            {
                PlayerManager.instance.ActivateSpeedPowerToPlayer(this);
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.name == "ScorePowerUp")
            {
                PlayerManager.instance.ActivateScorePower(this);
                other.gameObject.SetActive(false);
            }
        }
        #endregion
    }
    /// <summary>
    /// Add vegetables to playerPickedVegetables list
    /// </summary>
    /// <param name="vegetable"></param>
    void AddVegetables(char vegetable)
    {
        playerPickedVegetables.Add(vegetable);
        UpdatePlayerVegetableDetails();
    }

    /// <summary>
    /// UpdatePlayerVegetableDetails   
    /// </summary>
    void UpdatePlayerVegetableDetails()
    {
        if (playerPickedVegetables.Count <= 0)
        {
            playerVegetablesUI.text = "---";
        }

        if (playerChoppedVegetables.Count <= 0)
        {
            choppedVegetablesUI.text = "---";
        }

        string tempVegetableUIText = "";
        foreach (char vegetable in playerPickedVegetables)
        {
            tempVegetableUIText += vegetable + ", ";
        }

        if (tempVegetableUIText != "") playerVegetablesUI.text = tempVegetableUIText;

        tempVegetableUIText = "";
        for (int i = 0; i < playerChoppedVegetables.Count; i++)
        {
            tempVegetableUIText += playerChoppedVegetables[i] + ", ";
        }
        if (tempVegetableUIText != "") choppedVegetablesUI.text = tempVegetableUIText;
    }
    /// <summary>
    /// Player chopping vegetables
    /// </summary>
    /// <returns></returns>
    IEnumerator StartChoppingVegetable()
    {
        canPlayerMove = false;
        ischopping = true;
        vegetableChoppingProgress.SetActive(true);
        float timer = vegetableChoppingTime;
        while (vegetableChoppingTime > 0)
        {
            vegetableChoppingTime -= Time.deltaTime * 2;
            vegetableChoppingProgress.transform.localScale = new Vector3(vegetableChoppingTime / timer, 1, 1);
            yield return new WaitForSeconds(0.01f);
        }
        canPlayerMove = true;
        ischopping = false;
        vegetableChoppingProgress.SetActive(false);
        playerChoppedVegetables.Add(playerPickedVegetables[0]);
        playerPickedVegetables.Remove(playerPickedVegetables[0]);

        UpdatePlayerVegetableDetails();
        vegetableChoppingTime = timer;
    }
    /// <summary>
    /// ServingFoodtoCustomer check right or wrong
    /// </summary>
    bool correctItemsServed;
    void servingFoodToCustomer(GameObject customer)
    {
        List<char> customerfood = customer.GetComponent<Customer>().customerFoodOrder;
        if(playerChoppedVegetables.Count==customerfood.Count)
        foreach (var item in playerChoppedVegetables)
        {
            if (customerfood.Contains(item))
            {
                correctItemsServed = true;
            }
            else
            {
               correctItemsServed = false;
               break;
            }
        }
        else
        {
            correctItemsServed = false;
        }

        if (correctItemsServed)
        {
            AddScoreToPlayer(10);
            AddTimeToPlayer(10);
            customer.GetComponent<Customer>().CustomerLeft();
            print("CustomerHappy");
            playerChoppedVegetables.Clear();
            UpdatePlayerVegetableDetails();
            if (customer.GetComponent<Customer>().serveTime >= 0.7f)
            {
                customer.GetComponent<Customer>().RewardPlayerForFastServe();
            }
        }
        else
        {
            playerChoppedVegetables.Clear();
            UpdatePlayerVegetableDetails();
            customer.GetComponent<Customer>().inCorrectFoodServedPenalize += 2;
            customer.GetComponent<Customer>().customerProgressSprite.color = Color.red;
            PlayerManager.instance.PenalizePlayer();
        }
    }
    /// <summary>
    /// Adds  score to player
    /// </summary>
    /// <param name="score"></param>
    void AddScoreToPlayer(int score)
    {
        playerScore += score;
        PlayerManager.instance.UpdatePlayerScore();
    }
    /// <summary>
    /// Adds  time to player
    /// </summary>
    /// <param name="time"></param>
    void AddTimeToPlayer(int time)
    {
        playertime += time;
        PlayerManager.instance.UpdatePlayerTimer();
    }
    /// <summary>
    /// Player time counter function
    /// </summary>
    void PlayerTimer()
    {
        if (playertime >= 0)
        {
            AddTimeToPlayer(-1);
        }
        else
        {
            playertime = 0;
            PlayerManager.instance.checkTimeIsComplete();
            CancelInvoke("PlayerTimer");
        }
    }
    /// <summary>
    /// ThrownFoodInDustBin
    /// </summary>
    void ThrownFoodInDustBin()
    {
        playerChoppedVegetables.Clear();
        AddScoreToPlayer(-10);
        PlayerManager.instance.UpdatePlayerScore();
        UpdatePlayerVegetableDetails();
    }

}
