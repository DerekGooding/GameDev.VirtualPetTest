using System.Text.Json;
using System.Timers;

public class Program
{
    public static void Main()
    {
        //coords for inner border of screen (c2-11,r3-6)
        //end of screen (c0,r10)

        string[] menuOptions = { "Stats", "Food", "Exit"};

        string[] screenLines = [
        "---------------",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "---------------" ];


        Random rand = new();

        var currentPet = new Pet("Default");
        Food riceBowl;
        Food friedEggs;
        Food cake;
        var ownedFood = new List<Food> { };

        Console.CursorVisible = false;

        //new game setup
        if (!File.Exists("SaveData"))
        {
            Console.WriteLine("Starting new game...");
            riceBowl = new("RiceBowl", 1, 0, true);
            friedEggs = new("Fried Eggs", 1, 0, true);
            cake = new("Cake", 1, 1, true);
            ownedFood = new List<Food> { riceBowl, friedEggs, cake};
            Thread.Sleep(2000);
            HatchEgg();
            currentPet = NamePet();
            SaveGameData(currentPet, ownedFood);
        }
        else
        {
            Console.WriteLine("Loading game...");
            currentPet = LoadGameData().CurrentPetData;
            ownedFood = LoadGameData().OwnedFoodList;
            Thread.Sleep(2000);
        }

        StartTimer();
        MainScreen();

        void HatchEgg()
        {
            DrawScreen();
            Console.WriteLine("An Egg?...");
            Thread.Sleep(2000);

            screenLines[4] = "|      (z)    |";
            DrawScreen();
            Console.WriteLine("A crack in the egg?...");
            Thread.Sleep(2000);

            screenLines[4] = "|      \\?/    |";
            DrawScreen();
            Console.WriteLine("It's hatching! What will you name it?");
        }
        
        Pet NamePet()
        {
            Console.Write("Name: ");
            string? userInput = Console.ReadLine();

            Pet currentPet = new(userInput, DateTime.Now.ToString());
            screenLines[4] = $"|   {currentPet.Appearance}     |";

            return currentPet;
        }
        
        void MainScreen()
        {
            bool running = true;
            int selection = 1;
            DrawScreen();

            do
            {
                ClearLowerScreen();
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

                var userInput = Console.ReadKey(true).KeyChar;

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
            ClearLowerScreen();
            Console.WriteLine("Stats:");
            Console.WriteLine(
$@"Name: {currentPet.Name}
Age: {currentPet.Age}
Hunger: {currentPet.Hunger}
Happiness: {currentPet.Happiness}
Money: {currentPet.Money}
Birthday: {currentPet.Birthday}");

            Console.ReadKey(true);
        }

        void FoodScreen()
        {
            bool inMenu = true;
            int selection = 1;

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Food:");
            
                switch (selection)
                {
                    case 1:
                        Console.WriteLine($"\"{ownedFood[0].Name}\"\n{ownedFood[1].Name}\n{ownedFood[2].Name}");
                        break;
                    case 2:
                        Console.WriteLine($"{ownedFood[0].Name}\n\"{ownedFood[1].Name}\"\n{ownedFood[2].Name}");
                        break;
                    case 3:
                        Console.WriteLine($"{ownedFood[0].Name}\n{ownedFood[1].Name}\n\"{ownedFood[2].Name}\"");
                        break;
                    default:
                        break;
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedFood.Count)
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
                            currentPet.Hunger += ownedFood[0].RestoredHunger;
                            break;
                        case 2:
                            currentPet.Hunger += ownedFood[1].RestoredHunger;
                            break;
                        case 3:
                            currentPet.Hunger += ownedFood[2].RestoredHunger;
                            break;
                        default:
                            break;
                    }

                    ClearLowerScreen();

                    if(ownedFood[selection - 1].RestoredHappiness > 0)
                    {
                        currentPet.Happiness += 1;
                        Console.WriteLine($"Fed {currentPet.Name} {ownedFood[selection - 1].Name}, gained +{ownedFood[selection - 1].RestoredHunger} <3 +{ownedFood[selection - 1].RestoredHappiness}:)\n(Press any key to continue...)");
                    }
                    else
                    {
                        Console.WriteLine($"Fed {currentPet.Name} {ownedFood[1]}, gained +{ownedFood[selection - 1].RestoredHunger} <3\n(Press any key to continue...)");
                    }                    
                    Console.ReadKey(true);
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

        void ClearLowerScreen()
        {
            Console.SetCursorPosition(0, 10);
            for(int i = 0; i < 8; i++)
            {
                Console.WriteLine("".PadRight(45));
            }
            Console.SetCursorPosition(0, 10);
        }

        void SaveGameData(Pet currentPetData, List<Food> ownedFoodList)
        {
            var saveData = new SaveData();
            saveData.CurrentPetData = currentPetData;
            saveData.OwnedFoodList = ownedFoodList;
            var jsonString = JsonSerializer.Serialize(saveData);
            File.WriteAllText("SaveData", jsonString);
        }

        SaveData LoadGameData()
        {
            var jsonString = File.ReadAllText("SaveData");
            SaveData loadedData = JsonSerializer.Deserialize<SaveData>(jsonString);
            return loadedData;
        }

        void StartTimer()
        {
            System.Timers.Timer timer;
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Console.Write("aa");

            //food
            int roll = rand.Next(1, 11);
            if(currentPet.Hunger == 0)
            {
                if (roll == 10)
                {
                    //Console.WriteLine("Died");
                }
            }
            else
            {
                if(roll >= 6)
                {
                    currentPet.Hunger--;
                    //Console.WriteLine("Lost 1 Hunger");
                    Poop();
                }
            }

            //happiness
            if(roll >= 6)
            {
                currentPet.Happiness--;
                //Console.WriteLine("Lost 1 Happiness");

            }
        }

        void Poop()
        {
            int x = rand.Next(2,12);
            int y = rand.Next(3,7);
            Console.SetCursorPosition(x, y);
            Console.Write("s");
            Console.SetCursorPosition(x, y+1);
            Console.Write("*");
        }

        void TestRun()
        {



            //Pet currentPet = new("Timmy");



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

                if (Console.ReadKey(true).KeyChar == 's')
                {
                    Console.Clear();
                    Console.WriteLine($"Virtual Pet Test");
                    foreach (string screenLine in screenLines)
                    {
                        Console.WriteLine(screenLine);
                    }
                    Console.WriteLine("(A) (S) (D)");

                    Console.WriteLine("Stats\n\"Food\"");

                    if(Console.ReadKey(true).KeyChar == 'a')
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
                        Console.ReadKey(true);
                        screenLines[3] = "|             |";

                    }
                }
                else if (Console.ReadKey(true).KeyChar == 'a')
                {

                }
                else if (Console.ReadKey(true).KeyChar == 'd')
                {

                }




                //Console.WriteLine(
                    //$"Stats:\nName: {currentPet.Name}\nAppearance: {currentPet.Appearance}\nAge: {currentPet.Age}");


                    if (Console.ReadKey(true).KeyChar == 's')
                    {
                        Console.Clear();
                        Console.WriteLine($"Virtual Pet Test");
                        foreach (string screenLine in screenLines)
                        {
                            Console.WriteLine(screenLine);
                        }
                        Console.WriteLine("(A) (S) (D)");
                        Console.WriteLine("Food:\n\"Rice\"\nCheese\nCake");

                        if (Console.ReadKey(true).KeyChar == 's')
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

class Pet(string name, string birthday = "1/1/1 11:11", string appearance = "(0c0)", int age = 1, int hunger = 3, int happiness = 3, int money = 150, int stage = 1)
{
    public string Name { get; set; } = name;
    public string Appearance { get; set; } = appearance;
    public int Age { get; set; } = age;
    public int Hunger { get; set; } = hunger;
    public int Happiness { get; set; } = happiness;
    public int Money { get; set; } = money;
    public string Birthday { get; set; } = birthday;
    public int Stage { get; set; } = stage;
}

class Food(string name, int restoredHunger = 1, int restoredHappiness = 0, bool owned = false)
{
    public string Name { get; set; } = name;
    public int RestoredHunger { get; set; } = restoredHunger;
    public int RestoredHappiness { get; set; } = restoredHappiness;
    public bool Owned { get; set; } = owned;
}

class SaveData()
{
    public Pet CurrentPetData { get; set; }
    public List<Food> OwnedFoodList { get; set; }
}
