using Avalonia.Controls;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.AxisPanels;
using SeverstalTestTask.Interfaces;
using SeverstalTestTask.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeverstalTestTask.Views
{
    public partial class GraphWindow : Window
    {
        public GraphWindow()
        {
            InitializeComponent();
        }

        public GraphWindow(ObservableCollection<MachineRecord> machineRecords)
        {
            InitializeComponent();
            CreateGraph(machineRecords);
        }

        public override void Show()
        {
            base.Show();
            LogManager.Instance.AddEvent($"Graph is openned");
        }

        private void CreateGraph(ObservableCollection<MachineRecord> machineRecords)
        {
            var avaPlot = this.FindControl<AvaPlot>("PlotView");

            if (avaPlot != null)
            {
                var groupedRecords = machineRecords
                    .GroupBy(record => record.MachineNumber)
                    .ToList();

                foreach (var group in groupedRecords)
                {
                    // Line
                    List<double> xs = group.Select(item => item.GrossDate.ToOADate()).ToList();
                    List<double> ys = group.Select(item => (double)item.GrossWeight).ToList();

                    var scatter = avaPlot.Plot.Add.Scatter(xs.ToArray(), ys.ToArray());
                    scatter.LineWidth = 2;
                    scatter.MarkerSize = 10;

                    LogManager.Instance.AddEvent($"Added graph line with machine №{group.Key}");

                    int argb = (255 << 24)
                        | ((byte)(new Random().Next(256)) << 16)
                        | ((byte)(new Random().Next(256)) << 8)
                        | (byte)(new Random().Next(256));

                    scatter.Color = ScottPlot.Color.FromARGB(argb);

                    // Label under line
                    var lastPoint = new ScottPlot.Coordinates(xs.Last(), ys.Last());
                    var txt = avaPlot.Plot.Add.Text(
                        text: $"Машина №{group.Key}",
                        x: lastPoint.X,
                        y: lastPoint.Y
                    );
                    txt.LabelFontColor = ScottPlot.Color.FromARGB(argb);
                    txt.Alignment = ScottPlot.Alignment.MiddleLeft; 
                    txt.OffsetY = -15;
                    txt.LabelOffsetX = -40;
                }

                // General Settings
                avaPlot.Plot.Axes.DateTimeTicksBottom();
                avaPlot.Plot.Title("Динамика веса брутто по машинам");
                avaPlot.Plot.XLabel("Дата");
                avaPlot.Plot.YLabel("Вес брутто(кг)");
            }
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            LogManager.Instance.AddEvent("Graph is closed");
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}