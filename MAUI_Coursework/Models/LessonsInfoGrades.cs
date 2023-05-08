using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework.Models
{
    public class LessonsInfoGrades
    {
        public string Name { get; set; }
        public DateTime Date_lesson { get; set; }
        public string Est { get; set; } //Оценка
        public string Att { get; set; } //Посещаемость
    }
}
