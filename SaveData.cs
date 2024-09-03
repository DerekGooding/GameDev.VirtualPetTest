using System.Text.Json;
using VirtualPetTest.Model;

namespace VirtualPetTest;


class SaveData()
{
    public Pet CurrentPetData { get; set; }
    public List<string> OwnedFood { get; set; }
    public List<string> OwnedGames { get; set; }
}
