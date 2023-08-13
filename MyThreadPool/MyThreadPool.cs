using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThreadPool
{
    enum Priority
    {
        LOV,
        NORMAL,
        HIGH
    }

    internal class MyThreadPool
    {
        private readonly int _thrCount;// Кількість Thread
        private readonly System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<Action<Object>, Object>> _queue;//Колекція Thread
        private bool _stop;

        public MyThreadPool( int thrCount, Priority priority)
        {
            _stop=true;
            _thrCount = thrCount; //Кількість Thread
            _queue = new System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<Action<Object>, Object>>(); //Створюемо порожню колекцію Thread
            Start(priority);
        }

        private void Start (Priority priority) // Start Thread
        {
            for (int i = 0; i < _thrCount; i++) // Створення Thread
            {
                var p = new Thread(Worker);
                switch (priority) //Задання пріорітету
                {
                    case Priority.LOV:
                        p.Priority = ThreadPriority.Lowest;
                        break;
                    case Priority.NORMAL:
                        p.Priority = ThreadPriority.Normal;
                        break;
                    case Priority.HIGH:
                        p.Priority = ThreadPriority.Highest;
                        break;
                    default:
                        break;
                }

                p.Start();
            }
        }

        public void Stop()
        {
            _stop = false; 
        }

        private void Worker ()
        {
            while (_stop)
            {
                if (_queue.TryDequeue(out var kv))
                {
                    var action = kv.Key;
                    var value = kv.Value;
                    action.Invoke(value);
                }
            }
        }

        public void Run (Action<object>action, object value = null)
        {
            _queue.Enqueue(new KeyValuePair<Action<Object>, object>(action, value));
        }
    }
}
