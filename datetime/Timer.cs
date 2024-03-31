using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace datetime
{
    public class Timer
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        Thread TimerThread;

        public System.Threading.ThreadState State
        {
            get
            {
                return TimerThread.ThreadState;
            }
        }

        public TimeSpan Time { get; set; } = new TimeSpan(0);

        public TimeSpan Interval;

        public System.Windows.Forms.Label OutputLabel;

        public delegate void TickHandler(TimeSpan time, System.Windows.Forms.Label ol);

        public event TickHandler Handler;

        public long Ticks
        {
            get
            {
                return Time.Ticks / Interval.Ticks;
            }
        }

        public bool isStop
        {
            get { return (TimerThread.ThreadState == System.Threading.ThreadState.Suspended) || (TimerThread.ThreadState == System.Threading.ThreadState.Unstarted); }
        }

        public Timer()
        {
            Interval = new TimeSpan(0, 0, 1);
            TimerThread = new Thread(t => DoTimer(false));
        }

        public Timer(TimeSpan interval)
        {
            Interval = interval;
            TimerThread = new Thread(t => DoTimer(false));
        }

        public Timer(TimeSpan interval, System.Windows.Forms.Label outputLabel)
        {
            Interval = interval;
            TimerThread = new Thread(t => DoTimer(false));
            OutputLabel = outputLabel;
        }

        public Timer(TimeSpan interval, System.Windows.Forms.Label outputLabel, TimeSpan st)
        {
            Interval = interval;
            TimerThread = new Thread(t => DoTimer(false));
            OutputLabel = outputLabel;
            Time = st;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        ~Timer()
        {
            AbortThread();
        }

        void Tick()
        {
            Time = Time + Interval;

            // Вывод времени
            Handler.Invoke(Time, OutputLabel);
        }

        void DoTimer(bool stop)
        {
            Stopwatch sw = Stopwatch.StartNew();

            while (!stop)
            {
                sw.Restart();

                Tick();

                TimeSpan wait;

                sw.Stop();

                if ((wait = Interval - sw.Elapsed).Ticks >= 0)
                    Thread.Sleep(wait);
            }
        }

        public void Start()
        {
            //AllocConsole();
            if (TimerThread.ThreadState == System.Threading.ThreadState.Unstarted)
                TimerThread.Start();
            else
                TimerThread.Resume();
        }

        public void Pause()
        {
            TimerThread.Suspend();
        }

        public TimeSpan Stop()
        {
            Time = new TimeSpan(0);

            TimerThread.Suspend();

            TimeSpan time = Time;

            return time;
        }

        public void AbortThread()
        {
            try
            {
                TimerThread.Start();
            }
            catch { }
            try
            {
                TimerThread.Resume();
            }
            catch { }

            TimerThread = new Thread(t => DoTimer(true));

            try
            {
                TimerThread.Join();
            }
            catch { }

            TimerThread.Abort();
        }
    }
}
