using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    //total vegetable Array
    public char[] foodChar;

    //to store  customer order food 
    public List<char> customerFoodOrder;

    //customer food displaytext
    public TextMesh customerFoodDisplaytext;

    public char food1, food2, food3;
    //waiting time
    public float customerWaitingTime;

    // customer Progress  transfrom
    public Transform customerProgressBar;

    //customer current slot
    public int CustomerSlot;

    //decrease progress on wrong foodserved
    public int inCorrectFoodServedPenalize = 2;

    //to change Progress bar color
    public SpriteRenderer customerProgressSprite;

    [HideInInspector]//how  much time did customerwaited
    public float serveTime;
    void OnEnable()
    {
        customerProgressSprite.color = Color.white;
        GetfoodforCustomer();
        StartCoroutine(CustomerWaitingTime());
    }
    void Start()
    {

    }
    int prevRandomNumber;
    /// <summary>
    /// gives random numbers form 1to3
    /// </summary>
    /// <returns></returns>
    int GetRandomNumber()
    {
        int randomnumber = Random.Range(1, 4);
        if (randomnumber != prevRandomNumber)
        {
            prevRandomNumber = randomnumber;
            return randomnumber;
        }
        else
        {
            return GetRandomNumber();
        }
    }
    /// <summary>
    /// RandomFoodGenerater
    /// </summary>
    /// <returns></returns>
    int RandomFoodGenerter()
    {
        int randomnumber = Random.Range(0, foodChar.Length);

        if (randomnumber != prevRandomNumber)
        {
            prevRandomNumber = randomnumber;
            return randomnumber;
        }
        else
        {
            return RandomFoodGenerter();
        }
    }

    /// <summary>
    /// GetfoodforCustomer for customer order count
    /// </summary>
    public void GetfoodforCustomer()
    {
        switch (GetRandomNumber())
        {
            case 1:
                GenerateFoodWithMenuForCurrentCustomer(RandomFoodGenerter());
                break;
            case 2:
                GenerateFoodWithMenuForCurrentCustomer(RandomFoodGenerter(), RandomFoodGenerter());
                break;
            case 3:
                GenerateFoodWithMenuForCurrentCustomer(RandomFoodGenerter(), RandomFoodGenerter(), RandomFoodGenerter());
                break;
        }
    }

    void GenerateFoodWithMenuForCurrentCustomer(int food1)
    {
        this.food1 = GetFoodByNumbers(food1);
        customerFoodOrder.Add(this.food1);
        string temp = "";
        for (int i = 0; i < customerFoodOrder.Count; i++)
        {
            temp += customerFoodOrder[i] + "";
        }
        customerFoodDisplaytext.text = temp;
        customerWaitingTime = customerWaitingTime * customerFoodOrder.Count;
    }

    void GenerateFoodWithMenuForCurrentCustomer(int food1, int food2)
    {
        this.food1 = GetFoodByNumbers(food1);
        this.food2 = GetFoodByNumbers(food2);
        customerFoodOrder.Add(this.food1);
        customerFoodOrder.Add(this.food2);
        string temp = "";
        for (int i = 0; i < customerFoodOrder.Count; i++)
        {
            temp += customerFoodOrder[i] + ",";
        }
        customerFoodDisplaytext.text = temp;
        customerWaitingTime = customerWaitingTime * customerFoodOrder.Count;
    }

    void GenerateFoodWithMenuForCurrentCustomer(int food1, int food2, int food3)
    {
        this.food1 = GetFoodByNumbers(food1);
        this.food2 = GetFoodByNumbers(food2);
        this.food3 = GetFoodByNumbers(food3);
        customerFoodOrder.Add(this.food1);
        customerFoodOrder.Add(this.food2);
        customerFoodOrder.Add(this.food3);
        string temp = "";
        for (int i = 0; i < customerFoodOrder.Count; i++)
        {
            temp += customerFoodOrder[i] + ",";
        }
        customerFoodDisplaytext.text = temp;
        customerWaitingTime = customerWaitingTime * customerFoodOrder.Count;
    }
    /// <summary>
    /// gets food by int
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    char GetFoodByNumbers(int num)
    {
        return foodChar[num];
    }
    /// <summary>
    /// customer waiting function
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomerWaitingTime()
    {
        float timer = customerWaitingTime;
        while (customerWaitingTime > 0)
        {
            customerWaitingTime -= Time.deltaTime * inCorrectFoodServedPenalize;
            serveTime = (customerWaitingTime / timer);
            customerProgressBar.localScale = new Vector3(customerWaitingTime / timer, 1, 1);
            yield return new WaitForSeconds(0.02f);
        }
        print("Customer went");
        customerWaitingTime = timer;
        CustomerLeft();
    }

    /// <summary>
    /// customer left
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomerWaitAndLeft()
    {
        CustomerManager.instance.customerslots[CustomerSlot].isSlotFilled = false;
        CustomerManager.instance.AddCustomersToEmptySlot();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        customerFoodOrder.Clear();
    }

    public void CustomerLeft()
    {
        StartCoroutine(CustomerWaitAndLeft());
    }
    /// <summary>
    /// Reward to player when customer progress  >=0.7
    /// </summary>
    public void RewardPlayerForFastServe()
    {
        int rewardNumber = GetRandomNumber();
        if (rewardNumber == 1)
        {
            PlayerManager.instance.speedPower.SetActive(true);
        }
        else if (rewardNumber == 2)
        {
            PlayerManager.instance.timerPower.SetActive(true);
        }
        else if (rewardNumber == 3)
        {
            PlayerManager.instance.scorePower.SetActive(true);
        }
    }

}
