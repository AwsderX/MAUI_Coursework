using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework.Models
{
    public class Grades
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ID_student { get; set; }
        public int ID_lesson { get; set; }
        public DateTime Date_lesson { get; set; }
        public int Est { get; set; }
    }
}
