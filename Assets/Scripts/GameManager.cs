using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<PlayerClass> PlayersList = new List<PlayerClass>(4);
    int playerPointer = 0;

    public Text CodeInfo;
    public Text ShowInfoText;
    public GameObject Slider;

    bool enableClick = true;

    enum Rotation
    {
        Left, Right
    }

    private void Start()
    {
        PlayersList.Add(new PlayerClass() { HealthPoints = -100 });
        PlayersList.Add(new PolymorphismPlayer() { HealthPoints = -100 });
        PlayersList.Add(new EncapsulationPlayer() { HealthPoints = -100 });
        PlayersList.Add(new InheritancePlayer() { HealthPoints = -100 });
    }

    void Update()
    {
        if (enableClick)
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] raycastHits = Physics.RaycastAll(ray);

                if (raycastHits.Length > 0)
                {
                    GameObject rotationDirection = raycastHits[0].collider.gameObject;

                    switch (rotationDirection.name)
                    {
                        case "Left":
                            {
                                RotatePlatform(Rotation.Left);
                                break;
                            }
                        case "Right":
                            {
                                RotatePlatform(Rotation.Right);
                                break;
                            }
                    }
                }
            }
    }

    string GetPillarInfo(PlayerClass player)
    {
        return player.ShowInfo();
    }

    void RotatePlatform(Rotation direction)
    {
        Vector3 rotationAxis = Vector3.zero;

        switch (direction)
        {
            case Rotation.Left:
                {
                    if (--playerPointer < 0)
                        playerPointer = 3;

                    rotationAxis = Vector3.up;
                    break;
                }
            case Rotation.Right:
                {
                    if (++playerPointer > 3)
                        playerPointer = 0;

                    rotationAxis = Vector3.down;
                    break;
                }
        }

        StartCoroutine(Rotate(rotationAxis));
    }

    IEnumerator Rotate(Vector3 quaternion)
    {
        enableClick = false;

        int timer = 90;

        while (timer-- != 0)
        {
            Slider.transform.rotation *= Quaternion.Euler(quaternion);
            yield return new WaitForSeconds(0.01f);
        }

        ChangeMainText();

        enableClick = true;
    }

    void ChangeMainText()
    {
        switch (PlayersList[playerPointer].PlayerType)
        {
            case "Abstraction":
                {
                    CodeInfo.text =
@"public class PlayerClass
{
    protected int healthPoints;
    public string PlayerType { get; set; }
    public virtual int HealthPoints
    { get => healthPoints; set => healthPoints = value; }

    public PlayerClass() { PlayerType = ""Abstraction""; }

    public virtual string ShowInfo()
    {
        return ""PlayerType:"" + PlayerType;
    }
}
";
                    break;
                }
            case "Inheritance":
                {
                    CodeInfo.text =
@"public class InheritancePlayer : PlayerClass
{
    public InheritancePlayer() { PlayerType = ""Inheritance""; }

    public override string ShowInfo()
    {
        return base.ShowInfo() + ""; Parent: "" + ShowParentClass();
    }

    public string ShowParentClass()
    {
        return this.GetType().BaseType.ToString();
    }
}
";
                    break;
                }
            case "Polymorphism":
                {
                    CodeInfo.text =
@"public class PolymorphismPlayer : PlayerClass
{
    public override int HealthPoints { get => healthPoints + 100; set => healthPoints = value; }

    public PolymorphismPlayer() { PlayerType = ""Polymorphism""; }

    public override string ShowInfo()
    {
        return base.ShowInfo() + ""; healthPoints: "" + healthPoints;
    }
}
";
                    break;
                }
            case "Encapsulation":
                {
                    CodeInfo.text =
@"public class EncapsulationPlayer : PlayerClass
{
    public new int HealthPoints
    {
        get => healthPoints;
        set { healthPoints = value < 0 ? 0 : value; }
    }

    public EncapsulationPlayer() { PlayerType = ""Encapsulation""; }

    public override string ShowInfo()
    {
        string info = ShowType() + ""; "" + ShowHP();
        return info;
    }

    private string ShowType() { return ""PlayerType: "" + PlayerType; }
    private string ShowHP() { return ""HealthPoints: "" + healthPoints.ToString(); }
}
";
                    break;
                }
        }
    }

    public void ShowPillarInfo()
    {
        string pillarInfo = PlayersList[playerPointer].ShowInfo();
        ShowInfoText.text = pillarInfo;
    }
}
