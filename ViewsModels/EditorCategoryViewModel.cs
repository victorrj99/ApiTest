using System.ComponentModel.DataAnnotations;

namespace ApiOwn.ViewsModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "Nome deve ser obrigatório")]
    [StringLength(maximumLength: 40, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 40 caracteres")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Slug deve ser obrigatório")]
    public string Slug { get; set; }
    
    
}