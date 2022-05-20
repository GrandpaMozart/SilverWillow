
using CsvHelper;
using System.Globalization;

bool gameOver = false;

//Define the list of valid commands during normal gameplay.
List<string> commandList = new List<string>()
{
    "HELP",
    "BAG",
    "LOOK",
    "ATTACK",
    "TALK",
    "TAKE",
    "DROP",
    "Directions (NORTH, SOUTH, EAST, WEST)",
    "HEALTH",
    "EQUIP",
    "USE",
    "STATS",
};

// Pull the room data from the CSV file
IEnumerable<Room> rooms;
using (var reader = new StreamReader("Room.csv"))
using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    rooms = csv.GetRecords<Room>().ToList();
}

// Pull the item data from the CSV file
List<Item> items;
using (var reader = new StreamReader("Item.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    items = csv.GetRecords<Item>().ToList();
}

List<RoomDescription> roomDescriptions;
using (var reader = new StreamReader("RoomDescription.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    roomDescriptions = csv.GetRecords<RoomDescription>().ToList();
}
    List<Containment> containments;
using (var reader = new StreamReader("Containment.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    containments = csv.GetRecords<Containment>().ToList();
}

// Pull the NPC and player data from the CSV file
List<People> peeps;
using (var reader = new StreamReader("People.csv"))
using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    peeps = csv.GetRecords<People>().ToList();
}

// Combine NPC/item data into GameElement list
List<IGameElement> elements = new List<IGameElement>();
foreach (var item in items)
{
    elements.Add(item);
}
foreach (var peep in peeps)
{
    elements.Add(peep);
}

//Pull player data from CSV file into vars
var checkPlayerPeep = peeps.Where(peep => peep.Name == "ME");
int playerMaxHP = checkPlayerPeep.First().MaxHP;
int playerHP = checkPlayerPeep.First().HP;
int playerAttack = checkPlayerPeep.First().Attack;
int playerDefense = checkPlayerPeep.First().Defense;
int playerWeaponID = checkPlayerPeep.First().WeaponEquipped;
string playerDefaultAttack = checkPlayerPeep.First().DefaultAttack;
int playerExp = 0;

//ASCII art and introduction
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
Console.WriteLine();
Console.Write($"Thanks, {userName}! Type HELP to see a list of commands anytime.");
Console.WriteLine();

//Establish the player in Room 1 and display its description for the first time
int RoomID = 1;
var playerRoom = peeps.Where(peep => peep.ID == 1);
string roomDescript = rooms.ElementAt(RoomID - 1).Description;
Console.Write(roomDescript);
var checkRoomItems = (roomDescriptions.Where(roomDescription => roomDescription.RoomID == RoomID));
foreach (var checkRoomItem in checkRoomItems)
{
    var isItemHere = (items.Where(item => item.ID == checkRoomItem.ObjectID && item.Room == RoomID));
    if (isItemHere.Count() > 0)
        {
        Console.Write($" {checkRoomItem.Description} ");
    }
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Characters in room:");
var peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
foreach (var peep in peepsInRoom)
{
    Console.WriteLine(peep.Name);
}

//Prompt the user to input a command, and then parse and process it.
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
    var matchingRooms = (rooms.Where(room => room.ID == RoomID));
    switch (command)
    {
        //If the player types "HELP," display the list of possible commands
        case "HELP":
            for (int i = 0; i < commandList.Count; i++)
            {
                Console.WriteLine(commandList[i]);
            }
            break;
        //If the player types "LOOK," display the room description if there is no argument, or the description of a specified game element
        case "LOOK":
            if (argument == "null")
            {
                Console.Write(roomDescript);
                checkRoomItems = (roomDescriptions.Where(roomDescription => roomDescription.RoomID == RoomID));
                foreach (var checkRoomItem in checkRoomItems)
                {
                    var isItemHere = (items.Where(item => item.ID == checkRoomItem.ObjectID && item.Room == RoomID));
                    if (isItemHere.Count() > 0)
                    {
                        Console.Write($" {checkRoomItem.Description} ");
                    }
                }
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("\n Characters in room:");
                peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
                foreach (var peep in peepsInRoom)
                {
                    Console.WriteLine(peep.Name);
                }
            }

            else
            {
                var matchingElements = (elements.Where(element => element.Name == argument && element.Room == RoomID && element.Lookable == true));
                switch (matchingElements.Count())
                {
                    case 0:
                        matchingElements = (elements.Where(element => element.Name == argument && element.Room == 0 && element.IsCarried == true && element.Lookable == true));
                        if (matchingElements.Count() > 0)
                        {
                            Console.WriteLine(matchingElements.First().Description);
                           
                        }
                        else
                        {
                            Console.WriteLine($"I don't see any {argument} here.");
                        }
                        break;
                    case 1:
                        Console.Write(matchingElements.First().Description);
                        var checkContained = (containments.Where(containment => containment.ContainerID == matchingElements.First().ID));
                        if (checkContained.Count() > 0)
                        {
                            foreach (var containment in checkContained)
                            {
                                Console.Write($"{containment.Description} ");
                            }
                        }
                        Console.WriteLine();
                        break;
                }

            }
            break;
        case "EQUIP":
            if (argument == "null")
            {
                var checkItems = (items.Where(item => item.IsEquipped == true));
                if (checkItems.Count() > 0)
                {
                    Console.WriteLine("Equipped items:");
                    foreach (var item in checkItems)
                        Console.WriteLine(item.Name);
                }
                else
                {
                    Console.WriteLine("No equipped items.");
                }
                checkItems = (items.Where(item => item.IsCarried == true && item.Type == "BATTLE"));
                if (checkItems.Count() > 0)
                {
                    Console.WriteLine("Equippable items:");
                    foreach (var item in checkItems)
                        Console.WriteLine(item.Name);
                }
                else
                {
                    Console.WriteLine("No equippable items.");
                }
            }
            else
            {
                var checkItems = (items.Where(item => item.IsCarried == true && item.Type == "BATTLE" && item.Name == argument));
                switch (checkItems.Count())
                {
                    case 0:
                        Console.WriteLine($"{argument} is not equippable.");
                        break;
                    case 1:
                        checkItems.First().IsEquipped = true;
                        Console.WriteLine($"Equipped {argument}.");
                        playerWeaponID = checkItems.First().ID;
                        playerAttack = playerAttack + checkItems.First().AttackBuff;
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
                var matchingItems = (elements.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        var checkAttackable = (matchingItems.Where(item => item.Attackable == true));
                        if (checkAttackable.Count() == 0)
                        {
                            Console.WriteLine($"The {argument} is not your enemy!");
                        }
                        else
                        {
                            string playerWeapon = playerDefaultAttack;
                            switch (playerWeaponID)
                            {
                                case 0:
                                    break;
                                default:
                                    var checkWeapon = items.Where(item => item.IsEquipped == true);
                                    playerWeapon = checkWeapon.First().Name;
                                    break;

                            }
                            bool inBattle = true;
                            string attackTarget = argument;
                            var enemy = (peeps.Where(item => item.Name == argument));
                            Console.WriteLine($"You stare down the {argument}.");
                            while (inBattle)
                            {
                                Console.Write("\n ATTACK, RUN, or SPECIAL? ");
                                command = Console.ReadLine().ToUpper();
                                switch (command)
                                {
                                    case "ATTACK":
                                        Console.WriteLine($"You attack {argument} with your {playerDefaultAttack}!");
                                        var rand = new Random();
                                        int damageToEnemy = rand.Next((playerAttack - 7), (playerAttack + 7));
                                        if (damageToEnemy < 0 || damageToEnemy < enemy.First().Defense)
                                        {
                                            damageToEnemy = 0;
                                            Console.WriteLine("Your attack misses!");
                                        }
                                        enemy.First().HP = enemy.First().HP - damageToEnemy;
                                        Console.WriteLine($"{argument} takes {damageToEnemy} damage!");
                                        if (enemy.First().HP < 1)
                                        {
                                            enemy.First().Room = 0;
                                            Console.WriteLine($"You defeated {argument}!");
                                            playerExp = playerExp + enemy.First().ExpOnDefeat;
                                            Console.WriteLine($"You gained {enemy.First().ExpOnDefeat} EXP.");
                                            inBattle = false;
                                        }

                                        if (inBattle)
                                        {
                                            rand = new Random();
                                            int damageToPlayer = rand.Next((enemy.First().Attack - 7), (enemy.First().Attack + 7));
                                            if (damageToPlayer < 0 || damageToPlayer < playerDefense)
                                            {
                                                damageToPlayer = 0;
                                                Console.WriteLine($"{argument}'s attack misses!");
                                            }
                                            else
                                            {
                                                playerHP = playerHP - damageToPlayer;
                                                Console.WriteLine($"{argument} attacks you with {enemy.First().DefaultAttack}.");
                                                Console.WriteLine($"You take {damageToPlayer} damage!");
                                                if (playerHP < 1)
                                                {
                                                    Console.WriteLine("You have been defeated in battle!");
                                                    gameOver = true;
                                                    inBattle = false;
                                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                                    Console.WriteLine(@"
   _________    __  _________   ____ _    ____________ 
  / ____/   |  /  |/  / ____/  / __ \ |  / / ____/ __ \
 / / __/ /| | / /|_/ / __/    / / / / | / / __/ / /_/ /
/ /_/ / ___ |/ /  / / /___   / /_/ /| |/ / /___/ _, _/ 
\____/_/  |_/_/  /_/_____/   \____/ |___/_____/_/ |_|  
                                                       ");
                                                }
                                            }
                                        }
                                        break;
                                    case "RUN":
                                        Console.WriteLine("You run away.");
                                        inBattle = false;
                                        break;
                                    case "SPECIAL":
                                        var checkSpecial = items.Where(item => item.IsEquipped == true);
                                        if (checkSpecial.Count() > 0)
                                        {
                                            rand = new Random();
                                            int chanceSpecial = rand.Next(1, 15);
                                            if (chanceSpecial < 4)
                                            {
                                                Console.WriteLine("Your special attack misses.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"You grasp the {playerWeapon} tightly in your hand, and it emits a magical glow. You shout the words \"{checkSpecial.First().BattleMessage},\" and feel its power flow through you!");
                                                rand = new Random();
                                                int specialAttack = playerAttack + rand.Next((checkSpecial.First().AttackBuff - 2), (checkSpecial.First().AttackBuff + 7));
                                                enemy.First().HP = enemy.First().HP - specialAttack;
                                                Console.WriteLine($"{argument} takes {specialAttack} damage!");
                                                if (enemy.First().HP < 1)
                                                {
                                                    enemy.First().Room = 0;
                                                    Console.WriteLine($"You defeated {argument}!");
                                                    playerExp = playerExp + enemy.First().ExpOnDefeat;
                                                    Console.WriteLine($"You gained {enemy.First().ExpOnDefeat} EXP.");
                                                    inBattle = false;
                                                }
                                            }

                                            if (inBattle)
                                            {
                                                rand = new Random();
                                                int damageToPlayer = rand.Next((enemy.First().Attack - 7), (enemy.First().Attack + 7));
                                                if (damageToPlayer < 0 || damageToPlayer < playerDefense)
                                                {
                                                    damageToPlayer = 0;
                                                    Console.WriteLine($"{argument}'s attack misses!");
                                                }
                                                else
                                                {
                                                    playerHP = playerHP - damageToPlayer;
                                                    Console.WriteLine($"{argument} attacks you with {enemy.First().DefaultAttack}.");
                                                    Console.WriteLine($"You take {damageToPlayer} damage!");
                                                    if (playerHP < 1)
                                                    {
                                                        Console.WriteLine("You have been defeated in battle!");
                                                        gameOver = true;
                                                        inBattle = false;
                                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                                        Console.WriteLine(@"
   _________    __  _________   ____ _    ____________ 
  / ____/   |  /  |/  / ____/  / __ \ |  / / ____/ __ \
 / / __/ /| | / /|_/ / __/    / / / / | / / __/ / /_/ /
/ /_/ / ___ |/ /  / / /___   / /_/ /| |/ / /___/ _, _/ 
\____/_/  |_/_/  /_/_____/   \____/ |___/_____/_/ |_|  
                                                       ");
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("You have no weapon equipped, so you have no special attack.");
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("I don't understand.");
                                        break;




                                }




                            }
                        }
                        break;
                }

            }
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
                        Console.WriteLine($"You can't carry the {argument}.");
                        break;
                    case 1:
                        var checkTakeable = (matchingItems.Where(item => item.Takeable == true));
                        if (checkTakeable.Count() == 0)
                        {
                            Console.WriteLine($"You can't carry the {argument}.");
                        }
                        else
                        {
                            Console.WriteLine($"You put {argument} in your bag.");
                            matchingItems.First().IsCarried = true;
                            var checkContained = (containments.Where(containment => containment.ContainedID == checkTakeable.First().ID));
                            if (checkContained.Count() > 0)
                            {
                                checkContained.First().ContainerID = 0;
                            }
                            matchingItems.First().Room = 0;
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
                var matchingItems = (elements.Where(item => item.Name == argument && item.Room == RoomID && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"I don't see any {argument} here.");
                        break;
                    case 1:
                        var checkTalk = (elements.Where(item => item.Talkable == true));
                        switch (checkTalk.Count())
                        {
                            case 0:
                                Console.WriteLine($"You try talking to the {argument}, but get no response.");
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

        case "BAG":
            var checkBag = (elements.Where(item => item.Room == 0 && item.IsCarried == true));
            if (checkBag.Count() > 0)
            {
                foreach (var item in checkBag)
                {
                    Console.WriteLine(item.Name);
                }
            }
            else
            {
                Console.WriteLine("Nothing in inventory");
            }
            break;

        case "DROP":
            if (argument == "null")
            {
                Console.WriteLine("Drop what?");
            }

            else
            {
                var matchingItems = (items.Where(item => item.Name == argument && item.Room == 0 && item.Lookable == true));
                switch (matchingItems.Count())
                {
                    case 0:
                        Console.WriteLine($"You have no {argument} in your bag.");
                        break;
                    case 1:

                        Console.WriteLine($"You take {argument} out of the bag and put it down.");
                        matchingItems.First().IsCarried = false;
                        matchingItems.First().Room = RoomID;
                        matchingItems = (items.Where(item => item.Name == argument && item.IsEquipped == true));
                        if (matchingItems.Count() > 0)
                        {
                            matchingItems.First().IsEquipped = false;
                        }
                        break;
                }
            }
            break;

        case "NORTH":
            RoomID = matchingRooms.First().North;
            playerRoom.First().Room = RoomID;
            roomDescript = rooms.ElementAt(RoomID - 1).Description;
            Console.WriteLine(roomDescript);
            Console.WriteLine("\n Characters in room:");
            peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
            foreach (var peep in peepsInRoom)
            {
                Console.WriteLine(peep.Name);
            }
            break;

        case "SOUTH":
            RoomID = matchingRooms.First().South;
            playerRoom.First().Room = RoomID;
            roomDescript = rooms.ElementAt(RoomID - 1).Description;
            Console.WriteLine(roomDescript);
            Console.WriteLine("\n Characters in room:");
            peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
            foreach (var peep in peepsInRoom)
            {
                Console.WriteLine(peep.Name);
            }
            break;

        case "EAST":
            RoomID = matchingRooms.First().East;
            playerRoom.First().Room = RoomID;
            roomDescript = rooms.ElementAt(RoomID - 1).Description;
            Console.WriteLine(roomDescript);
            Console.WriteLine("\n Characters in room:");
            peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
            foreach (var peep in peepsInRoom)
            {
                Console.WriteLine(peep.Name);
            }
            break;
        case "WEST":
            RoomID = matchingRooms.First().West;
            playerRoom.First().Room = RoomID;
            roomDescript = rooms.ElementAt(RoomID - 1).Description;
            Console.WriteLine(roomDescript);
            Console.WriteLine("\n Characters in room:");
            peepsInRoom = (peeps.Where(peeps => peeps.Room == RoomID));
            foreach (var peep in peepsInRoom)
            {
                Console.WriteLine(peep.Name);
            }
            break;
        case "HEALTH":
            Console.WriteLine($"You have {playerHP} out of {playerMaxHP} health.");
            break;
        default:
            Console.WriteLine("I don't recognize that command.");
            break;
        case "USE":
            if (argument == "null")
            {
                Console.WriteLine("Use what?");
            }
            else
            {
                var checkUse = items.Where(item => item.Name == argument && item.IsCarried == true);
                if (checkUse.Count() < 1)
                {
                    Console.WriteLine($"You have no {argument} to use.");
                }
                else
                {
                    switch (checkUse.First().Type)
                    {
                        case "HEALING":
                            int RestoredHP = playerHP + checkUse.First().HPBuff;
                            if (RestoredHP >= playerMaxHP)
                            {
                                playerHP = playerMaxHP;
                                Console.WriteLine($"You use the {argument} and return to full health.");
                            }
                            else
                            {
                                playerHP = RestoredHP;
                                Console.WriteLine($"You use the {argument} and recover {checkUse.First().HPBuff} health.");
                                checkUse.First().IsCarried = false;
                            }
                            break;
                        default:
                                Console.WriteLine("I don't know how to use that.");
                                break;
                    }
                }

            }
            break;
        case "STATS":
            Console.WriteLine($"Attack: {playerAttack}");
            Console.WriteLine($"Defense: {playerDefense}");
            Console.WriteLine($"EXP: {playerExp}");
            Console.WriteLine($"Health: {playerHP}/{playerMaxHP}");
            break;


    }
}

