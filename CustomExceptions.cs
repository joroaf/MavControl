using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavControl
{
    class JoystickException : Exception
    {
        private string p;

        public JoystickException(string p) : base(p)
        {
            this.p = p;
        }

    }
}
