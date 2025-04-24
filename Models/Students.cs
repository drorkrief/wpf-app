using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_app.Models
{

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TagNumber { get; set; }
        public string Progress { get; set; }
        public string Points { get; set; }
        public string Class { get; set; }
        public Address Address { get; set; }
    }


}
