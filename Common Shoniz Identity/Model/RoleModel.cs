using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.ShonizIdentity.Model
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int ProgramId { get; set; }
    }
}
