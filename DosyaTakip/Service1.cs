using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DosyaTakip
{
    public partial class DosyaTakipService : ServiceBase
    {
        public DosyaTakipService()
        {
            InitializeComponent();
        }
        DosyaTakipEntities context = new DosyaTakipEntities();
        List<EylemTurleri> turler = new List<EylemTurleri>();
        protected override void OnStart(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Users\Emir Hazir\Desktop\Dotnet","*.txt");
            watcher.Changed += Watcher_Changed;
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Renamed += Watcher_Renamed;

            turler = context.EylemTurleri.ToList();
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            DosyaOlaylari olay = new DosyaOlaylari();
            olay.EylemTurID = turler.SingleOrDefault(x => x.Adi == "Adlandırma").Id;
            olay.OlaySaati = DateTime.Now;
            olay.Yol = e.FullPath;
            context.DosyaOlaylari.Add(olay);
            context.SaveChanges();
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            DosyaOlaylari olay = new DosyaOlaylari();
            olay.EylemTurID = turler.SingleOrDefault(x => x.Adi == "Oluşturma").Id;
            olay.OlaySaati = DateTime.Now;
            olay.Yol = e.FullPath;
            context.DosyaOlaylari.Add(olay);
            context.SaveChanges();
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            DosyaOlaylari olay = new DosyaOlaylari();
            olay.EylemTurID = turler.SingleOrDefault(x => x.Adi == "Silme").Id;
            olay.OlaySaati = DateTime.Now;
            olay.Yol = e.FullPath;
            context.DosyaOlaylari.Add(olay);
            context.SaveChanges();
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            DosyaOlaylari olay = new DosyaOlaylari();
            olay.EylemTurID = turler.SingleOrDefault(x => x.Adi == "Değiştirme").Id;
            olay.OlaySaati = DateTime.Now;
            olay.Yol = e.FullPath;
            context.DosyaOlaylari.Add(olay);
            context.SaveChanges();
        }


        protected override void OnStop()
        {
        }
    }
}
