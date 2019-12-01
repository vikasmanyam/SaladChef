using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {

    //to check plate is filled
    public bool isPlateFilled;

    //played vegetable text
    public TextMesh placedVegetableName;

    //played vegetable Name
    public char vegetableName;

    private void Start()
    {
        placedVegetableName.text = "--";
    }

}
