using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Timers;

public class Program
{
    public static void Main()
    {
        //coords for inner border of screen (c2-11,r3-6)
        //end of screen (c0,r10)

        string[] menuOptions = { "Stats", "Food", "Exit"};
        //


        List<string> petModels = new List<string> { "@", ">@", "@>@" };

        int tickCount = 1;

        Random rand = new();

        Screen screen = new();

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
            //HatchEgg();
            //currentPet = NamePet();
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
        /*
        void HatchEgg()
        {
            screenLines[4] = "|      ( )    |";
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
        }*/
        /*
        Pet NamePet()
        {
            Console.Write("Name: ");
            string? userInput = Console.ReadLine();

            Pet currentPet = new(userInput, DateTime.Now.ToString());
            screenLines[4] = $"|             |";

            return currentPet;
        }*/
        
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

            System.Timers.Timer timer;
            timer = new System.Timers.Timer(3000);
            //timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

            //StartEvolveTimer();
        }
        /*
        void StartEvolveTimer()
        {
            int evolveTime = 0;
            switch (currentPet.Stage)
            {
                case 1:
                    evolveTime = 20000;
                    break;
                case 2:
                    evolveTime = 40000;
                    break;
                case 3:
                    evolveTime = 60000;
                    break;
                default:
                    break;
            }
            System.Timers.Timer evolveTimer;
            evolveTimer = new System.Timers.Timer(evolveTime);
            evolveTimer.Elapsed += EvolvePet;
            evolveTimer.Enabled = true;

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
                    addPoop();
                }
            }

            //happiness
            if(roll >= 6)
            {
                currentPet.Happiness--;
                //Console.WriteLine("Lost 1 Happiness");

            }

            //DrawPet();
        }

        void EvolvePet(Object source, ElapsedEventArgs e)
        {
            switch (currentPet.Stage)
            {
                case 1:
                    currentPet.Appearance = petModels[1];
                    break;
                case 2:
                    currentPet.Appearance = petModels[2];
                    break;
                default:
                    break;

            }
            StartEvolveTimer();
        }*/

        void TickEvent(object sender, EventArgs e)
        {
            screen.UpdatePetPosition(currentPet.Appearance);
            screen.DrawScreen();
        }




        /*void drawPetTimed(Object source, ElapsedEventArgs e)
        {
            Poop();
            Console.SetCursorPosition(petX, petY);
            Console.Write("".PadRight(currentPet.Appearance.Length));

            int x = rand.Next(1, 10);
            int y = rand.Next(2, 8);
            Console.SetCursorPosition(x, y);
            Console.Write(currentPet.Appearance);
            petX = x;
            petY = y;
        }*/
    }
}

public class Screen()
{
    Random rand = new();

    string[] screenLines = [
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
    List<int[]> poopPositions = new();



    public void DrawScreen()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(" VIRTUAL PET");
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



    void AddPoop()
    {
        int x = rand.Next(2, 12);
        int y = rand.Next(3, 7);
        poopPositions.Add([x, y]);

        foreach(int[] poop in poopPositions)
        {
            screenLines[poop[1]].Insert(poop[0], "S");
            screenLines[poop[1] + 1].Insert(poop[0], "*");
            screenLines[poop[1]].Remove(poop[0], 1);
            screenLines[poop[1] + 1].Remove(poop[0], 1);
        }
    }

}

class Pet(string name, string birthday = "1/1/1 11:11", string appearance = "@", int age = 1, int hunger = 3, int happiness = 3, int money = 150, int stage = 1)
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
