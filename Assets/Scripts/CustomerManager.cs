using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    //customer gameobject
    public GameObject customer;

    //customerslots class to get info of slots empty or not
    public CustomerSlots[] customerslots;
    //[HideInInspector]
    public List<GameObject> customers;

    void Start()
    {
        instance = this;
        IntilizeCustomers();
    }
    /// <summary>
    /// intilize customers at  slot positions.
    /// </summary>
    void IntilizeCustomers()
    {
        for (int i = 0; i < customerslots.Length; i++)
        {
            GameObject customerRefernce = Instantiate(customer, customerslots[i].customerslot.position, Quaternion.identity, transform);
            customerRefernce.GetComponent<Customer>().CustomerSlot = i;
            customerslots[i].isSlotFilled = true;
            if(customerRefernce!=null)
            customers.Add(customerRefernce);
        }
    }

    /// <summary>
    /// Add customers to empty slot
    /// </summary>
    /// <returns></returns>
    IEnumerator AddMoreCustomers()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < customerslots.Length; i++)
        {
            if (!customerslots[i].isSlotFilled)
            {
                customers[i].SetActive(true);
                customerslots[i].isSlotFilled = true;
            }
        }
    }
    /// <summary>
    /// add customer after every left  or served customer
    /// </summary>
    public void AddCustomersToEmptySlot()
    {
       StartCoroutine(AddMoreCustomers());
    }

}

[System.Serializable]
public class CustomerSlots
{
    public Transform customerslot;
    public bool isSlotFilled=false;
}