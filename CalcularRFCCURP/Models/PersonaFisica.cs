using CalcularCURPyRFC.ViewModel.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CalcularCURPyRFC.Models;

public class PersonaFisica
{
    public int ID { get; set; }
    

    [Required(ErrorMessage = "Dato obligatorio"), MinLength(2, ErrorMessage = "Tamaño min. 2 caracteres"), StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Primer Nombre")]
    public string PrimerNombre { get; set; }=string.Empty;
    

    [StringLength(50,ErrorMessage = "Tamaño max. 50 caracteres")]
    public string? SegundoNombre { get; set; }
    

    [Required(ErrorMessage = "Dato obligatorio"), 
        MinLength(2, ErrorMessage = "Tamaño min. 2 caracteres"), 
        StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"), DisplayName("Apellido Paterno")]
    public string ApellidoPaterno { get; set; }=string.Empty;
    
    
    [StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres"),DisplayName("Apellido Materno")]
    public string? ApellidoMaterno { get; set; }


    [Required(ErrorMessage = "Dato obligatorio")]
    [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    public char SexoID { get; set; }
 
    public Sexo Sexo { get; set; }
    [Required]
    public int EstadoID { get; set; }
    
    [Required]
    public Estado Estado { get; set; }


}
