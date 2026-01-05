using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardSkillBox : MonoBehaviour
{
    // Script này dùng để gắn lên các cục skill Box, dùng để định danh tốt hơn
    public int Order; // 1, 2, 3, gắn ở bên ngoài nên là public
    private int CurrentSkillID = 1;
    private string[] SkillName = { "Star Sequence", "Astral Vortex", "Fiery Wrath"};
    private Color32[] NameColor = { new Color32(255, 165, 0, 255), new Color32(255, 0, 234, 255), new Color32(255, 83, 0, 255) };
    public TextMeshProUGUI SkillText;
    void Start()
    {
        // Show Skill id của con wizard hiện tại, ở wizard combo customizer nhé
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowNameByID(int ID)
    {
        SkillText.text = SkillName[ID - 1];
        SkillText.color = NameColor[ID - 1];
    }
    public void MoveUp()
    {
        if (CurrentSkillID < 3)
        {
            CurrentSkillID++;
        }
        else
        {
            CurrentSkillID = 1;
        }
        ShowNameByID(CurrentSkillID);
    }
    public void MoveDown()
    {
        if (CurrentSkillID > 1)
        {
            CurrentSkillID--;
        }
        else
        {
            CurrentSkillID = 3;
        }
        ShowNameByID(CurrentSkillID);
    }
    public int GetCurrentSkillID() { return CurrentSkillID; }
}
