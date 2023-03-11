using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace MarkShame
{
    public class OverallBarChartViewModel : ObservableObject
    {
        private List<SpellCastSuccessLine> _spellCastSuccessLines;

        public OverallBarChartViewModel()
        {
        }

        public SeriesCollection ChartSeries
        {
            get
            {
                SeriesCollection series = null;
                if (_spellCastSuccessLines != null)
                {
                    series = new SeriesCollection();
                    var casterGroups = _spellCastSuccessLines
                        .GroupBy(line => line.CasterInfo).Where(x=>x.Key != "\"Dathea")
                        .Select(group => new ColumnSeries ()
                        {
                            Title = group.Key, Values = new ChartValues<int>{group.Count()}, DataLabels = true
                        });

                    foreach (var group in casterGroups)
                    {

                        series.Add(group);
                    }
                }

                return series;
            }
        }

        public List<string> CasterNames
        {
            get
            {
                return _spellCastSuccessLines?
                    .Select(line => line.CasterInfo)
                    .Distinct()
                    .ToList();
            }
        }

        public void UpdateChart(List<SpellCastSuccessLine> spellCastSuccessLines)
        {
            _spellCastSuccessLines = spellCastSuccessLines;
            OnPropertyChanged(nameof(CasterNames));
            OnPropertyChanged(nameof(ChartSeries));
        }
    }
}
