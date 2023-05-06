using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework.Models
{
    public class Lessons
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ID_teacher { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Weekday { get; set; }
        public TimeSpan TimeL { get; set; }
    }
}
