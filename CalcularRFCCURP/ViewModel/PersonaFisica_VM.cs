using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CalcularCURPyRFC.ViewModel;

public class PersonaFisica_VM
{
    public int ID { get; set; }

    
    [Required(ErrorMessage = "Dato obligatorio"), MinLength(2, ErrorMessage = "Tamaño min. 2 caracteres"), StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Primer Nombre")]
    public string primerNombre { get; set; } = string.Empty;

    
    [StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Segundo Nombre")]
    public string? segundoNombre { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Dato obligatorio"), MinLength(2, ErrorMessage = "Tamaño min. 2 caracteres"), StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Apellido Paterno")]
    public string Paterno { get; set; } = string.Empty;

    
    [StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Apellido Materno")]
    public string? Materno { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dato obligatorio")]
    [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    //[Range(typeof(DateTime), "01/01/1980", "01/01/2090", ErrorMessage = "fecha aceptadas entre {1} y {2}")]
    public DateTime fechaNacimiento { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Dato obligatorio"), Range(1, int.MaxValue, ErrorMessage = "Seleccione Sexo")]
    public string SelectedSexoID { get; set; } = string.Empty;
    public SelectList? ListSexo { get; set; }


    [Required(ErrorMessage = "Dato obligatorio"), Range(1, int.MaxValue, ErrorMessage = "Seleccione Estado")]
    public int SelectedEstadoID { get; set; }
    public SelectList? ListEstado { get; set; }

    public PersonaFisica_VM()
    {
        GetListSexos();
        GetListEstados();
    }
    private void GetListSexos()
    {
        
        var enumSexos = from Enums.Sexo e in Enum.GetValues(typeof(Enums.Sexo))
                        select new
                        {
                            Text = e.ToString(),
                            Value = ((int)e).ToString()
                        };

        //SelectList listaSexo = new SelectList(enumData,"ID","Name");
        //ViewBag.EnumListSexo = listaSexo;
        ListSexo = new SelectList(enumSexos, "Value", "Text");
    }
    private void GetListEstados()
    {
        var enumEstados = from Enums.Estado e in Enum.GetValues(typeof(Enums.Estado))
                          select new
                          {
                              ID = (int)e,
                              Name = e.ToString()
                          };
        ListEstado = new SelectList(enumEstados, "ID", "Name");
    }

}
