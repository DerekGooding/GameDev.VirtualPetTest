using System.Text.Json;

namespace VirtualPetTest.Model;

public class Games
{
    Screen screen = new();

    public int[] LeftOrRightGame()
    {
        int score = 0;
        int side = 0;

        for (int i = 0; i < 5; i++)
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

            side = Random.Shared.Next(2);
            char answer = Console.ReadKey(true).KeyChar;
            if (answer == 'a')
            {
                if (side == 0)
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
            else if (answer == 's')
            {
                if (side == 1)
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
            else if (answer == 'd')
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
        if (score > 2)
            happinessGained++;
        Console.WriteLine($" Score: {score}\n Money: +{score * 10}\n Happiness: +{happinessGained}");
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
            nextNumber = Random.Shared.Next(1, 10);
            while (nextNumber == currentNumber)
            {
                nextNumber = Random.Shared.Next(1, 10);
            }

            if (nextNumber > currentNumber)
                higher = true;
            else
            {
                higher = false;
            }

            char userInput = Console.ReadKey(true).KeyChar;

            if (userInput == 'a' && higher) //higher
                score++;
            else if (userInput == 's' && !higher) //lower
            {
                score++;
            }
            else if (userInput == 'd')
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
            happinessGained++;
        Console.WriteLine($" Score: {score}\n Money: +{score * 10}\n Happiness: +{happinessGained}");

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
