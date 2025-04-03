using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Automata
{
    internal interface IStateMachine<State, Command>
    {
        void Reset();

        State GetNext(Command command);

        State MoveNext(Command command);

        State CurrentState { get; }
    }
}
