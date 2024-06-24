using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DinheiroManager : MonoBehaviour
{
    public int dinheiro = 200;
    public TextMeshProUGUI dinheiroText;
    private Request request;

    void Update()
    {
        dinheiroText.text = "Dinheiro: " + dinheiro + " €";
    }
}
