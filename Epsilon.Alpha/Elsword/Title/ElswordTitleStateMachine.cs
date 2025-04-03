using Epsilon.Alpha.Automata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Elsword.Title
{
    internal class ElswordTitleStateMachine : IStateMachine<ElswordTitleState, ElswordTitleCommand>
    {
        private Dictionary<ElswordTitleStateTransition, ElswordTitleState> _transitions;

        public ElswordTitleStateMachine()
        {
            _transitions = new Dictionary<ElswordTitleStateTransition, ElswordTitleState>()
            {
                { new ElswordTitleStateTransition(ElswordTitleState.None, ElswordTitleCommand.TitleSwap), ElswordTitleState.TitleSwapping },
                { new ElswordTitleStateTransition(ElswordTitleState.NightParade, ElswordTitleCommand.TitleSwap), ElswordTitleState.TitleSwapping },
                { new ElswordTitleStateTransition(ElswordTitleState.FreedShadow, ElswordTitleCommand.TitleSwap), ElswordTitleState.TitleSwapping },
                { new ElswordTitleStateTransition(ElswordTitleState.TheSettingSun, ElswordTitleCommand.TitleSwap), ElswordTitleState.TitleSwapping },
                { new ElswordTitleStateTransition(ElswordTitleState.TitleSwapping, ElswordTitleCommand.SelectOtherTitle), ElswordTitleState.None },
                { new ElswordTitleStateTransition(ElswordTitleState.TitleSwapping, ElswordTitleCommand.SelectNightParade), ElswordTitleState.NightParade },
                { new ElswordTitleStateTransition(ElswordTitleState.TitleSwapping, ElswordTitleCommand.SelectFreedShadow), ElswordTitleState.FreedShadow },
                { new ElswordTitleStateTransition(ElswordTitleState.TitleSwapping, ElswordTitleCommand.SelectTheSettingSun), ElswordTitleState.TheSettingSun }
            };

            Reset();
        }

        public void Reset()
        {
            this.CurrentState = ElswordTitleState.None;
        }

        public ElswordTitleState GetNext(ElswordTitleCommand command)
        {
            ElswordTitleStateTransition st = new ElswordTitleStateTransition(this.CurrentState, command);

            if (_transitions.ContainsKey(st))
                return _transitions[st];

            return this.CurrentState;
        }

        public ElswordTitleState MoveNext(ElswordTitleCommand command)
        {
            this.CurrentState = GetNext(command);
            return this.CurrentState;
        }

        public ElswordTitleState CurrentState { get; private set; }
    }
}
