using System.Text.Json;

namespace VirtualPetTest.Model;

class Pet(string name, string birthday = "1/1/1 11:11", string appearance = "@", int age = 1, int hunger = 3, int happiness = 3, int money = 150, int weight = 10, int discipline = 0, int stage = 0, int careLevel = 0, int evolutionProgress = 0)
{
    public string Name { get; set; } = name;
    public string Appearance { get; set; } = appearance;
    public int Age { get; set; } = age;
    public int Hunger { get; set; } = hunger;
    public int Happiness { get; set; } = happiness;
    public int Money { get; set; } = money;
    public string Birthday { get; set; } = birthday;
    public int Weight { get; set; } = weight;
    public int Discipline { get; set; } = discipline;
    public int Stage { get; set; } = stage;
    public int CareLevel { get; set; } = careLevel;
    public int EvolutionProgress { get; set; } = evolutionProgress;

}
