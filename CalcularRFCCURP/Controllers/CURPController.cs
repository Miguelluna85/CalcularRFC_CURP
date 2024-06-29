using CalcularCURPyRFC.Models;
using CalcularCURPyRFC.ViewModel;
using CalcularCURPyRFC.ViewModel.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CalcularCURPyRFC.Controllers;
public class CURPController : Controller
{

    public IActionResult FormCurp()
    {
        try
        {
            PersonaFisica_VM Persona = new PersonaFisica_VM();
            return View(Persona);
        }
        catch (Exception)
        {
            throw;
        }

    }

    [HttpPost]
    public IActionResult ResultadoCURP(PersonaFisica_VM PersonaVM)
    {
        try
        {
            if (PersonaVM == null)
                return View(PersonaVM);

            DateTime fechaNac;
            if (!DateTime.TryParse(PersonaVM.fechaNacimiento.ToString(), out fechaNac))
                return View(PersonaVM);

            CURP_VM CURP = new CURP_VM();

            PersonaFisica Persona = new PersonaFisica()
            {
                PrimerNombre = PersonaVM.primerNombre,
                SegundoNombre = PersonaVM.segundoNombre,
                ApellidoPaterno = PersonaVM.Paterno,
                ApellidoMaterno = PersonaVM.Materno,
                FechaNacimiento = PersonaVM.fechaNacimiento,
                Sexo = (Sexo)(Convert.ToChar(Convert.ToInt32((PersonaVM.SelectedSexoID)))),
                Estado = (Estado)(Convert.ToInt32((PersonaVM.SelectedEstadoID)))
            };

            string curp = CURP.Generar(Persona);

            return View(model: curp);
        }
        catch (Exception)
        {

            throw;
        }
    }


}

