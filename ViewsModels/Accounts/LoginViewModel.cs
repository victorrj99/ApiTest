using System.ComponentModel.DataAnnotations;

namespace ApiOwn.ViewsModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Nome Obrigatório")]
    [EmailAddress(ErrorMessage = "Senha inválido")]
    public string Email { get; set; }
    
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Informe a senha")]
    public string Password { get; set; }
}