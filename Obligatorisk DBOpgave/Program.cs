namespace Obligatorisk_DBOpgave
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            DBClient dbc = new DBClient();
            dbc.Start();
        }
    }
}
