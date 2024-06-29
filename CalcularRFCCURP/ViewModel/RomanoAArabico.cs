using System.Text.RegularExpressions;

namespace CalcularCURPyRFC.ViewModel;

public class RomanoAArabico
{
    public bool IsRomanoValid(string palabraMayus)
    {
        string pattern = @"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$";

        return Regex.IsMatch(palabraMayus, pattern);
    }

    public int RomanoToArabico(string NumRomano)
    {
        if (string.IsNullOrEmpty(NumRomano)) return 0;

        Dictionary<char, int> romanValues = new Dictionary<char, int>()
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        int numArabico = 0;
        int prevValue = 0;

        for (int i = NumRomano.Length - 1; i >= 0; i--)
        {
            int currentValue = romanValues[NumRomano[i]];

            if (currentValue < prevValue)
                numArabico -= currentValue;
            else
                numArabico += currentValue;

            prevValue = currentValue;
        }

        return numArabico;
    }

     
}
