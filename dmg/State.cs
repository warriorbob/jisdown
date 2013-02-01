using dmg.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmg
{
    public class State
    {
        public Dude Dude { get; set; }
        public List<Baddie> Baddies { get; set; }
        //public Map Map { get; set; }
    }
}
