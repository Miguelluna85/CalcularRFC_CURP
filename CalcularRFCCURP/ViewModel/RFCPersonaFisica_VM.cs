using CalcularCURPyRFC.Models;
using System.Collections;
using System.Text;
namespace CalcularCURPyRFC.ViewModel;

public class RFCPersonaFisica_VM
{
    private PersonaFisica Persona;
    private string RFC;

    public RFCPersonaFisica_VM()
    {
        try
        {
            Persona = new PersonaFisica();
            RFC = string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public string CalcularRFC(PersonaFisica persona)
    {
        try
        {


            Persona = persona;
            Persona.PrimerNombre = !string.IsNullOrEmpty(Persona.PrimerNombre) ? QuitarArticulosPF(Persona.PrimerNombre.ToUpper().Trim()) : string.Empty;
            Persona.SegundoNombre = !string.IsNullOrEmpty(Persona.SegundoNombre) ? QuitarArticulosPF(Persona.SegundoNombre.ToUpper().Trim()) : string.Empty;
            Persona.ApellidoPaterno = !string.IsNullOrEmpty(Persona.ApellidoPaterno) ? QuitarArticulosPF(Persona.ApellidoPaterno.ToUpper().Trim()) : string.Empty;
            Persona.ApellidoMaterno = !string.IsNullOrEmpty(Persona.ApellidoMaterno) ? QuitarArticulosPF(Persona.ApellidoMaterno.ToUpper().Trim()) : string.Empty;

            Persona.PrimerNombre = !string.IsNullOrEmpty(Persona.PrimerNombre) ? StringExtensions.RemoveAccentMarks(Persona.PrimerNombre) : string.Empty;
            Persona.SegundoNombre = !string.IsNullOrEmpty(Persona.SegundoNombre) ? StringExtensions.RemoveAccentMarks(Persona.SegundoNombre) : string.Empty;
            Persona.ApellidoPaterno = !string.IsNullOrEmpty(Persona.ApellidoPaterno) ? StringExtensions.RemoveAccentMarks(Persona.ApellidoPaterno) : string.Empty;
            Persona.ApellidoMaterno = !string.IsNullOrEmpty(Persona.ApellidoMaterno) ? StringExtensions.RemoveAccentMarks(Persona.ApellidoMaterno) : string.Empty;

            Persona.PrimerNombre = !string.IsNullOrEmpty(Persona.PrimerNombre) ? RemoveCharacterSpecialPF(Persona.PrimerNombre) : string.Empty;
            Persona.SegundoNombre = !string.IsNullOrEmpty(Persona.SegundoNombre) ? RemoveCharacterSpecialPF(Persona.SegundoNombre) : string.Empty;
            Persona.ApellidoPaterno = !string.IsNullOrEmpty(Persona.ApellidoPaterno) ? RemoveCharacterSpecialPF(Persona.ApellidoPaterno) : string.Empty;
            Persona.ApellidoMaterno = !string.IsNullOrEmpty(Persona.ApellidoMaterno) ? RemoveCharacterSpecialPF(Persona.ApellidoMaterno) : string.Empty;

            //apellidos
            if (Persona.ApellidoPaterno.Length > 2)
            {
                RFC += Persona.ApellidoPaterno.Substring(0, 1);

                if (StringExtensions.EsVocal(Convert.ToChar(Persona.ApellidoPaterno.Substring(0, 1))))
                {
                    Persona.ApellidoPaterno =
                        Persona.ApellidoPaterno.Substring(1, Persona.ApellidoPaterno.Length - 1);
                }
                foreach (char caracter in Persona.ApellidoPaterno)
                {
                    if (StringExtensions.EsVocal(caracter))
                    {
                        RFC += caracter;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(Persona.ApellidoMaterno))
                    RFC += Persona.ApellidoMaterno.Substring(0, 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(Persona.ApellidoMaterno))
                {
                    RFC += Persona.ApellidoPaterno.Substring(0, 1);
                    RFC += Persona.ApellidoMaterno.Substring(0, 1);
                }
                else
                {
                    RFC += Persona.ApellidoPaterno.Substring(0, 2);
                }
            }

            //nombres         
            if (esNombreComun(Persona.PrimerNombre))
            {
                if (!string.IsNullOrEmpty(Persona.SegundoNombre))
                    RFC += Persona.SegundoNombre.Substring(0, 1);
                else
                    RFC += Persona.PrimerNombre.Substring(0, 1);
            }
            else
            {
                if (Persona.ApellidoPaterno.Length <= 2)
                    RFC += Persona.PrimerNombre.Substring(0, 2);
                else
                    RFC += Persona.PrimerNombre.Substring(0, 1);

                if (string.IsNullOrEmpty(Persona.ApellidoMaterno))
                    RFC += Persona.PrimerNombre.Substring(1, 1);
            }

            if (StringExtensions.PalabrasInconvenientesCURP.Contains(RFC))
                RFC = RFC.Substring(0, 3) + "X";

            //Add fecha
            string anyo = (Persona.FechaNacimiento.Year.ToString().Length == 1) ? "0" + Persona.FechaNacimiento.Year.ToString() : Persona.FechaNacimiento.Year.ToString();
            string mes = (Persona.FechaNacimiento.Month.ToString().Length == 1) ? "0" + Persona.FechaNacimiento.Month.ToString() : Persona.FechaNacimiento.Month.ToString();
            string dia = (Persona.FechaNacimiento.Day.ToString().Length == 1) ? "0" + Persona.FechaNacimiento.Day.ToString() : Persona.FechaNacimiento.Day.ToString();

            RFC += anyo.Substring(2, 2) + mes + dia;

            CalcularHomoclave(
                Persona.ApellidoPaterno + " " +
                Persona.ApellidoMaterno + " " +
                Persona.PrimerNombre,
                dia + mes + anyo, ref RFC);

            return RFC;
        }
        catch (Exception ex)
        {
            RFC = "";
            throw ex;
        }
    }
    public string QuitarArticulosPF(string palabra)
    {
        return palabra
            .Replace("DEL ", String.Empty)
            .Replace("LAS ", String.Empty)
            .Replace("DE ", String.Empty)
            .Replace(" DE", String.Empty)
            .Replace("LA ", String.Empty)
            .Replace("Y ", String.Empty)
            .Replace("A ", String.Empty)
            .Replace("LOS ", String.Empty)
            .Replace("LO ", String.Empty);
    }
    public static string RemoveCharacterSpecialPF(string str)
    {
        str = str.Replace("\'", "")
            .Replace("A´", "A")
            .Replace("E´", "E")
            .Replace("I´", "I")
            .Replace("O´", "O")
            .Replace("U´", "U");

        return str;
    }
    //Regresa la primera vocal del primer nombre ignorando los nombres comunes como José o María
    public bool esNombreComun(string nombre)
    {
        nombre = nombre.ToUpper();

        string[] nombres = nombre.Split("\\s");

        if (nombres.Length >= 1)
            if (nombres[0].Equals("JOSE")
                || nombres[0].Equals("MARIA")
                || nombres[0].Equals("MA.")
                || nombres[0].Equals("MA"))
                return true;

        return false;
    }
    private string quitarNombreComun(string nombre)
    {

        string[] nombres = nombre.Split("\\s");

        if (nombres.Length > 1)
            if (nombres[0].Equals("JOSE")
                || nombres[0].Equals("MARIA")
                || nombres[0].Equals("MA.")
                || nombres[0].Equals("MA"))

                return nombres[1].ToString();

        return nombre[0].ToString();
    }
    public void CalcularHomoclave(string nombreCompleto, string fecha, ref string rfc)
    {
        //Guardara el nombre en su correspondiente numérico
        StringBuilder nombreEnNumero = new StringBuilder();
        //La suma de la secuencia de números de nombreEnNumero
        long valorSuma = 0;

        #region Tablas para calcular la homoclave
        //Estas tablas realmente no se porque son como son
        //solo las copie de lo que encontré en internet

        #region TablaRFC 1
        Hashtable tablaRFC1 = new Hashtable();
        tablaRFC1.Add("&", 10);
        tablaRFC1.Add("Ñ", 10);
        tablaRFC1.Add("A", 11);
        tablaRFC1.Add("B", 12);
        tablaRFC1.Add("C", 13);
        tablaRFC1.Add("D", 14);
        tablaRFC1.Add("E", 15);
        tablaRFC1.Add("F", 16);
        tablaRFC1.Add("G", 17);
        tablaRFC1.Add("H", 18);
        tablaRFC1.Add("I", 19);
        tablaRFC1.Add("J", 21);
        tablaRFC1.Add("K", 22);
        tablaRFC1.Add("L", 23);
        tablaRFC1.Add("M", 24);
        tablaRFC1.Add("N", 25);
        tablaRFC1.Add("O", 26);
        tablaRFC1.Add("P", 27);
        tablaRFC1.Add("Q", 28);
        tablaRFC1.Add("R", 29);
        tablaRFC1.Add("S", 32);
        tablaRFC1.Add("T", 33);
        tablaRFC1.Add("U", 34);
        tablaRFC1.Add("V", 35);
        tablaRFC1.Add("W", 36);
        tablaRFC1.Add("X", 37);
        tablaRFC1.Add("Y", 38);
        tablaRFC1.Add("Z", 39);
        tablaRFC1.Add("0", 0);
        tablaRFC1.Add("1", 1);
        tablaRFC1.Add("2", 2);
        tablaRFC1.Add("3", 3);
        tablaRFC1.Add("4", 4);
        tablaRFC1.Add("5", 5);
        tablaRFC1.Add("6", 6);
        tablaRFC1.Add("7", 7);
        tablaRFC1.Add("8", 8);
        tablaRFC1.Add("9", 9);
        #endregion

        #region TablaRFC 2
        Hashtable tablaRFC2 = new Hashtable();
        tablaRFC2.Add(0, "1");
        tablaRFC2.Add(1, "2");
        tablaRFC2.Add(2, "3");
        tablaRFC2.Add(3, "4");
        tablaRFC2.Add(4, "5");
        tablaRFC2.Add(5, "6");
        tablaRFC2.Add(6, "7");
        tablaRFC2.Add(7, "8");
        tablaRFC2.Add(8, "9");
        tablaRFC2.Add(9, "A");
        tablaRFC2.Add(10, "B");
        tablaRFC2.Add(11, "C");
        tablaRFC2.Add(12, "D");
        tablaRFC2.Add(13, "E");
        tablaRFC2.Add(14, "F");
        tablaRFC2.Add(15, "G");
        tablaRFC2.Add(16, "H");
        tablaRFC2.Add(17, "I");
        tablaRFC2.Add(18, "J");
        tablaRFC2.Add(19, "K");
        tablaRFC2.Add(20, "L");
        tablaRFC2.Add(21, "M");
        tablaRFC2.Add(22, "N");
        tablaRFC2.Add(23, "P");
        tablaRFC2.Add(24, "Q");
        tablaRFC2.Add(25, "R");
        tablaRFC2.Add(26, "S");
        tablaRFC2.Add(27, "T");
        tablaRFC2.Add(28, "U");
        tablaRFC2.Add(29, "V");
        tablaRFC2.Add(30, "W");
        tablaRFC2.Add(31, "X");
        tablaRFC2.Add(32, "Y");
        #endregion

        #region TablaRFC 3
        Hashtable tablaRFC3 = new Hashtable();
        tablaRFC3.Add("A", 10);
        tablaRFC3.Add("B", 11);
        tablaRFC3.Add("C", 12);
        tablaRFC3.Add("D", 13);
        tablaRFC3.Add("E", 14);
        tablaRFC3.Add("F", 15);
        tablaRFC3.Add("G", 16);
        tablaRFC3.Add("H", 17);
        tablaRFC3.Add("I", 18);
        tablaRFC3.Add("J", 19);
        tablaRFC3.Add("K", 20);
        tablaRFC3.Add("L", 21);
        tablaRFC3.Add("M", 22);
        tablaRFC3.Add("N", 23);
        tablaRFC3.Add("O", 25);
        tablaRFC3.Add("P", 26);
        tablaRFC3.Add("Q", 27);
        tablaRFC3.Add("R", 28);
        tablaRFC3.Add("S", 29);
        tablaRFC3.Add("T", 30);
        tablaRFC3.Add("U", 31);
        tablaRFC3.Add("V", 32);
        tablaRFC3.Add("W", 33);
        tablaRFC3.Add("X", 34);
        tablaRFC3.Add("Y", 35);
        tablaRFC3.Add("Z", 36);
        tablaRFC3.Add("0", 0);
        tablaRFC3.Add("1", 1);
        tablaRFC3.Add("2", 2);
        tablaRFC3.Add("3", 3);
        tablaRFC3.Add("4", 4);
        tablaRFC3.Add("5", 5);
        tablaRFC3.Add("6", 6);
        tablaRFC3.Add("7", 7);
        tablaRFC3.Add("8", 8);
        tablaRFC3.Add("9", 9);
        tablaRFC3.Add("", 24);
        tablaRFC3.Add(" ", 37);
        #endregion

        #endregion

        //agregamos un cero al inicio de la representación númerica del nombre
        nombreEnNumero.Append("0");

        //Recorremos el nombre y vamos convirtiendo las letras en
        //su valor numérico
        foreach (char c in nombreCompleto)
        {
            if (tablaRFC1.ContainsKey(c.ToString()))
                nombreEnNumero.Append(tablaRFC1[c.ToString()].ToString());
            else
                nombreEnNumero.Append("00");
        }
        //Calculamos la suma de la secuencia de números
        //calculados anteriormente
        //la formula es:
        //( (el caracter actual multiplicado por diez)
        //mas el valor del caracter siguiente )
        //(y lo anterior multiplicado por el valor del caracter siguiente)
        for (int i = 0; i < nombreEnNumero.Length - 1; i++)
        {
            valorSuma += ((Convert.ToInt32(nombreEnNumero[i].ToString()) * 10) + Convert.ToInt32(nombreEnNumero[i + 1].ToString())) * Convert.ToInt32(nombreEnNumero[i + 1].ToString());
        }

        //Lo siguiente no se porque se calcula así, es parte del algoritmo.
        //Los magic numbers que aparecen por ahí deben tener algún origen matemático
        //relacionado con el algoritmo al igual que el proceso mismo de calcular el
        //digito verificador.
        //Por esto no puedo añadir comentarios a lo que sigue, lo hice por acto de fe.

        int div = 0, mod = 0;
        div = Convert.ToInt32(valorSuma) % 1000;
        mod = div % 34;
        div = (div - mod) / 34;

        int indice = 0;
        string hc = String.Empty; //los dos primeros caracteres de la homoclave
        while (indice <= 1)
        {
            if (tablaRFC2.ContainsKey((indice == 0) ? div : mod))
                hc += tablaRFC2[(indice == 0) ? div : mod];
            else
                hc += "Z";
            indice++;
        }

        //Agregamos al RFC los dos primeros caracteres de la homoclave
        rfc += hc;

        //Aqui empieza el calculo del digito verificador basado en lo que tenemos del RFC
        //En esta parte tampoco conozco el origen matemático del algoritmo como para dar
        //una explicación del proceso, así que ¡tengamos fe hermanos!.
        int rfcAnumeroSuma = 0, sumaParcial = 0;
        for (int i = 0; i < rfc.Length; i++)
        {
            if (tablaRFC3.ContainsKey(rfc[i].ToString()))
            {
                rfcAnumeroSuma = Convert.ToInt32(tablaRFC3[rfc[i].ToString()]);
                sumaParcial += (rfcAnumeroSuma * (14 - (i + 1)));
            }
        }

        int moduloVerificador = sumaParcial % 11;
        if (moduloVerificador == 0)
            rfc += "0";
        else
        {
            sumaParcial = 11 - moduloVerificador;
            if (sumaParcial == 10)
                rfc += "A";
            else
                rfc += sumaParcial.ToString();
        }

        //en este punto la variable rfc pasada ya debe tener la homoclave
        //recuerda que la variable rfc se paso como "ref string" lo cual
        //hace que se modifique la original.
    }

}
