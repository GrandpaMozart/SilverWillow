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
while (gameOver == false)
{
    Console.WriteLine();
    Console.Write("What should I do? ");
    string command = Console.ReadLine().ToUpper();
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

