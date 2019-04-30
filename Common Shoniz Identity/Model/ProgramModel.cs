using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.ShonizIdentity.Model
{
    public class ProgramModel
    {
        public int ProgramId { get; set; }
        public string FarsiName { get; set; }
        public string EnglishName { get; set; }
        public int PartTypeId { get; set; }
    }
}
