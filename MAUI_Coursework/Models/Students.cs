using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Coursework.Models
{
    public class Students
    {
        [PrimaryKey]
        public int ID_user { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Group { get; set; }
        public byte[] Photo { get; set; }
    }
}
