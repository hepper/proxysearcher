using System;
using System.Collections.Generic;
using System.Timers;

namespace ProxySearch.Console.Code.Extensions
{
    public static class ActionExtensions
    {
        private static Dictionary<Action, Timer> actionsDictionary = new Dictionary<Action, Timer>();

        public static void RunWithDelay(this Action action, TimeSpan delay)
        {
            lock (actionsDictionary)
            {
                if (actionsDictionary.ContainsKey(action))
                {
                    actionsDictionary[action].Stop();
                }
                else
                {
                    Timer timer = new Timer(delay.TotalMilliseconds);

                    timer.Elapsed += (sender, e) =>
                    {
                        lock (actionsDictionary)
                        {
                            actionsDictionary[action].Stop();
                            actionsDictionary.Remove(action);
                        }

                        action();
                    };

                    actionsDictionary.Add(action, timer);
                }

                actionsDictionary[action].Start();
            }
        }
    }
}
