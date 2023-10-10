using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Player_PanelManager : MonoBehaviour
{
    #region #싱글턴
    public static Player_PanelManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Text text_StageNumber;
    public Text text_Wave_current;
    public Text text_currentHP;
    public Text text_maxHP;
    public Slider gage_HP;

    // Start is called before the first frame update
    void Start()
    {
        text_StageNumber.text = GameObject.FindGameObjectWithTag("StageNumber").name;
    }

    internal void UpdateHPGage(int playerCurrentHp, int playerMaxHp)
    {
        float value = (float)playerCurrentHp / (float)playerMaxHp;
        text_maxHP.text = $"/ {playerMaxHp}";
        text_currentHP.text = $"{playerCurrentHp} ";
        gage_HP.value = value;
        print($"{playerCurrentHp} / {playerMaxHp}");
        print(value);
    }
    internal void UpdateHPGage_Heal(int playerCurrentHp, int playerMaxHp)
    {
        float value = (float)playerCurrentHp / (float)playerMaxHp;
        text_maxHP.text = $"/ {playerMaxHp}";
        text_currentHP.text = $"{playerCurrentHp} ";
        gage_HP.value = value;
        print($"{playerCurrentHp} / {playerMaxHp}");
        print(value);
    }

    internal void ChangeWaveNumber(string Wave)
    {
        text_Wave_current.text = $"{Wave}";
    }

}
