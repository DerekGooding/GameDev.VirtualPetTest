using System.Text.Json;

namespace VirtualPetTest.Model;

public class Screen()
{
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

    public void UpdatePetPosition(string petAppearance, bool sick, bool wantsAttention)
    {
        int x = Random.Shared.Next(1, 12);
        int y = Random.Shared.Next(1, 7);
        screenLines[petY] = screenLines[petY].Replace(petAppearance, "".PadRight(petAppearance.Length));
        screenLines[y] = screenLines[y].Insert(x, petAppearance);
        screenLines[y] = screenLines[y].Remove(x + petAppearance.Length, petAppearance.Length);

        if (sick)
        {
            screenLines[petY - 1] = screenLines[petY - 1].Replace("X", " ");
            screenLines[y - 1] = screenLines[y - 1].Insert(x, "X");
            screenLines[y - 1] = screenLines[y - 1].Remove(x + 1, 1);
        }
        if (wantsAttention)
        {
            screenLines[petY - 1] = screenLines[petY - 1].Replace("!", " ");
            screenLines[y - 1] = screenLines[y - 1].Insert(x + 1, "!");
            screenLines[y - 1] = screenLines[y - 1].Remove(x + 2, 1);
        }

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
        foreach (int[] poop in poopPositions)
        {
            screenLines[poop[1]] = screenLines[poop[1]].Insert(poop[0], "S");
            screenLines[poop[1] + 1] = screenLines[poop[1] + 1].Insert(poop[0], "*");
            screenLines[poop[1]] = screenLines[poop[1]].Remove(poop[0] + 1, 1);
            screenLines[poop[1] + 1] = screenLines[poop[1] + 1].Remove(poop[0] + 1, 1);
        }
    }

}
