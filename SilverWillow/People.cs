using System;

public class People : IGameElement
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ID { get; set; }
    public int Room { get; set; }
    public bool Lookable { get; set; }
    public bool Attackable { get; set; }
    public bool Talkable { get; set; }
    public bool Takeable { get; set; }
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int WeaponEquipped { get; set; }
    public int ExpOnDefeat { get; set; }
    public string DefaultAttack { get; set; }
    public bool IsCarried { get; set; }

    public People()
    {
    }
}
