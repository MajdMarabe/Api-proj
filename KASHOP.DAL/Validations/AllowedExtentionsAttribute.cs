using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Validations
{
    public  class AllowedExtentionsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string[] _extentions = { ".jpg", ".webp" };
            if(value is IFormFile file)
            {
                //test.PNG
                //test.png
                var extention= Path.GetExtension(file.FileName).ToLower();
                if (!_extentions.Contains(extention)) {

                    return new ValidationResult($"Allowed Extensions: {string.Join(",", _extentions)}");
                
                }
            }
            return ValidationResult.Success;

        }
    }
}
