using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ValidateToken.Model;
using ValidateToken.Repository;

namespace ValidateToken.Service
{
    public class ValidateTokenService : IValidateTokenService
    {
        private readonly IValidateTokenRepository _validateTokenRepository;

        public ValidateTokenService(IValidateTokenRepository validateTokenRepository)
        {
            _validateTokenRepository = validateTokenRepository;
        }

        public async Task GetAllAsync()
        {
            var integrationvariablesStatus = await _validateTokenRepository.GetAllAsync();
            var resultList = new List<IntegrationvariablesStatus>();

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://bling.com.br");

            for (int i = 0; i < integrationvariablesStatus.Count; i++)
            {
                var variable = integrationvariablesStatus[i];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", variable.Value);

                var response = await httpClient.GetAsync("/Api/v3/nfe/98");

                if (response.IsSuccessStatusCode)
                {
                    variable.Status = "Valido";
                }
                else
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var errorDetails = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);

                    if (errorDetails?.Error?.Type == "RESOURCE_NOT_FOUND")
                    {
                        variable.Status = "Valido";
                    }
                    else if (errorDetails?.Error?.Type == "invalid_token")
                    {
                        variable.Status = "Invalido";
                    }
                    else if (errorDetails?.Error?.Type == "TOO_MANY_REQUESTS")
                    {
                        variable.Status = "Muitas resquisiçoes";
                    }
                    else
                    {
                        variable.Status = "Invalido";
                    }
                }

                resultList.Add(variable);
            }

            GenerateCsvReport(resultList);
        }

        public void GenerateCsvReport(List<IntegrationvariablesStatus> integrationVariables)
        {
            string directoryPath = @"C:\TokensValidos";
            var filePath = Path.Combine(directoryPath, "StatusToken.csv");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<IntegrationvariablesStatus>();
                    csvWriter.NextRecord();

                    foreach (var variable in integrationVariables)
                    {
                        csvWriter.WriteRecord(variable);  // Alteração aqui
                        csvWriter.NextRecord();
                    }
                }
            }

            Console.WriteLine($"Foi gerado uma palinha em CSV no caminho: {filePath}");
        }
    }
}