using Epsilon.Alpha.Automata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Title
{
    internal class ElswordTitleStateTransition : IStateTransition<ElswordTitleState, ElswordTitleCommand>
    {
        public ElswordTitleStateTransition(ElswordTitleState state, ElswordTitleCommand command)
        {
            this.CurrentState = state;
            this.Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * this.CurrentState.GetHashCode() + 31 * this.Command.GetHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ElswordTitleStateTransition st && st.CurrentState == this.CurrentState && st.Command == this.Command;
        }

        public ElswordTitleState CurrentState { get; }

        public ElswordTitleCommand Command { get; }
    }
}
