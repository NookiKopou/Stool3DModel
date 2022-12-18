using System.Diagnostics;
using StoolModel;
using Stool.Wrapper;
using System.IO;
using Microsoft.VisualBasic.Devices;

namespace Stool.StressTest
{
    /// <summary>
    /// Класс для нагрузочного тестирования
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Основной метод класса для запуска нагрузочного тестирования
        /// </summary>
        private static void Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var stoolBuilder = new StoolBuilder();
            var stoolParameters = new StoolParameters(350, 20, 40,
                400, 210, 55);
            var streamWriter = new StreamWriter($"StressTest.txt", true);
            var modelCounter = 0;
            var computerInfo = new ComputerInfo();
            ulong usedMemory = 0;
            while (usedMemory * 0.96 <= computerInfo.TotalPhysicalMemory)
            {
                stoolBuilder.Build(stoolParameters);
                usedMemory = (computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory);
                streamWriter.WriteLine(
                    $"{++modelCounter}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}");
                streamWriter.Flush();
            }
            stopWatch.Stop();
            streamWriter.WriteLine("END");
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}