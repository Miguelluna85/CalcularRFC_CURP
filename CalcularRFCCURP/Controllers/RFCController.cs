using CalcularCURPyRFC.Models;
using CalcularCURPyRFC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CalcularCURPyRFC.Controllers;
public class RFCController : Controller
{
    private RFCPersonaFisica_VM? PersonaFisicaRFC;
    private RFCPersonaMoral_VM? PersonaMoralRFC;
    public string RFCResultado = string.Empty;

    public IActionResult PersonaFisica()
    {
        return View();
    }
    public IActionResult RFCCalculadoPF(PersonaFisica persona)
    {
        try
        {
            PersonaFisicaRFC = new RFCPersonaFisica_VM();
            RFCResultado = PersonaFisicaRFC.CalcularRFC(persona);

            return View(model: RFCResultado);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public IActionResult PersonaMoral()
    {
        return View();
    }
    public IActionResult RFCCalculadoPM(PersonaMoral persona)
    {
        try
        {


            PersonaMoralRFC = new RFCPersonaMoral_VM();
            RFCResultado = PersonaMoralRFC.CalcularRFC(persona);

            return View(model: RFCResultado);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
