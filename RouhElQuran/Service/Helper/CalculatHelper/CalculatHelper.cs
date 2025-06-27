using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper.CalculatHelper
{
    public static class CalculatHelper 
    {
        public static string calculatYearsOfExperience(DateOnly workTo , DateOnly workFrom)
        {
            int totalDays = workTo.DayNumber - workFrom.DayNumber;
            int years = totalDays/365;
            int months = totalDays/30;
            return $"{years} year(s) : {months} month(s)";
        }
    }
}
