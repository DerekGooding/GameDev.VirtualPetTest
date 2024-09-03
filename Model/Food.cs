using System.Text.Json;

namespace VirtualPetTest.Model;

class Food(string name, int restoredHunger = 1, int restoredHappiness = 0, int weightGained = 1, bool owned = false)
{
    public string Name { get; set; } = name;
    public int RestoredHunger { get; set; } = restoredHunger;
    public int RestoredHappiness { get; set; } = restoredHappiness;
    public int WeightGained { get; set; } = weightGained;
    public bool Owned { get; set; } = owned;
}
