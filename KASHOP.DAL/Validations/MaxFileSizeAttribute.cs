using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int  _maxFileSizeInMB;
        public MaxFileSizeAttribute(int size) { 

        _maxFileSizeInMB = size;        
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var sizeInMB = file.Length / (1024*1024);
                if (sizeInMB > _maxFileSizeInMB) {
                    return new ValidationResult($"Max file size is : {_maxFileSizeInMB}");
                }
            }
            return ValidationResult.Success;

        }
    }
}
