using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MarkShame
{
    public class OverallDataGridViewModel : ObservableObject
    {
        #region Fields
        private ObservableCollection<Encounter> _encounters;
        #endregion
        #region Properties

        public ObservableCollection<Encounter> Encounters
        {
            get { return _encounters; }
            set
            {
                _encounters = value;
                OnPropertyChanged(nameof(Encounters));
            }
        }
        #endregion
        #region Constructor

        public OverallDataGridViewModel()
        {
            Encounters = new ObservableCollection<Encounter>();

        }
        #endregion

        #region Methods

        public void UpdateEncounters(IList<Encounter> encounters)
        {

            Encounters.Clear();
            foreach (var entry in encounters)
            {
                if (entry.SpellCastSuccessLines.Any())
                    Encounters.Add(entry);
            }

        }
        #endregion
    }
}
