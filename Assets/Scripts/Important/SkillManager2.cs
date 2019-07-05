using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager2 : MonoBehaviour
{
    /*SMB - Smite Button
    SMDuB - Smite Duration Button
    SMDaB - Smite Damage Button
    ASB - Attack Speed Button
    LSB - Life Steal Button
    AAB - Air Attack Button
    HRB - Heath Regen Button
    MSB - Move Speed Button
    DaMB - Damage with Move
    TJB - Triple Jump Button
    TuB - Turtle Button
    TuDeB - Turtle Defense Button
    TuDuB - Turtle Duration Button
    GWB - Grip Wall Button
    JHB - Jump Height Button
    IKB - Instant Kill Button
    WDVB - Weapon Drop Value Button
    */
    public Color pressedColor;
    public Color normalColor;
    public Color highlightedColor;
    public Color disabledColor;
    public Color maxedColor;
    public Color unlockedColor;
    public Color turnedOffColor;
    private ColorBlock colorBlock;
    public Button SmiteB, SmiteDurationB, SmiteDamageB, AttackSpeedB, LifeStealB, 
    AirAttackB, HealthBonusB, MoveSpeedB, MoveDefenceB, TripleJumpB, TurtleB, TurtleDefenseB,
    TurtleDurationB, GripWallsB, JumpHeightB, InstantKillB, WeaponDropValueB;
    public List<Button> buttons = new List<Button>();

    public Text HR2Tu, HR2DaM, MS2DaM, MS2TJ, MS2AA, HR2WLS, AS2AA, AS2LS, AS2SM, SM2SMDa, SM2SMDu, Tu2TuDe, Tu2TuDu, TJ2GW, TJ2JH, IK, WDV;

    private void Awake()
    {
        colorBlock.normalColor = normalColor;
        colorBlock.pressedColor = pressedColor;
        colorBlock.disabledColor = disabledColor;
        colorBlock.highlightedColor = highlightedColor;
        colorBlock.colorMultiplier = 1;

        //finds all the buttons
        SmiteB = gameObject.transform.Find("SMB").GetComponent<Button>();
        SmiteDurationB = gameObject.transform.Find("SMDuB").GetComponent<Button>();
        SmiteDamageB = gameObject.transform.Find("SMDaB").GetComponent<Button>();
        AttackSpeedB = gameObject.transform.Find("ASB").GetComponent<Button>();
        LifeStealB = gameObject.transform.Find("LSB").GetComponent<Button>();
        AirAttackB = gameObject.transform.Find("AAB").GetComponent<Button>();
        HealthBonusB = gameObject.transform.Find("HRB").GetComponent<Button>();
        MoveSpeedB = gameObject.transform.Find("MSB").GetComponent<Button>();
        MoveDefenceB = gameObject.transform.Find("DaMB").GetComponent<Button>();
        TripleJumpB = gameObject.transform.Find("TJB").GetComponent<Button>();
        TurtleB = gameObject.transform.Find("TuB").GetComponent<Button>();
        TurtleDefenseB = gameObject.transform.Find("TuDeB").GetComponent<Button>();
        TurtleDurationB = gameObject.transform.Find("TuDuB").GetComponent<Button>();
        GripWallsB = gameObject.transform.Find("GWB").GetComponent<Button>();
        JumpHeightB = gameObject.transform.Find("JHB").GetComponent<Button>();
        InstantKillB = gameObject.transform.Find("IKB").GetComponent<Button>();
        WeaponDropValueB = gameObject.transform.Find("WDVB").GetComponent<Button>();

        //finds the locks
        HR2Tu = transform.Find("HR2Tu").transform.Find("Text").GetComponent<Text>();
        HR2DaM = transform.Find("HR2DaM").transform.Find("Text").GetComponent<Text>(); 
        MS2DaM = transform.Find("MS2DaM").transform.Find("Text").GetComponent<Text>(); 
        MS2TJ = transform.Find("MS2TJ").transform.Find("Text").GetComponent<Text>(); 
        MS2AA = transform.Find("MS2AA").transform.Find("Text").GetComponent<Text>(); 
        HR2WLS = transform.Find("HR2LS").transform.Find("Text").GetComponent<Text>(); 
        AS2AA = transform.Find("AS2AA").transform.Find("Text").GetComponent<Text>(); 
        AS2LS = transform.Find("AS2LS").transform.Find("Text").GetComponent<Text>(); 
        AS2SM = transform.Find("AS2SM").transform.Find("Text").GetComponent<Text>();
        SM2SMDa = transform.Find("SM2SMDa").transform.Find("Text").GetComponent<Text>();
        SM2SMDu = transform.Find("SM2SMDu").transform.Find("Text").GetComponent<Text>();
        Tu2TuDe = transform.Find("Tu2TuDe").transform.Find("Text").GetComponent<Text>();
        Tu2TuDu = transform.Find("Tu2TuDu").transform.Find("Text").GetComponent<Text>();
        TJ2GW = transform.Find("TJ2GW").transform.Find("Text").GetComponent<Text>();
        TJ2JH = transform.Find("TJ2JH").transform.Find("Text").GetComponent<Text>();
        IK = transform.Find("IK").transform.Find("Text").GetComponent<Text>();
        WDV = transform.Find("WDV").transform.Find("Text").GetComponent<Text>();


        //adds all the buttons
        AddButtons(SmiteB, SmiteDurationB, SmiteDamageB, AttackSpeedB, LifeStealB,
        AirAttackB, HealthBonusB, MoveSpeedB, MoveDefenceB, TripleJumpB, TurtleB, TurtleDefenseB,
        TurtleDurationB, GripWallsB, JumpHeightB, InstantKillB, WeaponDropValueB);
        
   
}

    void Start()
    {
        foreach(Button b in buttons)
        {
            b.onClick.AddListener(delegate { onButtonClick(b.GetComponent<SkillButtons>()._name); } );
        }
        CheckButtons();
    }

    void onButtonClick(string _name)
    {
        Button tempButton = Translate(_name);
        SkillButtons skillButtons = tempButton.GetComponent<SkillButtons>();
        if(skillButtons.skillPointCost <= PersistantGameManager.Instance.playerStats.playerSkillPoints)
        {
            PersistantGameManager.Instance.skillLevels[_name]++;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= skillButtons.skillPointCost;
        }
        PersistantGameManager.Instance.CheckSkills();
        FindObjectOfType<skillDescription>().transform.Find("Lvl").GetComponent<Text>().text = "Level: " + PersistantGameManager.Instance.skillLevels[_name] + "/" + Translate(_name).GetComponent<SkillButtons>().maxLevel + ".";
        CheckButtons();

    }

    //Updates all buttons and sets all buttons to their correct state.
    void CheckButtons()
    {

        foreach (Button b in buttons)
        {
            if ((b.gameObject.name == "IKB" && !(InstantKillB.GetComponent<SkillButtons>().nessesaryXPLevel <= PersistantGameManager.Instance.playerStats.playerLevel)) || (b.gameObject.name == "WDVB" && !(WeaponDropValueB.GetComponent<SkillButtons>().nessesaryXPLevel <= PersistantGameManager.Instance.playerStats.playerLevel)))
            {

                InstantKillB.interactable = false;
                b.GetComponent<Image>().color = turnedOffColor;
                SkillButtons requirements = b.GetComponent<SkillButtons>();
                b.transform.Find("Text").GetComponent<Text>().text = requirements.text + "\n(" + requirements.skillPointCost + " S.P)   (" + PersistantGameManager.Instance.skillLevels[requirements._name] + "/" + requirements.maxLevel + ")";

            }
            else
            {
                bool shouldUnlock = false;
                int index = 0;
                SkillButtons requirements = b.GetComponent<SkillButtons>();
                foreach (string s in requirements.toBeUnlocked)
                {
                    if (requirements.unlocked)
                    {
                        shouldUnlock = true;
                    }
                    if (PersistantGameManager.Instance.skillLevels[s] > requirements.levelToUnlock - 1)
                    {
                        shouldUnlock = true;
                    }
                    index++;
                }
                if (requirements.toBeUnlocked.Length == 0)
                {
                    shouldUnlock = true;
                }
                if (PersistantGameManager.Instance.skillLevels[requirements._name] > 0 && shouldUnlock)
                {
                    b.interactable = true;
                    b.GetComponent<Image>().color = unlockedColor;

                    if (PersistantGameManager.Instance.skillLevels[requirements._name] >= requirements.maxLevel)
                    {
                        b.GetComponent<Image>().color = maxedColor;
                        b.interactable = false;
                    }

                }
                else if (shouldUnlock || requirements.unlocked)
                {
                    b.interactable = true;
                    requirements.unlocked = true;
                    b.GetComponent<Image>().color = normalColor;
                }
                else
                {
                    b.interactable = false;
                    b.GetComponent<Image>().color = turnedOffColor;
                }

                if (requirements.maxLevel > 1)
                {
                    b.transform.Find("Text").GetComponent<Text>().text = requirements.text + "\n(" + requirements.skillPointCost + " S.P)   (" + PersistantGameManager.Instance.skillLevels[requirements._name] + "/" + requirements.maxLevel + ")";
                }
                else
                {
                    b.transform.Find("Text").GetComponent<Text>().text = requirements.text + "\n(" + requirements.skillPointCost + " S.P)";
                }
            }
           
        }
        //Sets Locks to correct number
        //Health Regen To Turtle
        int HR2TuV = TurtleB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["HealthBonus"];
        if (HR2TuV < 1 || TurtleB.GetComponent<SkillButtons>().unlocked)
        {
            HR2Tu.transform.parent.gameObject.SetActive(false);
        }
        else 
        {
            HR2Tu.text = HR2TuV.ToString();
        }
        //Health Regen To Damage On Movement
        int HR2DaMV = MoveDefenceB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["HealthBonus"];
        if (HR2DaMV < 1 || MoveDefenceB.GetComponent<SkillButtons>().unlocked)
        {
            HR2DaM.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            HR2DaM.text = HR2DaMV.ToString();
        }

        //Health Regen to Weapon Drop Value
        //int HR2WDVV = WeaponDropValueB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["HealthBonus"];
        //if (HR2WDVV < 1 || WeaponDropValueB.GetComponent<SkillButtons>().unlocked) {}


        //Health Regen to Life Steal
        int HR2LSV = LifeStealB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["HealthBonus"];
        if (HR2LSV < 1 || LifeStealB.GetComponent<SkillButtons>().unlocked)
        {
            HR2WLS.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            HR2WLS.text = HR2LSV.ToString();
        }
        //Move Speed to Damage On Movement
        int MS2DaMV = MoveDefenceB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["MoveSpeed"];
        if (MS2DaMV < 1 || MoveDefenceB.GetComponent<SkillButtons>().unlocked)
        {
            MS2DaM.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            MS2DaM.text = MS2DaMV.ToString();
        }
        //Move Speed to triple Jump
        int MS2TJV = TripleJumpB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["MoveSpeed"];
        if (MS2TJV < 1 || TripleJumpB.GetComponent<SkillButtons>().unlocked)
        {
            MS2TJ.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            MS2TJ.text = MS2TJV.ToString();
        }
        //Move Speed to Air Attack
        int MS2AAV = AirAttackB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["MoveSpeed"];
        if (MS2AAV < 1 || AirAttackB.GetComponent<SkillButtons>().unlocked)
        {
            MS2AA.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            MS2AA.text = MS2AAV.ToString();
        }
        //Attack Speed to Air Attack
        int AS2AAV = AirAttackB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["AttackSpeed"];
        if (AS2AAV < 1 || AirAttackB.GetComponent<SkillButtons>().unlocked)
        {
            AS2AA.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            AS2AA.text = AS2AAV.ToString();
        }
        //Attack Speed to Life Steal
        int AS2WLSV = LifeStealB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["AttackSpeed"];
        if (AS2WLSV < 1 || LifeStealB.GetComponent<SkillButtons>().unlocked)
        {
            AS2LS.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            AS2LS.text = AS2WLSV.ToString();
        }
        //Attack Speed to Smite
        int AS2SMV = SmiteB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["AttackSpeed"];
        if (AS2SMV < 1 || SmiteB.GetComponent<SkillButtons>().unlocked)
        {
            AS2SM.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            AS2SM.text = AS2SMV.ToString();
        }
        //Smite to Smite Damage
        int SM2SMDaV = SmiteDamageB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["Smite"];
        if (SM2SMDaV < 1 || SmiteDamageB.GetComponent<SkillButtons>().unlocked)
        {
            SM2SMDa.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            SM2SMDa.text = SM2SMDaV.ToString();
        }
        //Smite to Smite Duration

        int SM2SMDuV = SmiteDurationB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["Smite"];
        if (SM2SMDuV < 1|| SmiteDurationB.GetComponent<SkillButtons>().unlocked)
        {
            SM2SMDu.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            SM2SMDu.text = SM2SMDuV.ToString();
        }
        //Turtle to Turtle Defense

        int Tu2TuDeV = TurtleDefenseB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["Turtle"];
        if (Tu2TuDeV < 1 || TurtleDefenseB.GetComponent<SkillButtons>().unlocked)
        {
            Tu2TuDe.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Tu2TuDe.text = Tu2TuDeV.ToString();
        }
        //Turtle to Turtle Duration

        int Tu2TuDuV = TurtleDurationB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["Turtle"];
        if (Tu2TuDuV < 1 || TurtleDurationB.GetComponent<SkillButtons>().unlocked)
        {
            Tu2TuDu.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Tu2TuDu.text = Tu2TuDuV.ToString();
        }
        //Triple Jump to Grip Walls

        int TJ2GWV = GripWallsB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["TripleJump"];
        if (TJ2GWV < 1 || GripWallsB.GetComponent<SkillButtons>().unlocked)
        {
            TJ2GW.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            TJ2GW.text = TJ2GWV.ToString();
        }
        //Triple Jump to Jump height

        int TJ2JHV = JumpHeightB.GetComponent<SkillButtons>().levelToUnlock - PersistantGameManager.Instance.skillLevels["TripleJump"];
        if (TJ2JHV < 1 || JumpHeightB.GetComponent<SkillButtons>().unlocked)
        {
            TJ2JH.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            TJ2JH.text = TJ2JHV.ToString();
        }
        //Instant Kill

        int SKV = InstantKillB.GetComponent<SkillButtons>().nessesaryXPLevel - PersistantGameManager.Instance.playerStats.playerLevel;
        if (SKV < 1)
        {
            IK.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            IK.text = SKV.ToString();
        }
        //Weapon Drop Value

        int LSV = WeaponDropValueB.GetComponent<SkillButtons>().nessesaryXPLevel - PersistantGameManager.Instance.playerStats.playerLevel;
        if (LSV < 1)
        {
            WDV.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            WDV.text = LSV.ToString();
        }

    }

    //Add all the buttons to the list of buttons
    void AddButtons(Button b1, Button b2, Button b3, Button b4, Button b5, Button b6, Button b7, Button b8, Button b9, Button b10, Button b11, Button b12, Button b13, Button b14, Button b15, Button b16, Button b17)
    {
        buttons.Add(b1);
        buttons.Add(b2);
        buttons.Add(b3);
        buttons.Add(b4);
        buttons.Add(b5);
        buttons.Add(b6);
        buttons.Add(b7);
        buttons.Add(b8);
        buttons.Add(b9);
        buttons.Add(b10);
        buttons.Add(b11);
        buttons.Add(b12);
        buttons.Add(b13);
        buttons.Add(b14); 
        buttons.Add(b15);
        buttons.Add(b16);
        buttons.Add(b17);

        foreach(Button b in buttons)
        {
            b.colors = colorBlock;
            b.GetComponent<SkillButtons>().text = b.gameObject.transform.Find("Text").GetComponent<Text>().text;
        }


    }
    //Returns the corresponding button
    Button Translate(string _name)
    {
        switch (_name)
        {
            case "AttackSpeed":
                return AttackSpeedB;
            case "Smite":
                return SmiteB;
            case "SmiteDuration":
                return SmiteDurationB;
            case "SmiteDamage":
                return SmiteDamageB;
            case "LifeSteal":
                return LifeStealB;
            case "AirAttack":
                return AirAttackB;
            case "DefenceWithMovement":
                return MoveDefenceB;
            case "MoveSpeed":
                return MoveSpeedB;
            case "TripleJump":
                return TripleJumpB;
            case "JumpHeight":
                return JumpHeightB;
            case "GripWalls":
                return GripWallsB;
            case "HealthBonus":
                return HealthBonusB;
            case "Turtle":
                return TurtleB;
            case "TurtleDuration":
                return TurtleDurationB;
            case "TurtleDefense":
                return TurtleDefenseB;
            case "WeaponDropValue":
                return WeaponDropValueB;
            case "InstantKill":
                return InstantKillB;
            default:

                return null;
        }
    }

}
