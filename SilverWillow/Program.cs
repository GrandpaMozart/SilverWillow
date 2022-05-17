
using CsvHelper;
using System.Globalization;

bool gameOver = false;
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(@" __                           
(_ o|   _ ._ \    /o|| _      
__)||\/(/_|   \/\/ |||(_)\/\/ ");
Console.WriteLine("\n ~|~| A Magical Girl's Return |~|~");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine(@"Protecting the world with glitter and friendship... fending off interstellar threats with love-enchanted weapons... 
That was your life sixty years ago.
After bringing relative peace to the world, you and your friends returned to the quiet of normal life.
Now, a new danger threatens the world. 
It's up to you to defend this planet once more.");
Console.Write("\n Enter your name: ");
string userName = Console.ReadLine().ToUpper();
List<string> commandList = new List<string>()
{
    "HELP",
    "LOOK",
    "ATTACK",
    "TALK",
    "TAKE",

};
Console.WriteLine();
Console.Write($"Thanks, {userName}! Type HELP to see a list of commands anytime.");
Console.WriteLine();
int RoomID = 1;
string roomDescript = "null";
IEnumerable<Room> rooms;
using (var reader = new StreamReader("Room.csv"))
using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    rooms = csv.GetRecords<Room>();
    roomDescript = rooms.ElementAt(RoomID - 1).Description;
}
Console.WriteLine(roomDescript);
while (gameOver == false)
{
    Console.WriteLine();
    Console.Write("What should I do? ");
    string[] userInput = Console.ReadLine().ToUpper().Split(' ');
    string command = userInput[0];
    string argument = null;
    if (userInput.Count() > 1)
    {
        argument = userInput[1];
    }
    else
    {
        argument = "null";
    }
    Console.WriteLine();
    List<Item> items;
    using (var reader = new StreamReader("Item.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        items = csv.GetRecords<Item>().ToList();
    }
    switch (command)
    {
        case "HELP":
            for (int i = 0; i < commandList.Count; i++)
            {
                Console.WriteLine(commandList[i]);
            }
            break;
        case "LOOK":
            if (argument == "null")
            {
                Console.WriteLine(roomDescript);
            }
                        
            else { 
                var matchingItems = (items.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        Console.WriteLine(matchingItems.First().Description);
                        break;
                }

            }
            break;
    
        case "ATTACK":
            
            if (argument == "null")
            {
                Console.WriteLine("Attack what?");
            }

            else
            {
                var matchingItems = (items.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true)) ;
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        var checkAttackable = (items.Where(item => item.Attackable == true));
                        if (checkAttackable.Count() == 0)
                        {
                            Console.WriteLine($"The {argument} is not your enemy!");
                        }
                        else
                        {
                            Console.WriteLine($"You attack the {argument}.");
                        }    
                        break;
                }

            }
            break;
        case "TALK":
            if (argument == "null")
            {
                Console.WriteLine("You mutter to yourself, but it doesn't seem like anyone is listening.");
            }

            else
            {
                var matchingItems = (items.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        var checkTalk = (items.Where(item => item.Talkable == true)) ;
                        switch (checkTalk.Count())
                        {
                            case 0:
                                Console.WriteLine($"You try talking to the {argument}, but it doesn't respond.");
                                break;
                            case 1:
                                Console.WriteLine($"You talk to the {argument}.");
                                break;
                        }
                        break;
                }

            }
            break;
            break;
        case "TAKE":
            if (argument == "null")
            {
                Console.WriteLine("Take what?");
            }
            else
            {
                var matchingItems = (items.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        var checkTakeable = (items.Where(item => item.Takeable == true));
                        if (checkTakeable.Count() == 0)
                        {
                            Console.WriteLine($"You can't carry the {argument}.");
                        }
                        else
                        {
                            Console.WriteLine($"You put the {argument} in your bag.");
                        }
                        break;
                }

            }
            break;

        default:
            Console.WriteLine("I don't recognize that command.");
            break;
    }
}

