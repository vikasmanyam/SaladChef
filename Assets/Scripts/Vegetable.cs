using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    public TextMesh vegetableText;
    [HideInInspector]
    public char vegetableName;

    void Start()
    {
        vegetableName = vegetableText.text.ToString()[0];
    }

}
