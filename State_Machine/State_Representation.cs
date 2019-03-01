using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Machine {
    public class State_Representation<TState, TTrigger> {
        private IDictionary<TTrigger, TState> transaction_to =
                new Dictionary<TTrigger, TState>();

        private SortedList<int, Action> call_On_Entry = new SortedList<int, Action>();
        private SortedList<int, Action> call_On_Exit = new SortedList<int, Action>();

        public State_Representation() {
        }

        public State_Representation<TState, TTrigger> On_Entry(Action entryAction, int order = 1) {
            if (!call_On_Entry.ContainsKey(order))
                call_On_Entry[order] = new Action(entryAction);
            else
                call_On_Entry[order] += entryAction;
            return this;
        }

        public State_Representation<TState, TTrigger> On_Exit(Action entryAction, int order = 1) {
             if (!call_On_Exit.ContainsKey(order))
                call_On_Exit[order] = new Action(entryAction);
            else
                call_On_Exit[order] += entryAction;
            return this;
        }

        public State_Representation<TState, TTrigger> Go_To_State(TTrigger trigger, TState state) {
            if (transaction_to.ContainsKey(trigger)) {
                throw new Exception($"Only one trigger can exist from trigger $(nameof(trigger)) to $(nameof(state)) state");
            }
            transaction_to[trigger] = state;
            return this;
        }

        public void Exit_State() {

            foreach(Action exit_Func in call_On_Exit.Values) {
                exit_Func.Invoke();
            }
        }

        public void Enter_State() {
            foreach(Action entry_Func in call_On_Entry.Values) {
                entry_Func.Invoke();
            }
        }

        public TState Get_Next_State(TTrigger trigger) {
            if (transaction_to.ContainsKey(trigger))
                return transaction_to[trigger];
            else
                throw new Exception($"can trigger ${nameof(trigger)}");
        }
    }

}
