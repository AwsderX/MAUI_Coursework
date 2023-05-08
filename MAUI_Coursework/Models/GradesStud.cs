using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework.Models
{
    public class GradesStud
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Est { get; set; } //Оценка
        public string Att { get; set; } //Посещаемость
        public int ID_user { get; set; }
        public int ID { get; set; }
    }
}
