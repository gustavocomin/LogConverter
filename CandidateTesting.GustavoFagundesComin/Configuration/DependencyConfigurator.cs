using CandidateTesting.GustavoFagundesComin.Application.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Service.Reader;
using CandidateTesting.GustavoFagundesComin.Service.Writer;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateTesting.GustavoFagundesComin.Application.Configuration
{
    public class DependencyConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogReader, LogReader>();
            services.AddSingleton<ILogWriter, LogWriter>();
            services.AddSingleton<ILogConverter, LogConverter>();
        }
    }
}
