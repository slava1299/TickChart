using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DynamicData;
using ScottPlot;
using ScottPlot.Avalonia;
using System;
using System.Collections.Generic;

namespace UTS.AvaloniaUI.ComponentTask1.Views
{
    public partial class MainWindow : Window
    {
        private int MaxVisibleTicks = 100; // минимальное количество точек на графике
        private DispatcherTimer timer;
        private int counter = 0;
        //Массивы для хранения данных X и Y
        private readonly List<DateTime> dataX = new List<DateTime>(); // Временные метки
        private readonly List<double> dataY = new List<double>();
        private Plot _currentPlot;

        //Генерация случайных чисел
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // Каждые 100 мс добавляем новую точку
            timer.Tick += Timer_Tick;
            timer.Start();
            numericUpDown.ValueChanged += NumericUpDown_ValueChanged;
            UpdateGraph();
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)   
        {       
         //Получаем новое значение от NumericUpDown
         MaxVisibleTicks = Convert.ToInt32(numericUpDown.Value);   
        }
        private void OnStartClicked(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void OnStopClicked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dataX.Count > MaxVisibleTicks)
            {
                while (dataX.Count > MaxVisibleTicks)
                {
                    dataX.RemoveAt(0);
                    dataY.RemoveAt(0);
                }
            }
            //Добавляем новую точку
            counter++;
            dataX.Add(DateTime.Now); // Добавляем текущую временную метку
            dataY.Add(random.Next(100, 115)); // Новая случайная точка

            UpdateGraph();
        }

        //Метод для обновления графика
        private void UpdateGraph()
        {
            if (!this.IsInitialized || !IsVisible) return;
            var plot = this.Find<AvaPlot>("AvaPlot1").Plot;
            _currentPlot = plot;
            plot.YLabel("Price");
            plot.Clear();
            plot.Add.SignalXY(dataX.ToArray(), dataY.ToArray());
            plot.Axes.DateTimeTicksBottom(); // отображение временных меток
            plot.Axes.AutoScale(); // автоматический масштаб осей
            this.Find<AvaPlot>("AvaPlot1").Refresh();
        }
        private void OnDarkTheme(object sender, RoutedEventArgs e)
        {
            _currentPlot.FigureBackground.Color = Color.FromHex("#181818");
            _currentPlot.DataBackground.Color = Color.FromHex("#1f1f1f");
            _currentPlot.Axes.Color(Color.FromHex("#d7d7d7"));
            _currentPlot.Grid.MajorLineColor = Color.FromHex("#404040");
            this.Find<AvaPlot>("AvaPlot1").Refresh();
        }
        private void OnLightTheme(object sender, RoutedEventArgs e)
        {
            _currentPlot.FigureBackground.Color = Color.FromHex("#FFFFFF");
            _currentPlot.DataBackground.Color = Color.FromHex("#F0F0F0");
            _currentPlot.Axes.Color(Color.FromHex("#000000"));
            _currentPlot.Grid.MajorLineColor = Color.FromHex("#DADADA");
            this.Find<AvaPlot>("AvaPlot1").Refresh();
        }
    }
}
