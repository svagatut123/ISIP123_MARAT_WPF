using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WpfApp1.Models
{
    // основная конфигурация
    public class Konfig : INotifyPropertyChanged
    {
        // модель авто
        private ModelAvto _poleModel;
        public ModelAvto PoleModel
        {
            get => _poleModel;
            set { _poleModel = value; Izmenilos("PoleModel"); }
        }

        // двигатель
        private Dvigatel _poleDvig;
        public Dvigatel PoleDvig
        {
            get => _poleDvig;
            set { _poleDvig = value; Izmenilos("PoleDvig"); }
        }

        // цвет
        private Cvet _poleCvet;
        public Cvet PoleCvet
        {
            get => _poleCvet;
            set { _poleCvet = value; Izmenilos("PoleCvet"); }
        }

        // опции
        public List<Opcia> VseOpcii { get; set; } = new List<Opcia>();

        // процент первого взноса
        private decimal _procentVznos = 20;
        public decimal procentVznos
        {
            get => _procentVznos;
            set { _procentVznos = value; Izmenilos("procentVznos"); }
        }

        // срок кредита
        private int _srokMes = 24;
        public int srokMes
        {
            get => _srokMes;
            set { _srokMes = value; Izmenilos("srokMes"); }
        }

        // данные клиента
        private string _fio;
        public string fio
        {
            get => _fio;
            set { _fio = value; Izmenilos("fio"); }
        }

        private string _telefon;
        public string telefon
        {
            get => _telefon;
            set { _telefon = value; Izmenilos("telefon"); }
        }

        private string _email;
        public string email
        {
            get => _email;
            set { _email = value; Izmenilos("email"); }
        }

        // расчетные поля
        public decimal CenaModel => PoleModel?.cenaOsn ?? 0;
        public decimal CenaDvig => PoleDvig?.cenaDop ?? 0;
        public decimal CenaCvet => PoleCvet?.cenaDop ?? 0;
        public decimal CenaOpcii => VseOpcii.Where(o => o.vibrano).Sum(o => o.cena);
        public decimal CenaItog => CenaModel + CenaDvig + CenaCvet + CenaOpcii;

        public decimal SummaVznos => CenaItog * (procentVznos / 100m);
        public decimal SummaCredit => CenaItog - SummaVznos;

        public decimal PlatejVMes
        {
            get
            {
                if (SummaCredit <= 0 || srokMes == 0) return 0;

                // ставка 15% в год
                decimal stavka = 0.15m;
                decimal stavkaMes = stavka / 12;

                // расчет платежа
                double osn = (double)SummaCredit;
                double mesStavka = (double)stavkaMes;
                int srok = srokMes;

                double koef = System.Math.Pow(1 + mesStavka, srok);
                double platej = osn * mesStavka * koef / (koef - 1);

                return (decimal)platej;
            }
        }

        // уведомление об изменении
        public event PropertyChangedEventHandler PropertyChanged;
        private void Izmenilos(string nazvanie)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazvanie));
        }
    }

    // модель авто
    public class ModelAvto
    {
        public string nazvanie { get; set; }
        public decimal cenaOsn { get; set; }
    }

    // двигатель
    public class Dvigatel
    {
        public string tip { get; set; }
        public decimal cenaDop { get; set; }
    }

    // цвет
    public class Cvet
    {
        public string nazvanie { get; set; }
        public decimal cenaDop { get; set; }
        public string kodCveta { get; set; }
    }

    // опция
    public class Opcia : INotifyPropertyChanged
    {
        private bool _vibrano;
        public string nazvanie { get; set; }
        public decimal cena { get; set; }

        public bool vibrano
        {
            get => _vibrano;
            set
            {
                _vibrano = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vibrano)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    // статический класс для хранения
    public static class Dannye
    {
        public static Konfig tecushaa { get; } = new Konfig();
    }
}