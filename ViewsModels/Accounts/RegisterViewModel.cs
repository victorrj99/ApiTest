using System.ComponentModel.DataAnnotations;

namespace ApiOwn.ViewsModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome Obrigatório")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    
    
}