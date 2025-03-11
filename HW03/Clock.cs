using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Content;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW03
{
    public class Clock
    {
        private Timer _timer;

        public event EventHandler? TimeChanged;

        public TimeOnly ClockTime
        {
            get => field;
            set 
            {
                if (field != value)
                {
                    field = value;
                    TimeChanged?.Invoke(this, EventArgs.Empty);
                }
            } 
        }

        public List<TimeOnly> AlarmTimes { get; set; }

        public Clock()
        {
            ClockTime = new TimeOnly(23, 33, 33);
            AlarmTimes = new List<TimeOnly>();

            _timer = new Timer((state)=>Tick(), null, 0, 1000);
        }

        public void Calibrate()
        {
            ClockTime = TimeOnly.FromDateTime(DateTime.Now);
        }

        public void Start()
        {
            _timer.Change(0, 1000);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Tick()
        {
            ClockTime = ClockTime.Add(TimeSpan.FromSeconds(1));
        }
    }

    public partial class ClockViewModel : ObservableObject
    {
        private readonly DispatcherQueue _dispatcherQueue;

        public Clock DefaultClock => field;

        public TimeOnly DefaultClockTime => DefaultClock.ClockTime;

        public ClockViewModel()
        {
            DefaultClock = new Clock();
            DefaultClock.TimeChanged += OnTimeChanged;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        [RelayCommand]
        private void Calibrate()
        {
            DefaultClock.Calibrate();
            OnPropertyChanged(nameof(DefaultClockTime));
        }

        // 在多线程环境中更新UI元素时，可能会遇到System.Runtime.InteropServices.COMException错误。
        // 这通常是因为UI元素只能在创建它们的线程（通常是主线程）上进行更新
        // 而Timer的回调方法是在一个线程池线程中执行的（非 UI 线程）
        // 使用 DispacherQueue 可以确保 OnPropertyChanged 在 UI 线程跟新
        private void OnTimeChanged(object? sender, EventArgs e)
        {
            _dispatcherQueue.TryEnqueue(()=>
            {
                OnPropertyChanged(nameof(DefaultClockTime));
            });
        }
    }

    public class ClockFormatter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeOnly time)
            {
                string format = parameter as string ?? "";
                return format + time.ToString("HH:mm:ss");
            }
            return "Invalid Time";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
