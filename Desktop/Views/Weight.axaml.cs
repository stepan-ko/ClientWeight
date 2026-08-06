using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ClientCW.ViewModels;
using Desktop;

namespace ClientCW.Views;

public partial class Weight : UserControl
{
    public Weight()
    {
        InitializeComponent();

        btnCoimbo1.SelectionChanged += BtnCoimbo1_SelectionChanged;
        btnCoimbo2.SelectionChanged += BtnCoimbo2_SelectionChanged;

        Print();
        var app = (App)Application.Current;        
        DataContext = new WeightViewModel(app.MbService); // <-- передаём тот же сервис

    }
      
    private static void SetBindingComboValue(int index, TextBlock block)
    {
        switch (index)
        {
            case 0:
                block.Bind(TextBlock.TextProperty, new Binding("Weight.orderData.OrderBalance"));
                break;
            case 1:
                block.Bind(TextBlock.TextProperty, new Binding("Weight.orderData.TotalWeight"));
                break;
            case 2:
                block.Bind(TextBlock.TextProperty, new Binding("Weight.statusData.mRunningTotalWeight"));
                break;

            default:
                break;
        }
    }

    private void BtnCoimbo1_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        SetBindingComboValue(btnCoimbo1.SelectedIndex, comboValue1);
    }
    private void BtnCoimbo2_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        SetBindingComboValue(btnCoimbo2.SelectedIndex, comboValue2);
    }



    private void Print()
    {
        // начальные координаты
        int x = 100;
        int y = 80;

        double scale = 1.35;

        //x = (int)(x * scale);
        //y = (int)(y * scale);

        PrintMisc(x, y);

        MyCanvas.Children.Add(UpGarner(x, y));
        MyCanvas.Children.Add(LowGarner(x, y));
        MyCanvas.Children.Add(MidGarner(x, y));

        Polygon UGFullSensor = FullSensor(x + 13, y - 7);
        UGFullSensor.Bind(Polygon.FillProperty, new Binding("Weight.statusData.ColorUGFullSensor"));

        

        MyCanvas.Children.Add(UGFullSensor);
        MyCanvas.Children.Add(FullSensor(x + 13, y + 123));
        MyCanvas.Children.Add(FullSensor(x + 14, y + 310, true));

        AddGates(x, y);

        MyCanvas.RenderTransform = new ScaleTransform(scale, scale);
    }
    
    private void AddGates(int x, int y)
    {
        MyCanvas.Children.Add(Gate(x + 25, y + 100));
        MyCanvas.Children.Add(Gate(x + 70, y + 100));

        MyCanvas.Children.Add(Gate(x + 25, y + 210));
        MyCanvas.Children.Add(Gate(x + 70, y + 210));

        MyCanvas.Children.Add(GateLG(x + 44, y + 334));

        // Если есть нижняя задвижка то рисуем
        AddGatesControl(x, y);
    }
    
    private void AddGatesControl(int x, int y)
    {
        int percentLG = 85;
        MyCanvas.Children.Add(GatePercent(percentLG, x + 44, y + 334));
        MyCanvas.Children.Add(TextPcnt(percentLG, x + 80, y + 334));

        MyCanvas.Children.Add(BtnControlPlus(x + 36, y + 347));
        MyCanvas.Children.Add(BtnControlMinus(x + 64, y + 347));
    }
    
    private void PrintMisc(int x, int y)
    {
        MyCanvas.Children.Add(VerticalPipe(x - 1, y));
        MyCanvas.Children.Add(VerticalPipe(x + 128, y));
        MyCanvas.Children.Add(HorizonPipe(x - 8, y - 8));

        PrintChain(x + 5, y + 90);
        PrintChain(x + 111, y + 90);
    }

    private void PrintChain(int x=0, int y=0)
    {
        x += 1;
        MyCanvas.Children.Add(VertRectangle(x, y));        
        MyCanvas.Children.Add(VertRectangle(x, y + 15));
        MyCanvas.Children.Add(VertRectangle(x, y + 22));
        MyCanvas.Children.Add(VertRectangle(x, y + 29));
        MyCanvas.Children.Add(VertRectangle(x, y + 36));
        MyCanvas.Children.Add(ChainCircle(x - 2, y + 18));
        MyCanvas.Children.Add(ChainCircle(x - 2, y + 25));
        MyCanvas.Children.Add(ChainCircle(x - 2, y + 32));

        MyCanvas.Children.Add(Sensor(x - 2, y + 5));
    }

    private static Polygon Gate(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x, y),
            new Point(x + 25, y),
            new Point(x + 25, y + 10),
            new Point(x, y + 10)
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = new SolidColorBrush(Color.FromRgb(34, 255, 0)),
            StrokeThickness = 1,
            Stroke = Brushes.Black,
        };
        
        return polygon;
    }

    private static TextBlock TextPcnt(int pcnt, int x = 0, int y = 0)
    {

        var txtPcnt = new TextBlock{
            FontSize = 10,
            Foreground = Brushes.Black,
            Text = pcnt.ToString() + "%"
        };
        Canvas.SetLeft(txtPcnt, x);
        Canvas.SetTop(txtPcnt, y);
        return txtPcnt;
    }

    private static Button BtnControlPlus(int x = 0, int y = 0)
    {

        var btn = new Button
        {
            FontSize = 20,
            Content = "+",
            Padding = Thickness.Parse("2, -5, 0, 0"),
            Width = 20,
            Height = 20,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,

        };
        
        Canvas.SetLeft(btn, x);
        Canvas.SetTop(btn, y);
        return btn;
    }

    private static Button BtnControlMinus(int x = 0, int y = 0)
    {

        var btn = new Button
        {
            FontSize = 20,
            Content = "-",
            Padding = Thickness.Parse("4, -4.5, 0, 0"),
            Width = 20,
            Height = 20,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,

        };

        Canvas.SetLeft(btn, x);
        Canvas.SetTop(btn, y);
        return btn;
    }

    private static Rectangle GatePercent(int pcnt, int x = 0, int y = 0)
    {

        var rect = GateLG(x, y);
        rect.Width = (100 - pcnt) * rect.Width / 100;
        rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        return rect;
    }

    private static Rectangle GateLG(int x = 0, int y = 0)
    {

        var rect = new Rectangle
        {
            Width = 32,
            Height = 12,
            Fill = new SolidColorBrush(Color.FromRgb(34, 255, 0)),
            StrokeThickness = 1, 
            Stroke =  Brushes.Black,
            
        };
        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        return rect;
    }

    private static Rectangle VertRectangle(int x = 0, int y = 0)
    {
        var rectangle = new Rectangle
        {
            Width = 2,
            Height = 5,
            Fill = Brushes.Black
        };
        Canvas.SetLeft(rectangle, x);
        Canvas.SetTop(rectangle, y);
        return rectangle;
    }

    private static Ellipse ChainCircle(int x = 0, int y = 0)
    {
        Ellipse circle = new Ellipse
        {
            Stroke = Brushes.Black,          // Черная граница
            StrokeThickness = 1,             // Толщина линии 1 пиксель
            Width = 6,                       // Диаметр по ширине
            Height = 6                        // Диаметр по высоте
        };

        Canvas.SetLeft(circle, x);
        Canvas.SetTop(circle, y);
        return circle;
    }

    private static Polygon VerticalPipe(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x - 8, y - 8),
            new Point(x + 1, y - 8),
            new Point(x + 1, y + 352),
            new Point(x - 8, y + 352)
        };

        // Создаем градиент
        LinearGradientBrush gradient = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative), // Начало градиента слева посередине
            EndPoint = new RelativePoint(1, 0.5, RelativeUnit.Relative),   // Конец градиента справа посередине
            GradientStops =
            {
                new GradientStop(Color.FromRgb(224, 224, 224), 0),     // Красный слева
                new GradientStop(Colors.White, 0.5),              // Белый в центре
                new GradientStop(Color.FromRgb(224, 224, 224), 1)     // Красный справа
            }
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = gradient,
            StrokeThickness = 1,           // Толщина контура
            Stroke = Brushes.Black        // Цвет контура
        };

        return polygon;
    }

    private static Polygon FullSensor(int x = 0, int y = 0, bool rotate = false)
    {
        var points = new List<Point>
        {
            new Point(x, y),
            new Point(x + 12, y),
            new Point(x + 12, y + 4),
            new Point(x + 8, y + 6),
            new Point(x + 8, y + 15),
            new Point(x + 4, y + 15),
            new Point(x + 4, y + 6),
            new Point(x, y + 4)
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
            StrokeThickness = 1,
            Stroke = Brushes.Black,
           
        };

        if (rotate) polygon.RenderTransform = new RotateTransform(215, x / 2, y / 2);

        return polygon;
    }
       
    private static Polygon Sensor(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x, y),
            new Point(x + 6, y),
            new Point(x + 6, y + 2),
            new Point(x + 2, y + 2),
            new Point(x + 2, y + 4),
            new Point(x + 6, y + 4),
            new Point(x + 6, y + 10),
            new Point(x, y + 10),
            new Point(x, y + 8),
            new Point(x + 4, y + 8),
            new Point(x + 4, y + 6),
            new Point(x, y + 6)
        };
               
        var polygon = new Polygon
        {
            Points = points,
            Fill = Brushes.Blue,           
        };

        return polygon;
    }

    private static Polygon HorizonPipe(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x + 8, y + 89),
            new Point(x + 128, y + 89),
            new Point(x + 128, y + 98),
            new Point(x + 8, y + 98)
        };

        // Создаем градиент
        LinearGradientBrush gradient = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative), // Начало градиента слева посередине
            EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),   // Конец градиента справа посередине
            GradientStops =
            {
                new GradientStop(Color.FromRgb(224, 224, 224), 0),    
                new GradientStop(Colors.White, 0.5),              // Белый в центре
                new GradientStop(Color.FromRgb(224, 224, 224), 1)     
            }
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = gradient,
            StrokeThickness = 1,           // Толщина контура
            Stroke = Brushes.Black        // Цвет контура
        };

        return polygon;
    }

    private static Polygon UpGarner(int x=0, int y=0)
    {
        var points = new List<Point>
        {
            new Point(x + 0, y + 0),
            new Point(x + 120, y + 0),
            new Point(x + 120, y + 70),
            new Point(x + 95, y + 100),
            new Point(x + 95, y + 110),
            new Point(x + 70, y + 110),
            new Point(x + 70, y + 100),
            new Point(x + 60, y + 90),
            new Point(x + 50, y + 100),
            new Point(x + 50, y + 110),
            new Point(x + 25, y + 110),
            new Point(x + 25, y + 100),
            new Point(x + 0, y + 70)
        };

        // Создаем градиент
        LinearGradientBrush gradient = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative), // Начало градиента слева посередине
            EndPoint = new RelativePoint(1, 0.5, RelativeUnit.Relative),   // Конец градиента справа посередине
            GradientStops =
            {
                new GradientStop(Color.FromRgb(224, 224, 224), 0),     // Красный слева
                new GradientStop(Colors.White, 0.5),              // Белый в центре
                new GradientStop(Color.FromRgb(224, 224, 224), 1)     // Красный справа
            }
        };
                
        var polygon = new Polygon
        {
            Points = points,
            Fill = gradient,
            StrokeThickness = 1,           // Толщина контура
            Stroke = Brushes.Black        // Цвет контура
        };
               
        return polygon;
    }
      
    private static Polygon LowGarner(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x + 0, y + 234),
            new Point(x + 120, y + 234),
            new Point(x + 120, y + 304),
            new Point(x + 75, y + 334),
            new Point(x + 75, y + 344),
            new Point(x + 45, y + 344),
            new Point(x + 45, y + 334),
            new Point(x + 0, y + 304)
        };

        // Создаем градиент
        LinearGradientBrush gradient = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative), // Начало градиента слева посередине
            EndPoint = new RelativePoint(1, 0.5, RelativeUnit.Relative),   // Конец градиента справа посередине
            GradientStops =
            {
                new GradientStop(Color.FromRgb(224, 224, 224), 0),     
                new GradientStop(Colors.White, 0.5),                // Белый в центре
                new GradientStop(Color.FromRgb(224, 224, 224), 1)    
            }
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = gradient,
            StrokeThickness = 1,           // Толщина контура
            Stroke = Brushes.Black        // Цвет контура
        };
                
        return polygon;
    }
    
    private static Polygon MidGarner(int x = 0, int y = 0)
    {
        var points = new List<Point>
        {
            new Point(x + 5, y + 130),
            new Point(x + 115, y + 130),
            new Point(x + 115, y + 189),
            new Point(x + 95, y + 209),
            new Point(x + 95, y + 219),
            new Point(x + 70, y + 219),
            new Point(x + 70, y + 209),
            new Point(x + 60, y + 199),
            new Point(x + 50, y + 209),
            new Point(x + 50, y + 219),
            new Point(x + 25, y + 219),
            new Point(x + 25, y + 209),
            new Point(x + 5, y + 189)
        };

        // Создаем градиент
        LinearGradientBrush gradient = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative), // Начало градиента слева посередине
            EndPoint = new RelativePoint(1, 0.5, RelativeUnit.Relative),   // Конец градиента справа посередине
            GradientStops =
            {
                new GradientStop(Color.FromRgb(224, 224, 224), 0),     // Красный слева
                new GradientStop(Colors.White, 0.5),              // Белый в центре
                new GradientStop(Color.FromRgb(224, 224, 224), 1)     // Красный справа
            }
        };

        var polygon = new Polygon
        {
            Points = points,
            Fill = gradient,
            StrokeThickness = 1,           // Толщина контура
            Stroke = Brushes.Black        // Цвет контура
        };

        return polygon;
    }


    //polygon.Bind(Polygon.FillProperty, new Binding("Weight.testColor"));

}