using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using S7.Net;

namespace Bormech.Plc;

    public class PlcService 
    {
        private readonly string _plcIpAddress;
        public OutGoPlc PlcData;
        private S7.Net.Plc? _plc;
        private System.Timers.Timer? _timer;
        // Definicja delegata dla eventu
        private readonly ILogger<PlcService> _logger;
        public delegate void PlcDataChangedEventHandler(OutGoPlc plcData);

        private readonly IHubContext<PlcHub> _hubContext;
        
        // Declarator event
        public event PlcDataChangedEventHandler? OnPlcDataChanged;
        public event Action<OutGoPlc> DataChanged;
        public delegate void PlcScheduleChangedEventHandler(PlcSchedule plcSchedule);
        public event PlcScheduleChangedEventHandler? OnPlcScheduleChanged;
        public PlcService(OutGoPlc plcData, ILogger<PlcService> logger, IHubContext<PlcHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _plcIpAddress = "10.1.1.243";
            PlcData = plcData;
            StartTimer();
        }

        public Task StartTimer()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += async (sender, e) =>
            {
                await ReadDataAsync();
            };
            _timer.AutoReset = true;
            _timer.Enabled = true;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync()
        {
            if (_plc == null)
            {
                _plc = new S7.Net.Plc(CpuType.S71200, _plcIpAddress, 0, 1);
                await _plc.OpenAsync();
                await Task.Delay(1000); // Dodaj opóźnienie, aby upewnić się, że PLC jest gotowy
                var test = await ReadScheduleTaskAsync();
                OnPlcScheduleChanged?.Invoke(test);
                // Wywołanie eventu
                _logger.LogInformation($"schedule pon {test.Monday}");
            }
        }
        public async Task<OutGoPlc?> ReadDataAsync()
        {
            try
            {
                await EnsureConnectedAsync();
                await Task.Run(() => _plc?.ReadClass(PlcData, 9));
                var test = await ReadScheduleTaskAsync();
                // Wywołanie eventu
                _logger.LogInformation($"schedule pon {PlcData.Wanna1Temperature}");
                await _hubContext.Clients.All.SendAsync("PlcScheduleChanged", PlcData);
                // OnPlcDataChanged!.Invoke(PlcData);
                // if (PlcData != null)
                // {
                //     _logger.LogInformation($"PlcDataChanged event received 1-> {PlcData.Wanna1Temperature}");
                //     OnDataChanged(PlcData);
                // }
                // else
                // {
                //     _logger.LogWarning("PlcData is null");
                // }
                return PlcData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas odczytu danych z PLC: {ex.Message}");
                return null;
            }
        }

        public async Task<PlcSchedule> ReadScheduleTaskAsync()
        {
            if (!_plc.IsConnected) return new PlcSchedule();
            var plcData = new PlcSchedule();
            await _plc.ReadClassAsync(plcData, 9,52);
            return plcData;
        }
        public async Task<float> ReadTemperatureAsync(string address)
        {
            await EnsureConnectedAsync();
            var value = await Task.Run(() => _plc?.Read(address));
            // OutGo_PLC plcData = new OutGo_PLC();
            // await Task.Run(()=> plc?.ReadClass(plcData, 9));

            await Task.Run(() => _plc?.ReadClass(PlcData, 9));
            
            Console.WriteLine($"dsfsdf -> {PlcData.Wanna1Temperature}");
            Console.WriteLine($"dsfsdf -> {PlcData.Wanna2Temperature}");
            Console.WriteLine($"wanna1 set mqtt -> {PlcData?.Status.Operation.TempFreeze1}");
            // OnPlcDataChanged?.Invoke(_plcData);

            return value != null ? Convert.ToSingle(value) / 10f : 0f;
            // return 0f;
        }

        public async Task<bool> ReadBitAsync(string address)
        {
            await EnsureConnectedAsync();
            var value = await Task.Run(() => _plc?.Read(address));
            return value != null && (bool)value;
        }

        public async Task WriteBitAsync(string address, bool value)
        {
            await EnsureConnectedAsync();
            await Task.Run(() => _plc?.Write(address, value));
        }

        private async Task EnsureConnectedAsync()
        {
            if (_plc == null || !_plc.IsConnected)
            {
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Błąd podczas łączenia z PLC: {ex.Message}");
                }
            }
        }
        public bool IsConnected()
        {
            return _plc?.IsConnected ?? false;
        }

        public async Task WriteSchedule(PlcSchedule schedule)
        {
            await _plc?.WriteClassAsync(schedule, 9, 52)!;            
        }


        protected virtual void OnDataChanged(OutGoPlc obj)
        {
            DataChanged?.Invoke(obj);
        }
    }
