namespace VirtualPet.Controller;

class Program
{
    static async void Main()
    {
        Console.WriteLine($"Virtual Pet Test");

        bool isMoving = true;


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


        async void MovePet()
        {
            Random random = new();
            int bottomScreenY = screenLines.Length - 1;
            int topScreenY = screenLines.Length - 7;

            if (isMoving)
            {
                int setY = random.Next(topScreenY, bottomScreenY + 1);

                int col = screenLines[0].Length - 6;
                int savedRow = Console.GetCursorPosition().Top;
                int savedCol = Console.GetCursorPosition().Left;
                Console.SetCursorPosition(col, screenHeight);
                Console.Write(currentPet.Appearance);
                Console.SetCursorPosition(savedCol, savedRow);
            }
        }



        Console.WriteLine(
            $"Stats:\nName: {currentPet.Name}\nAppearance: {currentPet.Appearance}\nAge: {currentPet.Age}");



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

