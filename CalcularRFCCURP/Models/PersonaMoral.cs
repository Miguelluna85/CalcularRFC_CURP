using System.ComponentModel.DataAnnotations;

namespace CalcularCURPyRFC.Models;

public class PersonaMoral
{
    public int Id { get; set; }


    [Required(ErrorMessage = "Dato Obligatorio"), MinLength(3, ErrorMessage = "3 Caracteres como minimo"), StringLength(50, ErrorMessage = "Tamaño max. 50 caracteres")]
    public string RazonSocial { get; set; } = string.Empty;


    [Required(ErrorMessage = "Dato obligatorio")]
    [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime FechaConstitucion { get; set; }

}
