using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GateSim;

namespace SimGUI_WPF
{
  public enum CompType { AND, OR, NOT, XOR, Clk, Compiled, Hierarchical };
  public enum ActionType { CreateAnd, CreateOr, CreateNot, CreateXor, Connect, Disconnect };

  public partial class MainWindow : Window
  {
    // simple flag for enabling "New thumb" mode
    private bool _isAddNew;

    // Paths for our predefined thumbs
    private Path _path1;
    private Path _path2;
    private Path _path3;
    private Path _path4;

    public MainWindow()
    {
      InitializeComponent();
    }

    // Event hanlder for dragging functionality support
    private void OnDragDelta(object sender, DragDeltaEventArgs e)
    {
      var thumb = e.Source as MyThumb;

      var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
      var top = Canvas.GetTop(thumb) + e.VerticalChange;

      Canvas.SetLeft(thumb, left);
      Canvas.SetTop(thumb, top);

      // Update lines's layouts
      UpdateLines(thumb);
    }

    private void createUiComponent(Component comp)
    {
      UiComp uiComp = new UiComp(comp);

      Ellipse conn1 = new Ellipse
      {
        Fill = new SolidColorBrush(Colors.Red),
        Width = 5,
        Height = 5,
        Opacity = 1,
        Margin = new Thickness(10, 10, 0, 0)
      };

      //Ellipse conn1 = new Ellipse();
      conn1.Height = 50;
      conn1.Width = 50;

      compCanvas.Children.Add(conn1);

      Canvas.SetLeft(conn1, 200);
      Canvas.SetTop(conn1, 200);
      //Panel.SetZIndex(conn1, 1);

      //conn1.UpdateLayout();

      for(int i = 0; i < uiComp.numInputs(); i++)
      {

      }

      for (int i = 0; i < uiComp.numOutputs(); i++)
      {

      }
    }

    // This method updates all the starting and ending lines assigned for the given thumb 
    // according to the latest known thumb position on the canvas
    private static void UpdateLines(MyThumb thumb)
    {
      var left = Canvas.GetLeft(thumb);
      var top = Canvas.GetTop(thumb);

      foreach(var ellipse in thumb.connEllipses)
        ellipse.Center = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);

      foreach (var line in thumb.StartLines)
        line.StartPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);

      foreach (var line in thumb.EndLines)
        line.EndPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
      createUiComponent(new ClockComp(1, 1));
      // Move all the predefined thumbs to the front to be over the lines
      Panel.SetZIndex(myThumb1, 1);
      Panel.SetZIndex(myThumb2, 1);
      Panel.SetZIndex(myThumb3, 1);
      Panel.SetZIndex(myThumb4, 1);

      #region Initialize paths for predefined thumbs

      _path1 = new Path { Stroke = Brushes.Black, StrokeThickness = 1 };
      _path2 = new Path { Stroke = Brushes.Blue, StrokeThickness = 1 };
      _path3 = new Path { Stroke = Brushes.Green, StrokeThickness = 1 };
      _path4 = new Path { Stroke = Brushes.Red, StrokeThickness = 1 };

      compCanvas.Children.Add(_path1);
      compCanvas.Children.Add(_path2);
      compCanvas.Children.Add(_path3);
      compCanvas.Children.Add(_path4);

      #endregion

      #region Initialize line geometry for predefined thumbs

      var line1 = new LineGeometry();
      _path1.Data = line1;

      var line2 = new LineGeometry();
      _path2.Data = line2;

      var line3 = new LineGeometry();
      _path3.Data = line3;

      var line4 = new LineGeometry();
      _path4.Data = line4;

      #endregion

      #region Setup connections for predefined thumbs

      myThumb1.StartLines.Add(line1);
      myThumb2.EndLines.Add(line1);

      myThumb2.StartLines.Add(line2);
      myThumb3.EndLines.Add(line2);

      myThumb3.StartLines.Add(line3);
      myThumb4.EndLines.Add(line3);

      myThumb4.StartLines.Add(line4);
      myThumb1.EndLines.Add(line4);

      #endregion

      #region Update lines' layouts

      UpdateLines(myThumb1);
      UpdateLines(myThumb2);
      UpdateLines(myThumb3);
      UpdateLines(myThumb4);

      #endregion

      PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDownHandler;
    }

    private void OpenButton_Click(object sender, RoutedEventArgs e)
    {

    }

    // Event handler for creating new thumb element by left mouse click
    // and visually connecting it to the myThumb2 element
    private void PreviewMouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
    {
      if (!_isAddNew) return;

      // Create new thumb object
      var newThumb = new MyThumb();
      // Assign our custom template to it
      newThumb.Template = Resources["template1"] as ControlTemplate;
      // Calling ApplyTemplate enables us to navigate the visual tree right now (important!)
      newThumb.ApplyTemplate();
      // Add the "onDragDelta" event handler that is common to all objects
      newThumb.DragDelta += OnDragDelta;
      // Put newly created thumb on the canvas
      compCanvas.Children.Add(newThumb);

      // Access the image element of our custom template and assign it to the new image
      var img = (Image)newThumb.Template.FindName("tplImage", newThumb);
      img.Source = new BitmapImage(new Uri("Images/gear_connection.png", UriKind.Relative));

      // Access the textblock element of template and change it too
      var txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
      txt.Text = "System action";

      // Set the position of the object according to the mouse pointer                
      var position = e.GetPosition(this);
      Canvas.SetLeft(newThumb, position.X);
      Canvas.SetTop(newThumb, position.Y);
      // Move our thumb to the front to be over the lines
      Panel.SetZIndex(newThumb, 1);
      // Manually update the layout of the thumb (important!)
      newThumb.UpdateLayout();

      // Create new path and put it on the canvas
      var newPath = new Path { Stroke = Brushes.Black, StrokeThickness = 1 };
      compCanvas.Children.Add(newPath);

      // Create new line geometry element and assign the path to it
      var newLine = new LineGeometry();
      newPath.Data = newLine;

      //newLine.MouseDown += new MouseButtonEventHandler(line_MouseDown);
      //newLine.MouseUp += new MouseButtonEventHandler(line_MouseUp);

      // Predefined "myThumb2" element will host the starting point
      myThumb2.StartLines.Add(newLine);
      // Our new thumb will host the ending point of the line
      newThumb.EndLines.Add(newLine);

      // Update the layout of line geometry
      UpdateLines(newThumb);
      UpdateLines(myThumb2);

      _isAddNew = false;
      Mouse.OverrideCursor = null;
      btnNewAction.IsEnabled = true;
      e.Handled = true;
    }

    /*
    void line_MouseUp(object sender, MouseButtonEventArgs e)
    {
      // Change line colour back to normal 
      ((Line)sender).Stroke = Brushes.Black;
    }

    void line_MouseDown(object sender, MouseButtonEventArgs e)
    {
      // Change line Colour to something
      ((Line)sender).Stroke = Brushes.Red;
    }
    */

    // Event handler for enabling new thumb creation by left mouse button click
    private void BtnNewActionClick(object sender, RoutedEventArgs e)
    {
      _isAddNew = true;
      Mouse.OverrideCursor = Cursors.SizeAll;
      btnNewAction.IsEnabled = false;
    }
  }
}