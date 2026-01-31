using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardComboCustomizer : MonoBehaviour
{
    private Wizard currentWizard;
    public WizardSkillBox[] SkillBoxes;
    public static WizardComboCustomizer instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseUI()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (CharacterUIControll.instance != null)
        {
            CharacterUIControll.instance.UI_Off();
        }
    }
    public void SetCurrentWizard(Wizard wizard)
    {
        currentWizard = wizard;
    }
    public void Confirm()
    {
        float totalDamage = 0f;
        for (int i = 0; i < SkillBoxes.Length; i++)
        {
            currentWizard.SkillOrderID[i] = SkillBoxes[i].GetCurrentSkillID();
            totalDamage += SkillBoxes[i].GetDamageByID();
        }
        currentWizard.SetVirtualDamage(totalDamage);
        CloseUI();
    }
    public void ShowCurrentWizardSkillOrder()
    {
        if (currentWizard != null)
        {
            for (int i = 0; i < SkillBoxes.Length; i++)
            {
                SkillBoxes[i].ShowNameByID(currentWizard.SkillOrderID[i]);
            }
        }
    }
}
