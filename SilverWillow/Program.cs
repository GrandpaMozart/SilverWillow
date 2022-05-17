// See https://aka.ms/new-console-template for more information
bool gameOver = false;
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(@" __                           
(_ o|   _ ._ \    /o|| _      
__)||\/(/_|   \/\/ |||(_)\/\/ ");
Console.WriteLine("\n ~|~| A Magical Girl Story |~|~");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine(@"Battling monsters with magical weapons... protecting the earth with the power of friendship...
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

};
Console.WriteLine();
Console.Write($"Thanks, {userName}! Type HELP to see a list of commands anytime.");
Console.WriteLine();
int roomID = 1;
string roomDescript = "You are in your bedroom. The WINDOW is open and a gentle breeze wafts through. Your BED is unmade. Atop your NIGHTSTAND is a sad-looking HOUSEPLANT.";
List<string> roomObjects = new List<string>()
    {
    "WINDOW",
    "BED",
    "NIGHTSTAND",
    "HOUSEPLANT",
    "DOOR",
    };
while (gameOver == false)
{
    Console.WriteLine();
    Console.Write("What should I do? ");
    string[]userInput = Console.ReadLine().ToUpper().Split(' ');
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
            else if (roomObjects.Contains(argument))
            {
                Console.WriteLine("When this works right, you'll see a description of the object you looked at here.");
            }
            else
            {
                Console.WriteLine("I don't see that here.");
            }
             break;
        case "ATTACK":
            break;
        case "TALK":
            break;
        default:
            Console.WriteLine("I don't recognize that command.");
            break;
    }
}

