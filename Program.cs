using System.Text.Json;
using VirtualPetTest.Model;

namespace VirtualPetTest;

public static class Program
{
    public static void Main()
    {
        Screen screen = new();
        Games games = new();

        string[] menuOptions =
        [
            "Stats",
            "Food",
            "Shop",
            "Games",
            "Care",
            "Light",
            "Exit"
        ];

        Food steak;

        List<string> ownedFood =
        [
            "riceBowl",
            "friedEggs",
            "cake"
        ];

        List<Food> ownedFoodList =
        [
            new("Rice Bowl", 1, 0, 1, true),
            new("Fried Eggs", 1, 0, 1, true),
            new("Cake", 1, 1, 2, true)
        ];

        string[] shopItems =
        [
            "Steak - $50",
            "Dice - $150"
        ];

        List<string> ownedGames = ["Left or Right?"];

        string[] careOptions =
        [
            "Clean",
            "Medicine",
            "Discipline"
        ];

        List<int[]> poopPositions = [];
        bool sick = false;
        bool wantsAttention = false;
        bool fakeAttention = false;
        bool hungryAttention = false;
        bool happyAttention = false;

        bool sleeping = false;

        bool programRunning = true;

        List<string> petModels = ["@", ">@", "@>@", "*>*", "O>O", "$>$"]; //baby, child, teen, adults (bad care, default care, good care)
        Pet currentPet = new("Default");
        int deathMistakes = 0;
        bool isDead = false;

        System.Timers.Timer tickTimer;

        //faster events
        /*
        int tickCount = 3000; //3s
        int hungerTickCount = 3; //9s
        int happinessTickCount = 5; //15s
        int evolveTickCount = 10; //30s
        int illnessTickCount = 5; //15s
        int deathCheckTickCount = 5; //15s
        int fakeAttentionTickCount = 5; //15s
        */

        //slower events
        /*        
        int tickCount = 30000; //30s
        int hungerTickCount = 10; //5m
        int happinessTickCount = 20; //10m
        int evolveTickCount = 120; //1h
        int illnessTickCount = 20; //10m
        int deathCheckTickCount = 60; //30m
        int fakeAttentionTickCount = 20; //10m
        */

        //slower events with faster tick count
        const int tickCount = 5000; //5s
        const int hungerTickCount = 60; //5m
        const int happinessTickCount = 120; //10m
        const int evolveTickCount = 720; //1h
        const int illnessTickCount = 120; //10m
        const int deathCheckTickCount = 360; //30m
        const int fakeAttentionTickCount = 120; //10m

        int activeHungerTickCount = hungerTickCount;
        int activeHappinessTickCount = happinessTickCount;
        int activeEvolveTickCount = evolveTickCount;
        int activeIllnessTickCount = illnessTickCount;
        int activeDeathCheckTickCount = deathCheckTickCount;
        int activeFakeAttentionTickCount = fakeAttentionTickCount;

        int[] topScreenInnerBordersX = [2, 11];
        int[] topScreenInnerBordersY = [3, 6];
        int[] bottomScreenStartPositionXY = [0, 10];

        Console.CursorVisible = false;
        Console.WindowWidth = 30;
        Console.WindowHeight = 30;

        //new game
        if (!File.Exists("SaveData"))
        {
            Console.WriteLine("Starting new game...");
            Thread.Sleep(2000);
            HatchEgg();
            currentPet = NamePet();
        }
        else //existing game
        {
            currentPet = LoadGameData().CurrentPetData;
            ownedFood = LoadGameData().OwnedFood;
            ownedGames = LoadGameData().OwnedGames;
            currentPet.Age = 1 + (int)float.Parse(DateTime.Now.Subtract(DateTime.Parse(currentPet.Birthday)).TotalDays.ToString());
            activeEvolveTickCount -= currentPet.EvolutionProgress;

            Console.WriteLine("Loading game...");
            Thread.Sleep(2000);
        }

        if (ownedFood.Contains("steak"))
        {
            ownedFoodList.Add(steak = new("Steak", 1, 1, 2, true));
            shopItems[0] = "SOLD OUT - Steak";
        }
        else
        {
            steak = new("Steak", 1, 1, 2, false);
        }

        if (ownedGames.Contains("Higher or Lower?"))
            shopItems[1] = "SOLD OUT - Dice";

        StartTimers();
        MainScreen();

        void HatchEgg()
        {
            string[] eggHatchSequence = ["|      ( )    |", "|      (z)    |", "|      \\?/    |"];
            string[] eggHatchDialogue = ["An Egg?...", "A crack in the egg?...", "It's hatching!\nWhat will you name it?"];
            for (int i = 0; i < 3; i++)
            {
                screen.screenLines[4] = eggHatchSequence[i];
                Console.Clear();
                screen.DrawScreen();
                Console.WriteLine(eggHatchDialogue[i]);
                Thread.Sleep(500);
            }
        }

        Pet NamePet()
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.Write("Name: ");
                string? userInput = Console.ReadLine();
                if (userInput != null && userInput.Length > 0 && userInput.Length < 11)
                {
                    Pet currentPet = new(userInput, DateTime.Now.ToString());
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input\nName must be 1-10 characters.\n(Press any key to continue...)");
                    Console.ReadKey(true);
                    ClearLowerScreen();
                }
            }
            return currentPet;
        }

        void MainScreen()
        {
            int selection = 0;
            screen.ResetScreen();
            screen.UpdatePetPosition(currentPet.Appearance, sick, wantsAttention);
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
                        Console.WriteLine($"\"{menuOptions[i]}\"");
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
                        selection = 0;
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
                                currentPet.CareLevel++;
                            else
                            {
                                currentPet.CareLevel--;
                            }

                            currentPet.EvolutionProgress = evolveTickCount - activeEvolveTickCount;

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
Discipline: {currentPet.Discipline}
Money: {currentPet.Money}
Weight: {currentPet.Weight}
Bday: {currentPet.Birthday}");
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

                for (int i = 0; i < ownedFoodList.Count; i++)
                {
                    if (selection == i)
                        Console.WriteLine($"\"{ownedFoodList[i].Name}\"");
                    else
                    {
                        Console.WriteLine(ownedFoodList[i].Name);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedFoodList.Count - 1)
                        selection = 0;
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
                        currentPet.Weight += ownedFoodList[selection].WeightGained;
                        currentPet.CareLevel++;
                        ClearLowerScreen();

                        if (ownedFoodList[selection].RestoredHappiness > 0)
                        {
                            if (currentPet.Happiness < 5)
                                currentPet.Happiness += 1;
                            Console.WriteLine($"Fed {currentPet.Name} {ownedFoodList[selection].Name}, +{ownedFoodList[selection].RestoredHunger} <3 +{ownedFoodList[selection].RestoredHappiness}:)\n(Press any key to continue...)");
                            currentPet.CareLevel -= 2;
                        }
                        else
                        {
                            Console.WriteLine($"Fed {currentPet.Name} {ownedFoodList[selection].Name}, +{ownedFoodList[selection].RestoredHunger} <3\n(Press any key to continue...)");
                        }
                    }
                    else
                    {
                        ClearLowerScreen();
                        Console.WriteLine($"{currentPet.Name} is already full!\n(Press any key to continue...)");
                        currentPet.CareLevel--;
                        SetIllness();
                    }
                    Console.ReadKey(true);
                }
                else if (userInput == 'd')
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
                        Console.WriteLine($"\"{shopItems[i]}\"");
                    else
                    {
                        Console.WriteLine(shopItems[i]);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == shopItems.Length - 1)
                        selection = 0;
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
                            if (!ownedGames.Contains("Higher or Lower?"))
                            {
                                if (currentPet.Money >= 150)
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
            int[] moneyAndHappiness = [0, 0];

            do
            {
                ClearLowerScreen();
                Console.WriteLine("Games:");

                for (int i = 0; i < ownedGames.Count; i++)
                {
                    if (selection == i)
                        Console.WriteLine($"\"{ownedGames[i]}\"");
                    else
                    {
                        Console.WriteLine(ownedGames[i]);

                    }
                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == ownedGames.Count - 1)
                        selection = 0;
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
                    if (currentPet.Happiness < 5)
                        currentPet.Happiness += moneyAndHappiness[1];

                    currentPet.Weight--;
                    if (moneyAndHappiness[1] > 0)
                        currentPet.CareLevel++;
                    tickTimer.Start();
                }
                else if (userInput == 'd')
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
                        Console.WriteLine($"\"{careOptions[i]}\"");
                    else
                    {
                        Console.WriteLine(careOptions[i]);
                    }


                }

                var userInput = Console.ReadKey(true).KeyChar;

                if (userInput == 's')
                {
                    if (selection == careOptions.Length - 1)
                        selection = 0;
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
            Console.SetCursorPosition(bottomScreenStartPositionXY[0], bottomScreenStartPositionXY[1]);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("".PadRight(45));
            }
            Console.SetCursorPosition(bottomScreenStartPositionXY[0], bottomScreenStartPositionXY[1]);
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
            tickTimer = new System.Timers.Timer(tickCount);
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
            FakeAttentionCheck();
            AttentionCheck();

            screen.ResetScreen();
            screen.UpdatePoops(poopPositions);
            screen.UpdatePetPosition(currentPet.Appearance, sick, wantsAttention);
            screen.DrawScreen();

        }

        void EvolveCheck()
        {
            if (currentPet.Stage <= 2)
            {
                if (activeEvolveTickCount <= 0)
                {
                    Evolve();
                    activeEvolveTickCount = evolveTickCount;
                    screen.ResetScreen();
                }
                else
                {
                    activeEvolveTickCount--;
                }
            }
        }

        void Evolve()
        {
            switch (currentPet.Stage)
            {
                case 0: //baby to child
                    currentPet.Appearance = petModels[1];
                    break;
                case 1: //child to teen
                    currentPet.Appearance = petModels[2];
                    break;
                case 2: //teen to adult
                    if (currentPet.CareLevel >= 50 && currentPet.Discipline >= 3)
                        currentPet.Appearance = petModels[5];
                    else if (currentPet.CareLevel >= 0 && currentPet.Discipline >= 1)
                    {
                        currentPet.Appearance = petModels[4];
                    }
                    else
                    {
                        currentPet.Appearance = petModels[3];
                    }
                    break;
                default:
                    break;
            }

            currentPet.Stage++;
        }

        void HappinessCheck()
        {
            if (activeHappinessTickCount <= 0)
            {
                if (currentPet.Happiness >= 5)
                    currentPet.CareLevel++;
                else if (currentPet.Happiness <= 0)
                {
                    currentPet.CareLevel--;
                    wantsAttention = true;
                    happyAttention = true;
                }

                if (currentPet.Happiness > 0)
                {
                    currentPet.Happiness--;
                    happyAttention = false;
                }

                activeHappinessTickCount = happinessTickCount;
            }
            else
            {
                activeHappinessTickCount--;
            }
        }

        void DeathCheck()
        {
            if (activeDeathCheckTickCount <= 0)
            {
                if (deathMistakes > 0)
                    deathMistakes--;

                if (deathMistakes > 5)
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

                activeDeathCheckTickCount = deathCheckTickCount;
            }
            else
            {
                activeDeathCheckTickCount--;
            }
        }

        void HungerCheck()
        {
            if (activeHungerTickCount <= 0)
            {
                if (currentPet.Hunger >= 5)
                    currentPet.CareLevel++;
                else if (currentPet.Hunger <= 0)
                {
                    currentPet.CareLevel--;
                    currentPet.Weight--;
                    deathMistakes++;
                    wantsAttention = true;
                    hungryAttention = true;
                }

                if (currentPet.Hunger > 0)
                {
                    currentPet.Hunger--;
                    AddPoop();
                    hungryAttention = false;
                }

                activeHungerTickCount = hungerTickCount;
            }
            else
            {
                activeHungerTickCount--;
            }
        }

        void IllnessCheck()
        {
            if (activeIllnessTickCount <= 0)
            {
                if (sick)
                {
                    currentPet.CareLevel--;
                    deathMistakes++;
                }
                else
                {
                    if (currentPet.Hunger == 0 || currentPet.Happiness == 0 || poopPositions.Count >= 4)
                        SetIllness();
                }

                activeIllnessTickCount = illnessTickCount;
            }
            else
            {
                activeIllnessTickCount--;
            }
        }

        void FakeAttentionCheck()
        {
            if (activeFakeAttentionTickCount <= 0)
            {
                if (Random.Shared.Next(1, 11) >= 7)
                {
                    wantsAttention = true;
                    fakeAttention = true;
                }
                activeFakeAttentionTickCount = fakeAttentionTickCount;
            }
            else
            {
                activeFakeAttentionTickCount--;
            }
        }

        void AttentionCheck()
        {
            if (!fakeAttention
            && !hungryAttention
            && !happyAttention)
            {
                wantsAttention = false;
            }
        }

        void SetIllness()
        {
            if (Random.Shared.Next(0, 10) >= 6)
            {
                sick = true;
                currentPet.CareLevel--;
            }
        }

        void GiveMedicine()
        {
            if (sick)
            {
                sick = false;
                currentPet.CareLevel++;
            }
            else
            {
                currentPet.CareLevel--;
            }
        }

        void Discipline()
        {
            if (fakeAttention)
            {
                currentPet.CareLevel++;
                currentPet.Discipline++;
                fakeAttention = false;
            }
            else
            {
                currentPet.CareLevel--;
                currentPet.Happiness--;
            }
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
            int x = Random.Shared.Next(1, 13);
            int y = Random.Shared.Next(1, 6);
            if (poopPositions.Count >= 5)
                currentPet.CareLevel--;
            poopPositions.Add([x, y]);
        }

        void CleanPoop()
        {
            if (poopPositions.Count > 0)
                currentPet.CareLevel++;
            poopPositions.Clear();
        }
    }
}