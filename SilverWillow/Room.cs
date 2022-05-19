using System;

public class Room
{
    public string Name { get; set; }
    public int ID { get; set; }
    public string Description { get; set; }
    public int North { get; set; }
    public int South { get; set; }
    public int East { get; set; }
    public int West { get; set; }
    public Room()
    {
    }
}
