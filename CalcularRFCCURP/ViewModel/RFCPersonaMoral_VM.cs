using CalcularCURPyRFC.Models;
using System.Text.RegularExpressions;

namespace CalcularCURPyRFC.ViewModel;

public class RFCPersonaMoral_VM
{
    private PersonaMoral Persona;
    private string RFC;
    private string AuxRazonSocial;
    
    public RFCPersonaMoral_VM()
    {
        Persona = new PersonaMoral();
        RFC = string.Empty;
        AuxRazonSocial = string.Empty;
    }
    
    public string CalcularRFC(PersonaMoral persona)
    {       
            Persona = persona;
        AuxRazonSocial = Persona.RazonSocial.ToUpper().Trim();

        AuxRazonSocial = !String.IsNullOrEmpty(AuxRazonSocial) ? StringExtensions.RemoveAccentMarks(AuxRazonSocial) : String.Empty;
        AuxRazonSocial = QuitarComas(AuxRazonSocial);
        AuxRazonSocial = Quitar_CIA_SOC(AuxRazonSocial);
        AuxRazonSocial = QuitarCaracteresEspecialesPM(AuxRazonSocial);
        AuxRazonSocial = NumeroArabicoALetra(AuxRazonSocial);
        AuxRazonSocial = QuitarArticulosPM(AuxRazonSocial);

        AuxRazonSocial = QuitarDenominacionSocialPM(AuxRazonSocial);
        AuxRazonSocial = NumeroRomanoALetra(AuxRazonSocial);

        //string[] nombres = PersonaMoralRFC.RazonSocial.Split(' ');
        //if (nombres.Length > 3)//no cuadra
        //    PersonaMoralRFC.RazonSocial = nombres[0] + " " + nombres[1] + " " + nombres[2];

        RFC = CalcularParteNombre(AuxRazonSocial);

        string anyo = (Persona.FechaConstitucion.Year.ToString().Length == 1) ? "0" + Persona.FechaConstitucion.Year.ToString() : Persona.FechaConstitucion.Year.ToString();
        string mes = (Persona.FechaConstitucion.Month.ToString().Length == 1) ? "0" + Persona.FechaConstitucion.Month.ToString() : Persona.FechaConstitucion.Month.ToString();
        string dia = (Persona.FechaConstitucion.Day.ToString().Length == 1) ? "0" + Persona.FechaConstitucion.Day.ToString() : Persona.FechaConstitucion.Day.ToString();

        RFC += anyo.Substring(2, 2) + mes + dia;

        RFCPersonaFisica_VM personaFisica = new RFCPersonaFisica_VM();
        personaFisica.CalcularHomoclave(AuxRazonSocial, dia + mes + anyo, ref RFC);

        return RFC;
    }
    private string CalcularParteNombre(string razonsocial)
    {
        string rfc = string.Empty;
        string[] nombres = razonsocial.Split(' ');

        for (int i = 0; i <= nombres.Length - 1; i++)
        {
            if (IsIniciales(nombres[i]))
                rfc = nombres[i].Replace(".", "");
        }
        if (rfc.Length > 0)
        {
            if (rfc.Length == 1)
            {
                if (nombres.Length >= 2)
                {
                    rfc += nombres[1].Substring(0, 1);
                    rfc += nombres[2].Substring(0, 1);
                }
                else
                {
                    rfc += "XX";
                }
            }
            else if (rfc.Length == 2)
            {
                if (nombres.Length == 1)
                    rfc += "X";
                else
                    rfc += nombres[1].Substring(0, 1);
            }
        }
        else
        {
            if (nombres.Length == 1)
            {
                if (nombres[0].Length >= 3)
                    rfc = nombres[0].Substring(0, 3);

                else if (nombres[0].Length == 2)
                    rfc = nombres[0].Substring(0, 2) + "X";

                else if (nombres[0].Length == 1)
                    rfc = nombres[0].Substring(0, 1) + "XX";
            }
            else if (nombres.Length == 2)
                rfc = nombres[0].Substring(0, 1) + nombres[1].Substring(0, 2);

            else if (nombres.Length >= 3)
                rfc = nombres[0].Substring(0, 1) + nombres[1].Substring(0, 1) + nombres[2].Substring(0, 1);
        }

        return rfc;
    }
    private string QuitarArticulosPM(string razonSocialMayus)
    {
        List<string> auxRazonSocial = new List<string>();

        List<string> palabras = razonSocialMayus.Split(' ')
            .Where(P => !string.IsNullOrEmpty(P))
            .ToList();

        string[] ArrayArticulos = new[] { "A", "DE", "DEL", "Y", "EL", "LA", "LAS", "LO", "LOS", "PARA" };

        auxRazonSocial = palabras
            .Where(P => !ArrayArticulos.Contains(P))
            .ToList();

        return string.Join(" ", auxRazonSocial) ?? "";
    }
    private string QuitarDenominacionSocialPM(string razonSocialMayus)
    {
        List<string> auxRazonSocial = new List<string>();

        List<string> palabras = razonSocialMayus.Split(' ')
            .Where(P => !string.IsNullOrEmpty(P))
            .ToList();

        List<string> ArrayDenominacionSocial = new List<string>{
            "S.","R.L.","S.A. DE C.V.","SA DE CV","S.A DE C.V.","S.A. DE C.V","SA. DE C.V.","SA DE CV."
            ,"SA DE CV.","S.A.","S.A","SA","C.V","CV","CV."
            ,"S. DE. R.L.","S. R.L.","S DE R.L.","S. DE RL.","S. DE R.L","S DE RL","S DE R.L","S. DE RL"
            , "S. EN N.C.", "S. EN C."
            , "S. EN C. POR A.", "S.N.C.", "S.C." ,"C.V.","A.C.","A. EN P.","S.C.L.","S.C.S."
            ,"COMPAÑIA","CIA.","SOCIEDAD","SOC."};
        auxRazonSocial = palabras
            .Where(P => !ArrayDenominacionSocial.Contains(P))
            .ToList();

        return string.Join(" ", auxRazonSocial) ?? "";
    }
    private string Quitar_CIA_SOC(string razonSocialMayus)
    {
        List<string> auxRazonSocial = new List<string>();

        List<string> palabras = razonSocialMayus.Split(' ')
            .Where(P => !string.IsNullOrEmpty(P))
            .ToList();

        string[] ArrayDenominacionSocial = new[] { "COMPAÑIA", "CIA.", "SOCIEDAD", "SOC." };

        auxRazonSocial = palabras
            .Where(P => !ArrayDenominacionSocial.Contains(P))
            .ToList();

        return string.Join(" ", auxRazonSocial) ?? "";
    }
    public string QuitarComas(string razonSocialMayus)
    {
        return razonSocialMayus
            .Replace(",", "");
    }
    private string QuitarCaracteresEspecialesPM(string razonSocialMayus)
    {
        List<string> auxRazonSocial = new List<string>();

        List<string> palabras = razonSocialMayus.Split(' ')
            .Where(P => !string.IsNullOrEmpty(P))
            .ToList();

        Dictionary<string, string> DiccionarySpecial = new Dictionary<string, string>
        {
            {"@","ARROBA" },
            {"%","PORCENTAJE" },
            {" ","BLANCA" },
            {"#","NUMERAL" },
            {"/","DIAGONAL" }
        };

        bool isSpecialValue = false;

        foreach (string palabra in palabras)
        {
            string? value = string.Empty;
            isSpecialValue = DiccionarySpecial.TryGetValue(palabra, out value);

            if (isSpecialValue)
                auxRazonSocial.Add(value);
            else
                auxRazonSocial.Add(palabra);
        }

        string rfcAux = string.Join(" ", auxRazonSocial) ?? "";

        if (!String.IsNullOrEmpty(rfcAux))
        {
            rfcAux = rfcAux
                .Replace("@", "")
                .Replace("%", "")
                .Replace("#", "")
                .Replace("/", "");
        }

        return rfcAux;
    }
    private bool IsIniciales(string palabraMayus)
    {
        string patter = @"^([A-Z]\.)+$";
        return Regex.IsMatch(palabraMayus, patter);
    }
    private string NumeroArabicoALetra(string palabraMayus)
    {
        string[] auxPalabras = palabraMayus.Split(' ');
        List<string> auxArrayConvert = new List<string>();
        ArabicoALetras ArabicoConvert = new ArabicoALetras();
        string decima = string.Empty;
        string numeroAPalabra = string.Empty;
        Int64 entero = 0;
        //int decimales = 0;
        double numero = 0.0D;

        foreach (string palabra in auxPalabras)
        {
            if (double.TryParse(palabra, out numero))
            {
                entero = Convert.ToInt64(Math.Truncate(numero));
                // decimales = Convert.ToInt32(Math.Round((numero - entero) * 100, 2));
                numeroAPalabra = ArabicoConvert.ArabicoToLetras(entero);
                //if (decimales > 0)
                //    decima = " CON " + decimales.ToString() + "/100";

                auxArrayConvert.Add(numeroAPalabra);
            }
            else
            {
                auxArrayConvert.Add(palabra);
            }
        }
        return string.Join(" ", auxArrayConvert);
    }
    private string NumeroRomanoALetra(string palabraMayus)
    {
        RomanoAArabico RomanoConvert = new RomanoAArabico();
        ArabicoALetras ArabicoConvert = new ArabicoALetras();
        string[] auxPalabras = palabraMayus.Split(' ');
        List<string> auxArrayConvert = new List<string>();

        foreach (string palabra in auxPalabras)
        {
            if (RomanoConvert.IsRomanoValid(palabra))
            {
                string romanoString = RomanoConvert.RomanoToArabico(palabra).ToString();
                double arabico = 0.0D;

                if (double.TryParse(romanoString, out arabico))
                {
                    auxArrayConvert.Add(ArabicoConvert.ArabicoToLetras(arabico));
                }
                else
                {
                    //si genera error que se hace??
                }

            }

            else
            {
                auxArrayConvert.Add(palabra);
            }
        }

        return string.Join(" ", auxArrayConvert);
    }
}
