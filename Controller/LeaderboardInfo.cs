using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Controller
{
    public class LeaderboardInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static string leaderboard;
        public string LeaderBoard { get => leaderboard;
            set
            {
                leaderboard = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LeaderBoard"));
                }
            }
        }


        private static string _winning;
        public string Winning
        {
            get => _winning;
            set
            {
                _winning = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Winning"));
                }
            }
        }

        public void refresh()
        {
            
            //var data = from particpants in Data.Currentrace.participants
            //    join winner in winners on particpants.Name equals ; 
            string s = "current match: \n";

            var currentMatch = from partic in Data.Currentrace.participants orderby partic.Points descending 
                select new
                {
                    partic.Name, partic.Points
                };
            
            currentMatch.ToList().ForEach(I => s += $"{I.Name} : {I.Points} \n");
            s += "past races: \n";
            foreach (var par in Data.MyProperty.winners)
            {
                s += "track: "+ par.Key +"\n";
                int i = 1;
                par.Value.ForEach(I =>
                {
                    s += $"{I.Name} : finish place {i} \n";
                    i++;


                });
            }
            this.LeaderBoard = s;
            
            String winners = "";
            Dictionary<string, int> winnersPoints = new Dictionary<string, int>();
            foreach (var par in Data.MyProperty.winners.Values)
            {
                int placePoints = 3;
                par.Take(3).ToList().ForEach(i =>
                {

                    if (!winnersPoints.TryAdd(i.Name,placePoints))
                    {
                        winnersPoints[i.Name] = winnersPoints[i.Name] + placePoints;
                    }

                    placePoints--;

                });
                
            }

            foreach (var winnersFull in winnersPoints)
            {
                winners += $"{winnersFull.Key} - {winnersFull.Value} \n";
            }


            Winning = winners;
            


        }
    }
}