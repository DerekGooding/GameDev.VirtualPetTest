using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Timers;

public class Program
{
    public static void Main()
    {
        //coords for inner border of screen (c2-11,r3-6)
        //end of screen (c0,r10)

        Random rand = new();
        Screen screen = new();
        Food riceBowl;
        Food friedEggs;
        Food cake;
        List<int[]> poopPositions = new();

        var currentPet = new Pet("Default");
        var ownedFood = new List<Food> { };

        string[] menuOptions = { "Stats", "Food","Shop", "Exit"};
        string[] shopItems = { "Steak", "Jump Rope"};


        List<string> petModels = new List<string> { "@", ">@", "@>@" };

        int HungerTickCount = 0;
        int HappinessTickCount = 0;
        int EvolveTickCount = 0;

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

        StartTimers();
        MainScreen();
        
        void HatchEgg()
        {
            screen.screenLines[4] = "|      ( )    |";
            screen.DrawScreen();
            Console.WriteLine("An Egg?...");
            Thread.Sleep(2000);

            screen.screenLines[4] = "|      (z)    |";
            screen.DrawScreen();
            Console.WriteLine("A crack in the egg?...");
            Thread.Sleep(2000);

            screen.screenLines[4] = "|      \\?/    |";
            screen.DrawScreen();
            Console.WriteLine("It's hatching! What will you name it?");
        }
        
        Pet NamePet()
        {
            Console.Write("Name: ");
            string? userInput = Console.ReadLine();

            Pet currentPet = new(userInput, DateTime.Now.ToString());
            screen.screenLines[4] = $"|             |";

            return currentPet;
        }
        
        void MainScreen()
        {
            bool running = true;
            int selection = 1;

            screen.UpdatePetPosition(currentPet.Appearance);
            screen.DrawScreen();

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Menu:");

                switch(selection)
                {
                    case 1:
                        Console.WriteLine($"\"{menuOptions[0]}\"\n{menuOptions[1]}\n{menuOptions[2]}\n{menuOptions[3]}");
                        break;
                    case 2:
                        Console.WriteLine($"{menuOptions[0]}\n\"{menuOptions[1]}\"\n{menuOptions[2]}\n{menuOptions[3]}");
                        break;
                    case 3:
                        Console.WriteLine($"{menuOptions[0]}\n{menuOptions[1]}\n\"{menuOptions[2]}\"\n{menuOptions[3]}");
                        break;
                    case 4:
                        Console.WriteLine($"{menuOptions[0]}\n{menuOptions[1]}\n{menuOptions[2]}\n\"{menuOptions[3]}\"");
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
                            ShopScreen();
                            break;
                        case 4:
                            SaveGameData(currentPet, ownedFood);
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
                if (ownedFood.Count == 3)
                {
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
                }
                else if (ownedFood.Count == 4)
                {
                    switch (selection)
                    {
                        case 1:
                            Console.WriteLine($"\"{ownedFood[0].Name}\"\n{ownedFood[1].Name}\n{ownedFood[2].Name}\n{ownedFood[3].Name}");
                            break;
                        case 2:
                            Console.WriteLine($"{ownedFood[0].Name}\n\"{ownedFood[1].Name}\"\n{ownedFood[2].Name}\n{ownedFood[3].Name}"); 
                            break;
                        case 3:
                            Console.WriteLine($"{ownedFood[0].Name}\n{ownedFood[1].Name}\n\"{ownedFood[2].Name}\"\n{ownedFood[3].Name}"); 
                            break;
                        case 4:
                            Console.WriteLine($"{ownedFood[0].Name}\n{ownedFood[1].Name}\n{ownedFood[2].Name}\n\"{ownedFood[3].Name}\"");
                            break;
                        default:
                            break;
                    }

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

        void ShopScreen()
        {
            bool inMenu = true;
            int selection = 1;

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Shop:");

                switch (selection)
                {
                    case 1:
                        Console.WriteLine($"\"{shopItems[0]}\" - $50\n{shopItems[1]} - $300");
                        break;
                    case 2:
                        Console.WriteLine($"{shopItems[0]} - $50\n\"{shopItems[1]}\" - $300");
                        break;
                    default:
                        break;
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection >= shopItems.Length)
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
                    ClearLowerScreen();

                    switch (selection)
                    {
                        case 1:
                            if(currentPet.Money >= 50)
                            {
                                currentPet.Money -= 50;
                                Console.WriteLine($"Bought {shopItems[0]} for $50!\n(Press any key to continue...)");
                                shopItems[0] = "SOLD OUT - Steak";
                                ownedFood.Add(new Food("Steak",1,1,true));
                            }
                            else
                            {
                                Console.WriteLine("Not enough money!\n(Press any key to continue...)");
                            }
                            Console.ReadKey(true);
                            break;
                        case 2:
                            break;
                        default:
                            break;
                    }
                }
                else if (userInput == 'd')
                {
                    inMenu = false;
                }
            } while (inMenu);
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

        void StartTimers()
        {
            System.Timers.Timer tickTimer;
            tickTimer = new System.Timers.Timer(3000);
            tickTimer.Elapsed += TickEvent;
            tickTimer.AutoReset = true;
            tickTimer.Enabled = true;
        }

        void TickEvent(object sender, EventArgs e)
        {
            HungerCheck();
            HappinessCheck();
            EvolveCheck();

            screen.ResetScreen();
            screen.UpdatePoops(poopPositions);
            screen.UpdatePetPosition(currentPet.Appearance);
            screen.DrawScreen();

        }

        void EvolveCheck()
        {
            if(currentPet.Stage <= 2)
            {
                if (EvolveTickCount >= 10) //30s
                {
                    currentPet.Appearance = petModels[currentPet.Stage];
                    currentPet.Stage++;
                    EvolveTickCount = 0;
                    screen.ResetScreen();
                }
                else
                {
                    EvolveTickCount++;
                }
            }
        }

        void HappinessCheck()
        {
            if (HappinessTickCount >= 5) //15s
            {
                currentPet.Happiness--;
                HappinessTickCount = 0;
            }
            else
            {
                HappinessTickCount++;
            }
        }

        void HungerCheck()
        {
            if (HungerTickCount >= 3) //9s
            {
                currentPet.Hunger--;
                HungerTickCount = 0;
                AddPoop();

            }
            else
            {
                HungerTickCount++;
            }
        }

        void AddPoop()
        {
            int x = rand.Next(2, 12);
            int y = rand.Next(3, 7);
            poopPositions.Add([x, y]);
        }
    }
}

public class Screen()
{
    Random rand = new();

    public string[] screenLines = [
        "---------------",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "---------------" ];
    int petX = 5;
    int petY = 5;

    public void DrawScreen()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(" VIRTUAL PET   ");
        foreach (string screenLine in screenLines)
        {
            Console.WriteLine(screenLine);
        }
        Console.WriteLine("  (A) (S) (D)");
    }

    public void UpdatePetPosition(string petAppearance)
    {
        int x = rand.Next(1, 12);
        int y = rand.Next(1, 7);
        screenLines[petY] = screenLines[petY].Replace(petAppearance, "".PadRight(petAppearance.Length));
        screenLines[y] = screenLines[y].Insert(x, petAppearance);
        screenLines[y] = screenLines[y].Remove(x + petAppearance.Length, petAppearance.Length);


        petX = x;
        petY = y;
    }

    public void ResetScreen()
    {
        screenLines = [
        "---------------",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "|             |",
        "---------------" ];
    }

    public void UpdatePoops(List<int[]> poopPositions)
    {
        foreach(int[] poop in poopPositions)
        {
            screenLines[poop[1]] = screenLines[poop[1]].Insert(poop[0], "S");
            screenLines[poop[1] + 1] = screenLines[poop[1] + 1].Insert(poop[0], "*");
            screenLines[poop[1]] = screenLines[poop[1]].Remove(poop[0] + 1, 1);
            screenLines[poop[1] + 1] = screenLines[poop[1] + 1].Remove(poop[0] + 1, 1);
        }
    }

}

class Pet(string name, string birthday = "1/1/1 11:11", string appearance = "@", int age = 1, int hunger = 3, int happiness = 3, int money = 150, int stage = 0)
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
