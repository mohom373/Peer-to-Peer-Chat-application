using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.Models
{
    /// <summary>
    /// Used to indicate an error involving validation
    /// </summary>
    public class ValidationError
    {
        public string ErrorMessage { get; set; } = "";

        public Visibility HasError { get; set; } = Visibility.Collapsed;
    }
}
