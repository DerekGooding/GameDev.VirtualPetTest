
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main()
    {
        //TestRun();
        string[] menuOptions = { "Stats", "Food", "Exit"};
        string[] ownedFood = { "Rice", "Eggs", "Cake"};

        string[] screenLines = [
            "---------------",
            "|             |",
            "|             |",
            "|             |",
            "|      ()     |",
            "|             |",
            "|             |",
            "---------------" ];


        Pet currentPet = new("Tommy");

        MainScreen();
        
        void MainScreen()
        {
            bool running = true;
            int selection = 1;

            do
            {
                DrawScreen();
                Console.WriteLine("Menu:");

                switch(selection)
                {
                    case 1:
                        Console.WriteLine($"\"{menuOptions[0]}\"\n{menuOptions[1]}\n{menuOptions[2]}");
                        break;
                    case 2:
                        Console.WriteLine($"{menuOptions[0]}\n\"{menuOptions[1]}\"\n{menuOptions[2]}");
                        break;
                    case 3:
                        Console.WriteLine($"{menuOptions[0]}\n{menuOptions[1]}\n\"{menuOptions[2]}\"");
                        break;
                    default:
                        break;
                }

                var userInput = Console.ReadKey().KeyChar;

                if (userInput == 's')
                {
                    if (selection == menuOptions.Length)
                    {
                        selection = 1;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (userInput == 'a')
                {
                    switch (selection)
                    {
                        case 1:
                            StatScreen();
                            break;
                        case 2:
                            FoodScreen();
                            break;
                        case 3:
                            running = false;
                            break;
                        default:
                            break;
                    }
                }
            } while (running);

        }

        void StatScreen()
        {
            DrawScreen();
            Console.WriteLine("Stats:");
            Console.WriteLine(
$@"Name: {currentPet.Name}
Age: {currentPet.Age}
Hunger: {currentPet.Hunger}
Happiness: {currentPet.Happiness}
Money: {currentPet.Money}");

            Console.ReadKey();
        }

        void FoodScreen()
        {
            bool inMenu = true;
            int selection = 1;

            do
            {
                DrawScreen();
                Console.WriteLine("Food:");
            
                switch (selection)
                {
                    case 1:
                        Console.WriteLine($"\"{ownedFood[0]}\"\n{ownedFood[1]}\n{ownedFood[2]}");
                        break;
                    case 2:
                        Console.WriteLine($"{ownedFood[0]}\n\"{ownedFood[1]}\"\n{ownedFood[2]}");
                        break;
                    case 3:
                        Console.WriteLine($"{ownedFood[0]}\n{ownedFood[1]}\n\"{ownedFood[2]}\"");
                        break;
                    default:
                        break;
                }

                var userInput = Console.ReadKey().KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedFood.Length)
                    {
                        selection = 1;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (userInput == 'a')
                {
                    switch (selection)
                    {
                        case 1:
                            currentPet.Hunger += 1;
                            break;
                        case 2:
                            currentPet.Hunger += 1;
                            break;
                        case 3:
                            currentPet.Hunger += 1;
                            break;
                        default:
                            break;
                    }
                    DrawScreen();
                    Console.WriteLine($"Fed {currentPet.Name} {ownedFood[selection-1]}, gained +1 <3\n(Press any key to continue...)");
                    Console.ReadKey();
                }
                else if(userInput == 'd')
                {
                    inMenu = false;
                }
            } while (inMenu);

        }

        void DrawScreen()
        {
            Console.Clear();
            Console.WriteLine("VIRTUAL PET");
            foreach (string screenLine in screenLines)
            {
                Console.WriteLine(screenLine);
            }
            Console.WriteLine("  (A) (S) (D)");

        }

        void TestRun()
        {



            Pet currentPet = new("Timmy");



/*            string[] screenLines = [
                "---------------",
            "|             |",
            "|             |",
            "|             |",
            "|      ()     |",
            "|             |",
            "|             |",
            "---------------" ];*/

            while(true)
            {            
                Console.WriteLine($"Virtual Pet Test");

                foreach (string screenLine in screenLines)
                {
                    Console.WriteLine(screenLine);
                }

                Console.WriteLine("(A) (S) (D)");

                Console.WriteLine("\"Stats\"\nFood");

                if (Console.ReadKey().KeyChar == 's')
                {
                    Console.Clear();
                    Console.WriteLine($"Virtual Pet Test");
                    foreach (string screenLine in screenLines)
                    {
                        Console.WriteLine(screenLine);
                    }
                    Console.WriteLine("(A) (S) (D)");

                    Console.WriteLine("Stats\n\"Food\"");

                    if(Console.ReadKey().KeyChar == 'a')
                    {
                        Console.Clear();
                        Console.WriteLine($"Virtual Pet Test");
                        screenLines[3] = "|      <3     |";
                        foreach (string screenLine in screenLines)
                        {
                            Console.WriteLine(screenLine);
                        }
                        Console.WriteLine("(A) (S) (D)");
                        Console.WriteLine($"Fed {currentPet.Name} and gained +1 <3!");
                        Console.ReadKey();
                        screenLines[3] = "|             |";

                    }
                }
                else if (Console.ReadKey().KeyChar == 'a')
                {

                }
                else if (Console.ReadKey().KeyChar == 'd')
                {

                }




                //Console.WriteLine(
                    //$"Stats:\nName: {currentPet.Name}\nAppearance: {currentPet.Appearance}\nAge: {currentPet.Age}");


                    if (Console.ReadKey().KeyChar == 's')
                    {
                        Console.Clear();
                        Console.WriteLine($"Virtual Pet Test");
                        foreach (string screenLine in screenLines)
                        {
                            Console.WriteLine(screenLine);
                        }
                        Console.WriteLine("(A) (S) (D)");
                        Console.WriteLine("Food:\n\"Rice\"\nCheese\nCake");

                        if (Console.ReadKey().KeyChar == 's')
                        {
                            Console.Clear();
                            Console.WriteLine($"Virtual Pet Test");
                            foreach (string screenLine in screenLines)
                            {
                                Console.WriteLine(screenLine);
                            }
                            Console.WriteLine("(A) (S) (D)");
                            Console.WriteLine("Food:\nRice\n\"Cheese\"\nCake");
                        }
                    }
            }
        }        
    }
}

class Pet(string name, string appearance = "(0c0)", int age = 1, int hunger = 3, int happiness = 3, int money = 150, DateTime birthday = DateTime.Now)
{
    public string Name { get; set; } = name;
    public string Appearance { get; set; } = appearance;
    public int Age { get; set; } = age;
    public int Hunger { get; set; } = hunger;
    public int Happiness { get; set; } = happiness;
    public int Money { get; set; } = money;
    public DateTime Birthday { get; set; } = birthday;
}

