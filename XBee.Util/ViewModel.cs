﻿using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XBee.Util.Annotations;

namespace XBee.Util
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly StringBuilder _log = new StringBuilder();

        public ViewModel()
        {
            DiscoverCommand = new RelayCommand(async () => await Discover());
        }

        public ICommand DiscoverCommand { get; private set; }

        public string Log { get { return _log.ToString(); } }

        private async Task Discover()
        {
            var ports = SerialPort.GetPortNames();
            var discoverMessage = string.Format("Checking serial ports: {0}", string.Join(", ", ports));
            AddLog(discoverMessage);

            var controller = new XBeeController();

            foreach (var port in ports)
            {
                try
                {
                    await controller.OpenAsync(port, 9600);
                    AddLog(string.Format("Opened {0}", port));
                }
                catch (Exception e)
                {
                    AddLog(string.Format("Failed to open {0} with {1}", port, e.Message));
                }
                finally
                {
                    controller.Close();
                }
            }

            AddLog("Done.");
        }

        private void AddLog(string message)
        {
            _log.AppendLine(message);
            OnPropertyChanged("Log");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
