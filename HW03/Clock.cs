using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Content;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public event EventHandler? AlarmRaised;
        public event EventHandler? AlarmItemsChanged;

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
                if (AlarmItems.Contains(new TimeOnly(field.Hour, field.Minute, field.Second)))
                {
                    AlarmRaised?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public List<TimeOnly> AlarmItems { get; set; }

        public Clock()
        {
            AlarmItems = new List<TimeOnly>();
            ClockTime = new TimeOnly(23, 33, 40);

            _timer = new Timer((state) => Tick(), null, 0, 1000);
        }

        public void AddAlarm(TimeOnly alarm)
        {
            if (!AlarmItems.Contains(alarm))
            {
                AlarmItems.Add(alarm);
                AlarmItemsChanged?.Invoke(this, EventArgs.Empty);
            }
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

        public ObservableCollection<TimeOnly> AlarmItems { get; set; }

        [ObservableProperty]
        public partial TimeSpan SelectedAlarmTime { get; set; } = TimeSpan.Zero;

        public Clock DefaultClock => field;

        public TimeOnly DefaultClockTime => DefaultClock.ClockTime;

        public ClockViewModel()
        {
            DefaultClock = new Clock();
            DefaultClock.TimeChanged += OnTimeChanged;
            DefaultClock.AlarmRaised += OnAlarmRaised;
            DefaultClock.AlarmItemsChanged += OnAlarmItemsChanged;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            AlarmItems = new ObservableCollection<TimeOnly>();
            DefaultClock.AddAlarm(new TimeOnly(23, 34, 00));
        }

        [RelayCommand]
        private void Calibrate()
        {
            DefaultClock.Calibrate();
            OnPropertyChanged(nameof(DefaultClockTime));
        }

        [RelayCommand]
        private void AddAlarm()
        {
            DefaultClock.AddAlarm(TimeOnly.FromTimeSpan(SelectedAlarmTime));
        }

        // 在多线程环境中更新UI元素时，可能会遇到System.Runtime.InteropServices.COMException错误。
        // 这通常是因为UI元素只能在创建它们的线程（通常是主线程）上进行更新
        // 而Timer的回调方法是在一个线程池线程中执行的（非 UI 线程）
        // 使用 DispacherQueue 可以确保 OnPropertyChanged 在 UI 线程跟新
        private void OnTimeChanged(object? sender, EventArgs e)
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                // 通知 UI 更新
                OnPropertyChanged(nameof(DefaultClockTime));
            });
        }

        private void OnAlarmRaised(object? sender, EventArgs e)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Alarm",
                    Content = "起きて",
                    CloseButtonText = "OK"
                };


                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = ((Application.Current as App)?.m_window as MainWindow)?.Content.XamlRoot;

                var result = await dialog.ShowAsync();
            });
        }

        private void OnAlarmItemsChanged(object? sender, EventArgs e)
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                AlarmItems.Clear();
                foreach (var alarm in DefaultClock.AlarmItems)
                {
                    AlarmItems.Add(alarm);
                }
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
