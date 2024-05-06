using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Timers;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    public static void Main()
    {
        //bug w/ drawing screen

        Random rand = new();
        Screen screen = new();
        Games games = new();

        Food riceBowl = new("RiceBowl", 1, 0, true);
        Food friedEggs = new("Fried Eggs", 1, 0, true);
        Food cake = new("Cake", 1, 1, true);
        Food steak;

        var ownedFood = new List<string>{ "riceBowl","friedEggs", "cake"};
        var ownedFoodList = new List<Food>{ riceBowl, friedEggs, cake};


        List<int[]> poopPositions = new();
        List<string> petModels = new List<string> { "@", ">@", "@>@", "*>*", "O>O", "$>$"}; //baby, child, adults (default care, bad care, good care, good care and high money?
        var currentPet = new Pet("Default");
        
        string[] menuOptions = { "Stats", "Food", "Shop" ,"Games", "Care", "Light", "Exit"};
        string[] shopItems = { "Steak - $50", "Dice - $150"};
        var ownedGames = new List<string>{ "Left or Right?" };


        System.Timers.Timer tickTimer;
        int hungerTickCount = 3; //9s
        int happinessTickCount = 5; //15s
        int evolveTickCount = 10; //30s
        int illnessTickCount = 5; //15s
        int deathCheckTickCount = 5; //15s

        int deathMistakes = 0;

        bool sleeping = false;
        bool sick = false;
        string[] careOptions = { "Clean", "Medicine", "Discipline/Praise"};

        int[] topScreenInnerBordersX = { 2, 11};
        int[] topScreenInnerBordersY = { 3, 6 };
        int[] bottomScreenStartPositionXY = { 0, 10};
        List<Food> allFood;

        Console.CursorVisible = false;
        bool programRunning = true;
        bool isDead = false;

        //new game setup
        if (!File.Exists("SaveData"))
        {
            ownedFood = [ "riceBowl", "friedEggs", "cake"];
            steak = new("Steak", 1, 1, false);

            Console.WriteLine("Starting new game...");
            Thread.Sleep(2000);
            HatchEgg();
            currentPet = NamePet();
        }
        else
        {
            currentPet = LoadGameData().CurrentPetData;
            ownedFood = LoadGameData().OwnedFood;
            ownedGames = LoadGameData().OwnedGames;
            currentPet.Age = 1 + (int)float.Parse(DateTime.Now.Subtract(DateTime.Parse(currentPet.Birthday)).TotalDays.ToString());
            if (ownedFood.Contains("steak"))
            {
                steak = new("Steak", 1, 1, true);
                ownedFoodList.Add(steak);
                shopItems[0] = "SOLD OUT - Steak";
            }
            else
            {
                steak = new("Steak", 1, 1, false);
            }

            if (ownedGames.Contains("Higher or Lower?"))
            {
                shopItems[1] = "SOLD OUT - Dice";
            }

            Console.WriteLine("Loading game...");
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
            int selection = 0;

            screen.UpdatePetPosition(currentPet.Appearance);
            screen.DrawScreen();

            do
            {
                if (isDead)
                {
                    programRunning = false;
                    break;
                }

                ClearLowerScreen();
                Console.WriteLine("Menu:");

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (selection == i)
                    {
                        Console.WriteLine($"\"{menuOptions[i]}\"");
                    }
                    else
                    {
                        Console.WriteLine(menuOptions[i]);

                    }
                }



                var userInput = Console.ReadKey(true).KeyChar;
                if (isDead)
                {
                    programRunning = false;
                    break;
                }

                if (userInput == 's')
                {
                    if (selection == menuOptions.Length - 1)
                    {
                        selection = 0;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (userInput == 'a')
                {
                    if (sleeping && selection != 0 && selection != 5 && selection != 6)
                    {
                        ClearLowerScreen();
                        Console.WriteLine($"{currentPet.Name} is asleep!\n(Press any key to continue...)");
                        Console.ReadKey(true);

                        continue;
                    }
                    switch (selection)
                    {
                        case 0:
                            StatScreen();
                            break;
                        case 1:
                            FoodScreen();
                            break;
                        case 2:
                            ShopScreen();
                            break;
                        case 3:                            
                            GameScreen();
                            break;
                        case 4:
                            CareScreen();
                            break;
                        case 5:
                            SwitchLightOnOff();
                            break;
                        case 6:
                            if (sleeping)
                            {
                                currentPet.CareLevel++;
                            }
                            else
                            {
                                currentPet.CareLevel--;
                            }

                            SaveGameData(currentPet, ownedFood, ownedGames);
                            programRunning = false;
                            break;
                        default:
                            break;
                    }
                }
                else if (userInput == 'd')
                {
                    Console.Clear();
                    selection = 0;
                    screen.DrawScreen();
                    ClearLowerScreen();
                }

                if (isDead)
                {
                    programRunning = false;
                    break;
                }

            } while (programRunning);

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
            Console.WriteLine("(Press any key to continue...)");
            Console.ReadKey(true);
        }

        void FoodScreen()
        {
            bool inMenu = true;
            int selection = 0;

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Food:");

                for(int i = 0; i < ownedFoodList.Count; i++)
                {
                    if(selection == i)
                    {
                        Console.WriteLine($"\"{ownedFoodList[i].Name}\"");
                    }
                    else
                    {
                        Console.WriteLine(ownedFoodList[i].Name);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedFoodList.Count - 1)
                    {
                        selection = 0;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (userInput == 'a')
                {
                    if (currentPet.Hunger < 5)
                    {
                        currentPet.Hunger += ownedFoodList[selection].RestoredHunger;
                        currentPet.CareLevel++;
                        ClearLowerScreen();

                        if (ownedFoodList[selection].RestoredHappiness > 0)
                        {
                            currentPet.Happiness += 1;
                            Console.WriteLine($"Fed {currentPet.Name} {ownedFoodList[selection].Name}, gained +{ownedFoodList[selection].RestoredHunger} <3 +{ownedFoodList[selection].RestoredHappiness}:)\n(Press any key to continue...)");
                        }
                        else
                        {
                            Console.WriteLine($"Fed {currentPet.Name} {ownedFoodList[selection].Name}, gained +{ownedFoodList[selection].RestoredHunger} <3\n(Press any key to continue...)");
                        }
                    }
                    else
                    {
                        ClearLowerScreen();
                        Console.WriteLine($"{currentPet.Name} is already full!\n(Press any key to continue...)");
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
            int selection = 0;

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Shop:");

                for (int i = 0; i < shopItems.Length; i++)
                {
                    if (selection == i)
                    {
                        Console.WriteLine($"\"{shopItems[i]}\"");
                    }
                    else
                    {
                        Console.WriteLine(shopItems[i]);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == shopItems.Length - 1)
                    {
                        selection = 0;
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
                        case 0:
                            if (!steak.Owned)
                            {
                                if (currentPet.Money >= 50)
                                {
                                    currentPet.Money -= 50;
                                    Console.WriteLine($"Bought Steak for $50!\n(Press any key to continue...)");
                                    shopItems[0] = "SOLD OUT - Steak";
                                    steak.Owned = true;
                                    ownedFood.Add("steak");
                                    ownedFoodList.Add(steak);
                                }
                                else
                                {
                                    Console.WriteLine("Not enough money!\n(Press any key to continue...)");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Item Steak is sold out!\n(Press any key to continue...)");
                            }
                            Console.ReadKey(true);
                            break;
                        case 1:
                            if(!ownedGames.Contains("Higher or Lower?"))
                            {
                                if(currentPet.Money >= 150)
                                {
                                    currentPet.Money -= 150;
                                    Console.WriteLine($"Bought Dice for $150!\n(Press any key to continue...)");
                                    shopItems[1] = "SOLD OUT - Dice";
                                    ownedGames.Add("Higher or Lower?");
                                }
                                else
                                {
                                    Console.WriteLine("Not enough money!\n(Press any key to continue...)");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Item Dice is sold out!\n(Press any key to continue...)");
                            }
                            Console.ReadKey(true);
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

        void GameScreen()
        {
            bool running = true;
            int selection = 0;
            int[] moneyAndHappiness = [0,0];

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Games:");

                for (int i = 0; i < ownedGames.Count; i++)
                {
                    if (selection == i)
                    {
                        Console.WriteLine($"\"{ownedGames[i]}\"");
                    }
                    else
                    {
                        Console.WriteLine(ownedGames[i]);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedGames.Count - 1)
                    {
                        selection = 0;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (userInput == 'a')
                {
                    tickTimer.Stop();
                    ClearLowerScreen();
                    switch (selection)
                    {
                        case 0:
                            moneyAndHappiness = games.LeftOrRightGame();
                            break;
                        case 1:
                            moneyAndHappiness = games.HigherOrLowerGame();
                            break;
                        default:
                            break;
                    }
                    currentPet.Money += moneyAndHappiness[0];
                    currentPet.Happiness += moneyAndHappiness[1];
                    if (moneyAndHappiness[1] > 0)
                    {
                        currentPet.CareLevel++;
                    }
                    tickTimer.Start();
                }
                else if(userInput == 'd')
                {
                    running = false;
                }

            } while (running);

        }

        void CareScreen()
        {
            bool running = true;
            int selection = 0;

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Care:");

                for (int i = 0; i < careOptions.Length; i++)
                {
                    if (selection == i)
                    {
                        Console.WriteLine($"\"{careOptions[i]}\"");
                    }
                    else
                    {
                        Console.WriteLine(careOptions[i]);
                    }


                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == careOptions.Length - 1)
                    {
                        selection = 0;
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
                        case 0:
                            //clean
                            CleanPoop();
                            break;
                        case 1:
                            //medicine
                            GiveMedicine();
                            break;
                        case 2:
                            //discipline
                            Discipline();
                            break;
                        default:
                            break;
                    }
                }
                else if (userInput == 'd')
                {
                    running = false;
                }
            } while (running);
        }

        void ClearLowerScreen()
        {
            Console.SetCursorPosition(0, 10);
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine("".PadRight(45));
            }
            Console.SetCursorPosition(0, 10);
        }

        void SaveGameData(Pet currentPetData, List<string> ownedFood, List<string> ownedGames)
        {
            var saveData = new SaveData();
            saveData.CurrentPetData = currentPetData;
            saveData.OwnedFood = ownedFood;
            saveData.OwnedGames = ownedGames;
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
            tickTimer = new System.Timers.Timer(3000); //3s
            tickTimer.Elapsed += TickEvent;
            tickTimer.AutoReset = true;
            tickTimer.Enabled = true;
        }
        
        void TickEvent(object sender, EventArgs e)
        {
            DeathCheck();
            HungerCheck();
            HappinessCheck();
            EvolveCheck();
            IllnessCheck();


            screen.ResetScreen();
            screen.UpdatePoops(poopPositions);
            screen.UpdatePetPosition(currentPet.Appearance);
            screen.DrawScreen();

        }

        void EvolveCheck()
        {
            if(currentPet.Stage <= 2)
            {
                if (evolveTickCount <= 0) //30s
                {
                    currentPet.Appearance = petModels[currentPet.Stage];
                    currentPet.Stage++;
                    evolveTickCount = 10;
                    screen.ResetScreen();
                }
                else
                {
                    evolveTickCount--;
                }
            }
        }

        void HappinessCheck()
        {
            if (happinessTickCount <= 0) //15s
            {
                if(currentPet.Happiness >= 5)
                {
                    currentPet.CareLevel++;
                }

                if (currentPet.Happiness > 0)
                {
                    currentPet.Happiness--;
                }

                happinessTickCount = 5;
            }
            else
            {
                happinessTickCount--;
            }
        }

        void DeathCheck()
        {
            if(deathCheckTickCount <= 0) //15s
            {
                if(deathMistakes > 0)
                {
                    deathMistakes--;
                }
                
                if(deathMistakes > 5)
                {
                    //death
                    tickTimer.Stop();
                    isDead = true;
                    Console.Clear();
                    Console.WriteLine($"{currentPet.Name} died!");
                    Thread.Sleep(1500);
                    Console.WriteLine(
$@"Name: {currentPet.Name}
Age: {currentPet.Age}
Hunger: {currentPet.Hunger}
Happiness: {currentPet.Happiness}
Money: {currentPet.Money}
Birthday: {currentPet.Birthday}");
                    Thread.Sleep(1500);
                    Console.WriteLine("(Press any key to exit...)");
                    Console.ReadKey(true);
                    Thread.Sleep(1500);
                }

                deathCheckTickCount = 5;
            }
            else
            {
                deathCheckTickCount--;
            }
        }

        void HungerCheck()
        {
            if (hungerTickCount <= 0) //9s
            {
                if(currentPet.Hunger >= 5)
                {
                    currentPet.CareLevel++;
                }

                if (currentPet.Hunger > 0)
                {
                    currentPet.Hunger--;
                    AddPoop();
                }
                else
                {
                    deathMistakes++;
                }
                hungerTickCount = 3;
            }
            else
            {
                hungerTickCount--;
            }
        }

        void IllnessCheck()
        {
            if(illnessTickCount <= 0) //15s
            {
                if(rand.Next(0,10) >= 6)
                {
                    //set sick
                    sick = true;
                }
                illnessTickCount = 5;
            }
            else
            {
                illnessTickCount--;
            }
        } //todo

        void GiveMedicine()
        {
            if (sick)
            {
                currentPet.CareLevel++;
            }
            sick = false;
        } //

        void Discipline()//
        {
            //todo
            //if(correct discipline timing){currentPet.CareLevel++;}
        }

        void SwitchLightOnOff()
        {
            if (!sleeping)
            {
                tickTimer.Stop();
                screen.ResetScreen();
                screen.screenLines[2] = "|        ZZ   |";
                screen.screenLines[3] = "|      zz     |";
                screen.screenLines[4] = screen.screenLines[4].Insert(5, currentPet.Appearance);
                screen.screenLines[4] = screen.screenLines[4].Remove(5 + currentPet.Appearance.Length, currentPet.Appearance.Length);
                screen.DrawScreen();

                sleeping = true;
            }
            else
            {
                tickTimer.Start();
                screen.ResetScreen();

                sleeping = false;
            }
        }

        void AddPoop()
        {
            int x = rand.Next(1, 13);
            int y = rand.Next(1, 6);
            poopPositions.Add([x, y]);
        }

        void CleanPoop()
        {
            if(poopPositions.Count > 0)
            {
                currentPet.CareLevel++;
            }
            poopPositions.Clear();
        }
    }
}

public class Games
{
    Screen screen = new();
    Random rand = new();

    public int[] LeftOrRightGame()
    {
        int score = 0;
        int side = 0;

        for(int i =0;i<5;i++)
        {
            screen.ResetScreen();
            screen.screenLines[2] = $"|      {score}      |";
            screen.screenLines[4] = "|     @>@     |";
            screen.DrawScreen();
            Thread.Sleep(1000);
            screen.screenLines[4] = "|   <I@>@     |";
            screen.DrawScreen();
            Thread.Sleep(1000);
            screen.screenLines[4] = "|     @>@I>   |";
            screen.DrawScreen();
            Thread.Sleep(1000);
            screen.screenLines[4] = "|    ?@>@?    |";
            screen.DrawScreen();

            side = rand.Next(2);
            char answer = Console.ReadKey(true).KeyChar;
            if(answer == 'a')
            {
                if(side == 0)
                {
                    screen.screenLines[3] = "|   +1        |";
                    screen.screenLines[4] = "|   <I@>@     |";
                    score++;
                }
                else
                {
                    screen.screenLines[3] = "|         !   |";
                    screen.screenLines[4] = "|     @>@I>   |";
                }
            }
            else if(answer == 's')
            {
                if(side == 1)
                {
                    screen.screenLines[3] = "|        +1   |";
                    screen.screenLines[4] = "|     @>@I>   |";
                    score++;
                }
                else
                {
                    screen.screenLines[3] = "|    !        |";
                    screen.screenLines[4] = "|   <I@>@     |";
                }
            }
            else if(answer == 'd')
            {
                break;
            }
            else
            {
                screen.screenLines[3] = "|    !   !    |";
                screen.screenLines[4] = "|   <I@>@I>   |";
            }
            screen.DrawScreen();
            Thread.Sleep(2000);
        }

        screen.screenLines[2] = $"|      {score}      |";
        screen.screenLines[3] = "|     @>@     |";
        screen.screenLines[4] = "|             |";
        screen.DrawScreen();
        Thread.Sleep(1000);
        screen.screenLines[3] = "|             |";
        screen.screenLines[4] = "|     @>@     |";
        screen.DrawScreen();
        int happinessGained = 0;
        if(score > 2)
        {
            happinessGained++;
            Console.WriteLine($" Score: {score} Money: +{score * 10} Happiness: +1");
        }
        else
        {
            Console.WriteLine($" Score: {score} Money: +{score * 10} Happiness: +0");
        }
        Thread.Sleep(1000);
        Console.WriteLine("(Press any key to continue...)");
        screen.screenLines[3] = "|     @>@     |";
        screen.screenLines[4] = "|             |";
        screen.DrawScreen();
        Thread.Sleep(1000);
        screen.screenLines[3] = "|             |";
        screen.screenLines[4] = "|     @>@     |";
        screen.DrawScreen();
        Console.ReadKey(true);
        int[] moneyAndHappiness = [score*10, happinessGained];
        return moneyAndHappiness;
    }

    public int[] HigherOrLowerGame()
    {
        int currentNumber = 5;
        int nextNumber = 1;
        int score = 0;
        bool higher = false;

        for (int i = 0; i < 5; i++)
        {
            screen.ResetScreen();
            screen.screenLines[2] = $"|      {score}      |";
            screen.screenLines[4] = $"|   {currentNumber} @>@ ?   |";
            screen.DrawScreen();
            Thread.Sleep(1000);
            nextNumber = rand.Next(1, 10);
            while(nextNumber == currentNumber)
            {
                nextNumber = rand.Next(1, 10);
            }

            if(nextNumber > currentNumber)
            {
                higher = true;
            }
            else
            {
                higher = false;
            }

            char userInput = Console.ReadKey(true).KeyChar;

            if (userInput == 'a' && higher) //higher
            {
                score++;
            }
            else if(userInput == 's' && !higher) //lower
            {
                score++;
            }
            else if(userInput == 'd')
            {
                break;
            }

            screen.screenLines[4] = $"|   {currentNumber} @>@ {nextNumber}   |";
            screen.DrawScreen();
            currentNumber = nextNumber;
            Thread.Sleep(2000);
        }

        screen.screenLines[2] = $"|      {score}      |";
        screen.screenLines[3] = "|     @>@     |";
        screen.screenLines[4] = "|             |";
        screen.DrawScreen();
        Thread.Sleep(1000);
        screen.screenLines[3] = "|             |";
        screen.screenLines[4] = "|     @>@     |";
        screen.DrawScreen();

        int happinessGained = 0;
        if (score > 2)
        {
            happinessGained++;
            Console.WriteLine($" Score: {score} Money: +{score * 10} Happiness: +1");
        }
        else
        {
            Console.WriteLine($" Score: {score} Money: +{score * 10} Happiness: +0");
        }

        Thread.Sleep(1000);
        Console.WriteLine("(Press any key to continue...)");
        screen.screenLines[3] = "|     @>@     |";
        screen.screenLines[4] = "|             |";
        screen.DrawScreen();
        Thread.Sleep(1000);
        screen.screenLines[3] = "|             |";
        screen.screenLines[4] = "|     @>@     |";
        screen.DrawScreen();
        Console.ReadKey(true);

        int[] moneyAndHappiness = [score * 10, happinessGained];
        return moneyAndHappiness;
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

class Pet(string name, string birthday = "1/1/1 11:11", string appearance = "@", int age = 1, int hunger = 3, int happiness = 3, int money = 150, int stage = 0, int careLevel = 0)
{
    public string Name { get; set; } = name;
    public string Appearance { get; set; } = appearance;
    public int Age { get; set; } = age;
    public int Hunger { get; set; } = hunger;
    public int Happiness { get; set; } = happiness;
    public int Money { get; set; } = money;
    public string Birthday { get; set; } = birthday;
    public int Stage { get; set; } = stage;
    public int CareLevel { get; set; } = careLevel;

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
    public List<string> OwnedFood { get; set; }
    public List<string> OwnedGames { get; set; }
}
