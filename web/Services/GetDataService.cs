using System.Data.SqlClient;
using web.Models;

namespace web.Services.GetDataService;

public class GetDataService{

    public static int NumOfStations = 1000;
    public static WeatherStation[] ws = new WeatherStation[NumOfStations];
    public static WeatherStation[] getWeatherStationData(){
        using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
        {
        string query = "select m.name, m.date, m.temp, m.hum, p.name, oblacnost, m.date, p.latitude, p.longitude, p.altitude from [dbo].[meritev] m inner join (select name, max(date) as MaxDate from [dbo].[meritev] group by name) tm on m.name = tm.name and m.date = tm.MaxDate inner join [dbo].[postaja] p on p.meteosiID = m.name order by m.name asc;";
        using(SqlCommand command = new SqlCommand(query, cn))
        {
            cn.Open();
            //moj dodatek :hehe:
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                int st = 0;
                while (reader.Read())
                {
                    
                    ws[st] = new WeatherStation();
                    ws[st].MeteoId = reader.GetString(0);
                    ws[st].Time = reader.GetDateTime(1);
                    if(!reader.IsDBNull(2)){
                        ws[st].Temp = reader.GetFloat(2);
                    }
                    if(!reader.IsDBNull(3)){
                        ws[st].Hum = reader.GetFloat(3);
                    }
                    ws[st].Name = reader.GetString(4);
                    if(reader.GetString(5).Equals("")){
                        ws[st].Img = "";
                    }
                    else{
                        ws[st].Img = reader.GetString(5);
                    }
                    ws[st].Time = reader.GetDateTime(6);
                    ws[st].Lat = reader.GetFloat(7);
                    ws[st].Long = reader.GetFloat(8);
                    ws[st].Alt = reader.GetFloat(9);
                    //Console.WriteLine("{0}", ws[st].Img);
                    st++;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

        }
        cn.Close();
        }
    return ws;
    }
}