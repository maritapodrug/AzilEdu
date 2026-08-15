using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.DTOs;

public class AiTextRequestDto
{
    public string Purpose { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
}