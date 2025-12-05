namespace RestaurantOrderSystem;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new System.Windows.Application();
        app.Run(new View.MainWindow());
    }
}