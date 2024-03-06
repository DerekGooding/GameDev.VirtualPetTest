
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main()
    {
        Console.WriteLine($"Virtual Pet Test");

        Pet currentPet = new("Timmy");

        

        string[] screenLines = [
            "---------------",
            "|             |",
            "|             |",
            "|             |",
            "|             |",
            "|             |",
            "|             |",
            "---------------" ];

        foreach (string screenLine in screenLines)
        {
            Console.WriteLine(screenLine);
        }





        Console.WriteLine(
            $"Stats:\nName: {currentPet.Name}\nAppearance: {currentPet.Appearance}\nAge: {currentPet.Age}");



    }
}

class Movement()
{
    async Task<bool> MovePet()
    {
        bool isMoving = true;

        Random random = new();
        int bottomScreenY =  Main   screenLines.Length - 1;
        int topScreenY = 3;
        int leftScreenX = 1;
        int rightScreenX = screenLines[0].Length - 1;

        if (isMoving)
        {
            int setX = random.Next(leftScreenX, rightScreenX + 1);
            int setY = random.Next(topScreenY, bottomScreenY + 1);

            Console.SetCursorPosition(setX, setY);
            Console.Write(currentPet.Appearance);
        }


        return isMoving;
    }
}

class Pet(string name, string appearance = "(0c0)", int age = 1, int hunger = 3, int happiness = 3)
{
    public string Name { get; set; } = name;
    public string Appearance { get; set; } = appearance;
    public int Age { get; set; } = age;
    public int Hunger { get; set; } = hunger;
    public int Happiness { get; set; } = happiness;
}

