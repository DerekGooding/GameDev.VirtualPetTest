namespace VirtualPet.Controller;

class Program
{
    static void Main()
    {
        Console.WriteLine($"Virtual Pet Test");

        Pet currentPet = new(Console.ReadLine());
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

