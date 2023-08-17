using System;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Threading;
namespace Reserva_Comedor
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer countdownTimer;
        private DateTime targetTime;
        public MainWindow()
        {
            InitializeComponent();

            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            // Establece la fecha objetivo para la cuenta regresiva (1 minuto en este caso)
            targetTime = DateTime.Now.AddMinutes(1);
            countdownTimer.Start();
        }
        //RELOJ CUENTA REGRESIVA
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = targetTime - DateTime.Now;
            if (remainingTime.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                TextBlockCuentaRegresiva.Text = "Tiempo agotado";
                // Deshabilitar los TextBox cuando el tiempo se agote
                TextBoxUsuario.IsEnabled = false;
                TextBoxClave.IsEnabled = false;
            }
            else
            {
                TextBlockCuentaRegresiva.Text = $"Reserva en: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }
        }
        // FOCUS-------------------------------------
        //CAJA USUARIO
        public void TextBoxUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxUsuario.Text == "Usuario")
            {
                TextBoxUsuario.Text = string.Empty;
            }
        }
        public void TextBoxUsuario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUsuario.Text))
            {
                TextBoxUsuario.Text = "Usuario";
            }
        }
        //CAJA CLAVE
        private void TextBoxClave_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxClave.Text == "Clave Semestre 2023 - II")
            {
                TextBoxClave.Text = string.Empty;
            }
        }
        public void TextBoxClave_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUsuario.Text))
            {
                TextBoxUsuario.Text = "Clave Semestre 2023 - II";
            }
        }
        //VERIFICAR ALUMNOS APTOS ALMACENADOS
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usuarioIngresado = TextBoxUsuario.Text;
            string claveIngresada = TextBoxClave.Text;
            //RECURRIR A LA BASE DE DATOS
            string connectionString = "Data Source=DESKTOP-N8QQUGE; Initial Catalog=ComedorUniversitario; Integrated Security=True";
            //USAR
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //CONSULTA
                string query = "SELECT Codigo FROM Alumnos WHERE Codigo = @Codigo AND AP = @AP";
                //EJECUTAR CONSULTA
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Codigo", usuarioIngresado);
                    command.Parameters.AddWithValue("@AP", claveIngresada);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //VEREFICAR LAS FILAS
                        if ((bool)reader.HasRows)
                        {
                            MessageBox.Show("FELICITACIONES USTED HA RESERVADO SU CUPO EN EL COMEDOR");
                            // Deshabilitar los TextBox cuando se reserva cupo
                            TextBoxUsuario.IsEnabled = false;
                            TextBoxClave.IsEnabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Usuario o clave incorrectos.");
                        }
                    }
                }
            }
        }
    }
}
