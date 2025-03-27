﻿using System.ComponentModel.DataAnnotations;

namespace ApiOwn.ViewsModels;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Imagem inválida")]
    public string Base64Image { get; set; }
}