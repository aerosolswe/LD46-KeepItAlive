using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour {

    public static EconomyManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public int baseMoney = 50;
    public int baseIncome = 50;

    private int money = 0;
    private int income = 0;

    private CultureInfo usCulture;

    public void SetBaseEconomy() {
        Money = baseMoney;
        Income = baseIncome;
    }

    public int Income {
        get {
            return income;
        }
        set {
            income = value;

            if (income < 0) {
                income = 0;
            }

            string formattedIncome = FormatDollar(income);
            UIManager.instance.incomeText.text = $"+{formattedIncome}";
        }
    }

    public int Money {
        get {
            return money;
        }
        set {
            money = value;

            if (money < 0) {
                money = 0;
            }

            UIManager.instance.buildingButton.interactable = money >= UIManager.instance.BuildingCost;
            UIManager.instance.jetButton.interactable = money >= UIManager.instance.JetCost;

            string formattedMoney = FormatDollar(money);
            UIManager.instance.moneyText.text = $"{formattedMoney}";
        }
    }

    public string FormatDollar(int amount) {
        if (usCulture == null) {
            usCulture = new CultureInfo("en-US");
        }

        return amount.ToString("C", usCulture);
    }
    
}
