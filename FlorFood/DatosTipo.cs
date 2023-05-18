using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlorFood
{
    public class DatosTipo
    {
        public ObservableCollection<Tipo> Tipos { get; set; }
        public DatosTipo() 
        {
            this.Tipos = new ObservableCollection<Tipo>();
            this.Tipos.Add(new Tipo() { Name = "Cliente", ID = 0 });
            this.Tipos.Add(new Tipo() { Name = "Empresa", ID = 1 });
        }

    }
    public class Tipo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
