using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Automata
{
    internal interface IStateTransition<State, CommandT>
    {
        State CurrentState { get; }

        CommandT Command { get; }
    }
}
