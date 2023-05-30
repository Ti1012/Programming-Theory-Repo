using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass
{
    protected int healthPoints;
    public string PlayerType { get; set; }
    public virtual int HealthPoints
    { get => healthPoints; set => healthPoints = value; }

    public PlayerClass() { PlayerType = "Abstraction"; }

    public virtual string ShowInfo()
    {
        return "PlayerType:" + PlayerType;
    }
}

public class InheritancePlayer : PlayerClass
{
    public InheritancePlayer() { PlayerType = "Inheritance"; }

    public override string ShowInfo()
    {
        return base.ShowInfo() + "; Parent: " + ShowParentClass();
    }

    public string ShowParentClass()
    {
        return this.GetType().BaseType.ToString();
    }
}

public class PolymorphismPlayer : PlayerClass
{
    public override int HealthPoints { get => healthPoints + 100; set => healthPoints = value; }

    public PolymorphismPlayer() { PlayerType = "Polymorphism"; }

    public override string ShowInfo()
    {
        return base.ShowInfo() + "; healthPoints: " + healthPoints;
    }
}

public class EncapsulationPlayer : PlayerClass
{
    public new int HealthPoints
    {
        get => healthPoints;
        set { healthPoints = value < 0 ? 0 : value; }
    }

    public EncapsulationPlayer() { PlayerType = "Encapsulation"; }

    public override string ShowInfo()
    {
        string info = ShowType() + "; " + ShowHP();
        return info;
    }

    private string ShowType() { return "PlayerType: " + PlayerType; }
    private string ShowHP() { return "HealthPoints: " + healthPoints.ToString(); }
}
