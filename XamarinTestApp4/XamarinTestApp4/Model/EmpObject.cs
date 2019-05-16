using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTestApp4.Model
{
    public class EmpObject
    {
        public int empNo { get; set; }
        public String eName { get; set; }
        public int sal { get; set; }
        public String job { get; set; }
    }

    public class EmpList
    {
        public List<EmpObject> empList { get; set; }
    }
}
