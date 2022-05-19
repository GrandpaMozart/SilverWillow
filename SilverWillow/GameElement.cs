using System;

public interface IGameElement
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ID { get; set; }
    public int Room { get; set; }
    public bool Lookable { get; set; }
    public bool Attackable { get; set; }
    public bool Talkable { get; set; }
    public bool Takeable { get; set; }
    public bool IsCarried { get; set; }


}
