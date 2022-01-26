using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using C3_schoolvoetbal_goksysteem.Model;
using Newtonsoft.Json;

namespace C3_schoolvoetbal_goksysteem.ViewModel
{

    public class Match
    {
        public int id { get; set; }
        public int round_num { get; set; }
        public int team1_id { get; set; }
        public int team2_id { get; set; }
        public int team1_score { get; set; }
        public int team2_score { get; set; }
        public int field_id { get; set; }
        public int referee_id { get; set; }
        public DateTime datetime { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public int creator_id { get; set; }
    }
    public class MainPageDeluxeViewModel
    {
        public string Row1Team1 { get; set; }
        public string Row1Team2 { get; set; }
        public string Row1Time { get; set; }
        public string Row2Team1 { get; set; }
        public string Row2Team2 { get; set; }
        public string Row2Time { get; set; }
        public string Row3Team1 { get; set; }
        public string Row3Team2 { get; set; }
        public string Row3Time { get; set; }
        public string Row4Team1 { get; set; }
        public string Row4Team2 { get; set; }
        public string Row4Time { get; set; }
        public string Row5Team1 { get; set; }
        public string Row5Team2 { get; set; }
        public string Row5Time { get; set; }
        public string Username { get; set; }
        public MainPageDeluxeViewModel()
        {
            var json2 = new WebClient().DownloadString("http://127.0.0.1:8000/api/teams");
            var json = new WebClient().DownloadString("http://127.0.0.1:8000/api/matches");
            Username = "U bent ingelogd als: " + User.Username;
            Debug.WriteLine(json);
            List<Match> matches = JsonConvert.DeserializeObject<List<Match>>(json);
            List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(json2);
            //Excuse my ugly code, don't care
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.WriteLine(i);
                if(i == 0)
                {
                    Row1Team1 = teams.Find(x => x.id == matches[i].team1_id).name;
                    Row1Team2 = teams.Find(x => x.id == matches[i].team2_id).name;
                    Row1Time = matches[i].datetime.ToString();
                }
                if (i == 1)
                {
                    Row2Team1 = teams.Find(x => x.id == matches[i].team1_id).name;
                    Row2Team2 = teams.Find(x => x.id == matches[i].team2_id).name;
                    Row2Time = matches[i].datetime.ToString();
                }
                if (i == 2)
                {
                    Row3Team1 = teams.Find(x => x.id == matches[i].team1_id).name;
                    Row3Team2 = teams.Find(x => x.id == matches[i].team2_id).name;
                    Row3Time = matches[i].datetime.ToString();
                }
                if (i == 3)
                {
                    Row4Team1 = teams.Find(x => x.id == matches[i].team1_id).name;
                    Row4Team2 = teams.Find(x => x.id == matches[i].team2_id).name;
                    Row4Time = matches[i].datetime.ToString();
                }
                if (i == 4)
                {
                    Row5Team1 = teams.Find(x => x.id == matches[i].team1_id).name;
                    Row5Team2 = teams.Find(x => x.id == matches[i].team2_id).name;
                    Row5Time = matches[i].datetime.ToString();
                }
                if(i == 5)
                {
                    break;
                }
            }
        }
    }
}
