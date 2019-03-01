using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Machine {
    public class State_Machine<TState, TTrigger> {
        TState current_State;
        IDictionary<TState, State_Representation<TState, TTrigger>> states =
            new Dictionary<TState, State_Representation<TState, TTrigger>>();

        public State_Representation<TState, TTrigger> Configure(TState state_name) {
            if (states.ContainsKey(state_name))
                return states[state_name];
            else
                return states[state_name] = new State_Representation<TState, TTrigger>();
        }


        public State_Machine(TState init_State) {
            current_State = init_State;
        }

        public void Fire(TTrigger trigger) {
            Transaction(trigger);
        }

        public void Fire<Targ>(TTrigger trigger, Targ data) {
            Transaction(trigger);
        }

        public void Transaction(TTrigger trigger) {
            states[current_State].Exit_State();
            var next_state = states[current_State].Get_Next_State(trigger);
            states[next_state].Enter_State();
            current_State = next_state;
        }
    }
}
