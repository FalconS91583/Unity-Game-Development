using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;

    [SerializeField] private TextMeshProUGUI displayBalance;

    [SerializeField] private int currentBalance;
    public int CurreentBalance { get { return currentBalance;  } }

    private void Awake()
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amout)
    {
        currentBalance += Mathf.Abs(amout);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance;
    }

    public void Withdraw(int amout)
    {
        currentBalance -= Mathf.Abs(amout);
        UpdateDisplay();

        if (currentBalance < 0)
        {
            SceneManager.LoadScene(0);
        }
    }

}
