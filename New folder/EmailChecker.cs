using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace mainForm
{
   public class EmailChecker
    {
        public static bool checkForEmail(String email) {
        bool IsValid = false;
       Regex r = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
       if(r.IsMatch(email))
           IsValid = true; 
        return IsValid;
           
        }
    }

}
