using State_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Start_State_machine {

    class Program {
        private static void OnSetVolume(int volume) {
            Console.WriteLine("Volume set to " + volume + "!");
        }

        private static void OnUnmute() {
            Console.WriteLine("Microphone unmuted!");
        }

        private static void OnMute() {
            Console.WriteLine("Microphone muted!");
        }

        static void StartCallTimer() {
            Console.WriteLine("[Timer:] Call started at {0}", DateTime.Now);
        }

        static void StopCallTimer() {
            Console.WriteLine("[Timer:] Call ended at {0}", DateTime.Now);
        }

        static void Fire(State_Machine<State, Trigger> phoneCall, Trigger trigger) {
            Console.WriteLine("[Firing:] {0}", trigger);
            phoneCall.Fire(trigger);
        }

        static void SetVolume(State_Machine<State, Trigger> phoneCall, Trigger trigger, int volume) {
            phoneCall.Fire(trigger, volume);
        }

        static void Print(State_Machine<State, Trigger> phoneCall) {
            Console.WriteLine("[Status:] {0}", phoneCall);
        }

        enum Trigger {
            CallDialed,
            CallConnected,
            LeftMessage,
            PlacedOnHold,
            TakenOffHold,
            PhoneHurledAgainstWall,
            MuteMicrophone,
            UnmuteMicrophone,
            SetVolume,
            close
        }

        enum State {
            OffHook,
            Ringing,
            Connected,
            OnHold,
            PhoneDestroyed
        }
        static void Main(string[] args) {
            var phoneCall = new State_Machine<State, Trigger>(State.OffHook);
            phoneCall.Configure(State.OffHook).Go_To_State(Trigger.CallDialed, State.Ringing);
            phoneCall.Configure(State.Ringing).Go_To_State(Trigger.CallConnected, State.Connected)
                .On_Entry(() => Console.WriteLine("start ringing"))
                .On_Exit(() => Console.WriteLine("stop ringing"));
            phoneCall.Configure(State.Connected).
                On_Entry(() => StartCallTimer()).
                On_Exit(() => StopCallTimer());

            phoneCall.Fire(Trigger.CallDialed);
            phoneCall.Fire(Trigger.CallConnected);
            Thread.Sleep(2000);
            phoneCall.Fire(Trigger.close);
            Console.ReadKey();
        }
    }
}
