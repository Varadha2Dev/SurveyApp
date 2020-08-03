﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCreationAPI.Model
{
    public class ValidationResult
    {
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}