using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
public class CombinationLock : MonoBehaviour
{
    [SerializeField] XRButtonInteractable[] comboButtons;
    [SerializeField] Image lockedPennel;
    [SerializeField] Color unlockedColor;
    [SerializeField] Color lockedColor;
    [SerializeField] bool isLocked;
    [SerializeField] int[] comboValues=new int[3];//最终答案
    [SerializeField] int[] inputValues ;//输入答案
    [SerializeField] TMP_Text userInputText;
    [SerializeField] TMP_Text lockedText;
    [SerializeField] bool isResettable;//是否开启密码重置功能
    private bool resetCombo;
    private int maxButtonPresses;
    private int ButtonPresses;
    private const string unlockedString="unlocked";
    private const string lockedString = "locked";
    // Start is called before the first frame update
    void Start()
    {
        maxButtonPresses = comboValues.Length;
        ResetUserValues();
        
        for (int i = 0; i < comboButtons.Length; i++) { comboButtons[i].selectEntered.AddListener(OnComboButtonPressed); }

    }
    private void OnComboButtonPressed(SelectEnterEventArgs arg0)
    {
        if (ButtonPresses >= maxButtonPresses)
        {
            Debug.Log("***数字超了");
        }
        else
        {
            for (int i = 0; i < comboButtons.Length; i++)
            {
                if (arg0.interactableObject.transform.name == comboButtons[i].transform.name)
                {
                    userInputText.text += i.ToString();
                    inputValues[ButtonPresses] = i;
                }
                else { comboButtons[i].ResetColor(); }
            }
            ButtonPresses++;
            if (ButtonPresses == maxButtonPresses)
            {
                CheckCombo();
            }
        }
        
    }
    private void CheckCombo()
    {
        if (resetCombo)
        {
            resetCombo = false;
            LockCombo();
            return;
        }

        int matches = 0;
        for (int i = 0; i < maxButtonPresses; i++)
        {
            if (inputValues[i] == comboValues[i]) 
            { matches++; }
        }
        if (matches ==maxButtonPresses)
        {
            //isLocked = false;
            //lockedPennel.color=unlockedColor;
            //lockedText.text=unlockedString;
            UnlockCombo();


        }
        else 
        {
            ResetUserValues();
        }
    }
    private void ResetUserValues()
    {
        inputValues = new int[maxButtonPresses];
        userInputText.text = "";
        ButtonPresses = 0;
    }

    private void LockCombo()
    {
       
        for (int i = 0; i < maxButtonPresses; i++)
        { comboValues[i] = inputValues[i]; }
        isLocked = true;
        lockedPennel.color = lockedColor;
        lockedText.text = lockedString;
        ResetUserValues();
    }
    private void UnlockCombo()//解锁
    { isLocked = false;
        lockedPennel.color = unlockedColor;
        lockedText.text = unlockedString;
        if (isResettable) { ResetCombo(); }
    }

    private void ResetCombo() 
    {
        ResetUserValues();
        resetCombo = true;
    }//重箐密码锁






}
