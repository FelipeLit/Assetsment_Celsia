using assetsment_Celsia.Data;
using assetsment_Celsia.Interfaces;
using assetsment_Celsia.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace assetsment_Celsia.Services
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly CelsiaContext _context;
        private readonly ILogger<ExcelRepository> _logger;

        public ExcelRepository(CelsiaContext context, ILogger<ExcelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ImportDataFromExcelAsync(IFormFile file)
        {
            var datos = LeerDatosExcel(file);
            await GuardarDatosEnBaseDeDatos(datos);
        }

        private List<DatosIntermedios> LeerDatosExcel(IFormFile file)
        {
            var listaDatos = new List<DatosIntermedios>();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var datos = new DatosIntermedios
                    {
                        IDdelaTransacción = worksheet.Cells[row, 1].Value?.ToString(),
                        FechayHoradelaTransacción = DateTime.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out DateTime fechaTransaccion)
                                            ? fechaTransaccion
                                            : DateTime.MinValue,
                        MontodelaTransacción = worksheet.Cells[row, 3].Value?.ToString(),
                        EstadodelaTransacción = worksheet.Cells[row, 4].Value?.ToString(),
                        TipodeTransacción = worksheet.Cells[row, 5].Value?.ToString(),
                        NombredelCliente = worksheet.Cells[row, 6].Value?.ToString(),
                        NúmerodeIdentificación = worksheet.Cells[row, 7].Value?.ToString(),
                        Dirección = worksheet.Cells[row, 8].Value?.ToString(),
                        Teléfono = worksheet.Cells[row, 9].Value?.ToString(),
                        CorreoElectrónico = worksheet.Cells[row, 10].Value?.ToString(),
                        PlataformaUtilizada = worksheet.Cells[row, 11].Value?.ToString(),
                        NúmerodeFactura = worksheet.Cells[row, 12].Value?.ToString(),
                        PeriododeFacturación = worksheet.Cells[row, 13].Value?.ToString(),
                        MontoFacturado = worksheet.Cells[row, 14].Value?.ToString(),
                        MontoPagado = worksheet.Cells[row, 15].Value?.ToString(),
                    };

                    listaDatos.Add(datos);
                }
            }

            return listaDatos;
        }

        private async Task GuardarDatosEnBaseDeDatos(List<DatosIntermedios> datos)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Diccionarios para almacenar las entidades únicas
                    var plataformasDict = new Dictionary<string, Platform>();
                    var clientesDict = new Dictionary<string, Client>();

                    foreach (var dato in datos)
                    {
                        // Obtener o crear plataforma
                        if (!plataformasDict.ContainsKey(dato.PlataformaUtilizada))
                        {
                            var plataforma = await _context.Platforms
                                .FirstOrDefaultAsync(p => p.Name == dato.PlataformaUtilizada);
                            if (plataforma == null)
                            {
                                plataforma = new Platform
                                {
                                    Name = dato.PlataformaUtilizada
                                };
                                _context.Platforms.Add(plataforma);
                                await _context.SaveChangesAsync();
                            }
                            plataformasDict[dato.PlataformaUtilizada] = plataforma;
                        }

                        // Obtener o crear cliente
                        if (!clientesDict.ContainsKey(dato.CorreoElectrónico))
                        {
                            var cliente = await _context.Clients
                                .FirstOrDefaultAsync(c => c.Email == dato.CorreoElectrónico);
                            if (cliente == null)
                            {
                                cliente = new Client
                                {
                                    Name = dato.NombredelCliente,
                                    Email = dato.CorreoElectrónico,
                                    Phone = dato.Teléfono,
                                    Address = dato.Dirección,
                                    Status = "Active", // Asignar el estado "Active"
                                    RoleId = 2 // Asignar el RoleId 2
                                };
                                _context.Clients.Add(cliente);
                                await _context.SaveChangesAsync();
                            }
                            clientesDict[dato.CorreoElectrónico] = cliente;
                        }

                        // Obtener o crear transacción
                        var clienteActual = clientesDict[dato.CorreoElectrónico];
                        var plataformaActual = plataformasDict[dato.PlataformaUtilizada];

                        // Extraer el número del ID de la transacción (por ejemplo: TXN001 -> 1)
                        var transactionNumber = ExtractTransactionNumber(dato.IDdelaTransacción);

                        var transaccion = new Transaction
                        {
                            TransactionDate = dato.FechayHoradelaTransacción,
                            Amount = float.Parse(dato.MontodelaTransacción),
                            Status = dato.EstadodelaTransacción,
                            Type = dato.TipodeTransacción,
                            ClientId = clienteActual.Id,
                            PlatformId = plataformaActual.Id,
                            Acronym = "TXN",
                            Id = transactionNumber
                        };

                        _context.Transactions.Add(transaccion);
                        await _context.SaveChangesAsync();

                        // Crear factura
                        if (!string.IsNullOrEmpty(dato.NúmerodeFactura))
                        {
                            var factura = new Invoice
                            {
                                Period = dato.PeriododeFacturación,
                                BilledAmount = float.Parse(dato.MontoFacturado),
                                PaidAmount = float.Parse(dato.MontoPagado),
                                ClientId = clienteActual.Id,
                                TransactionId = transaccion.Id,
                                InvoiceNumber = dato.NúmerodeFactura
                            };

                            _context.Invoices.Add(factura);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    // Registro detallado del error
                    var errorDetails = $"Error al guardar datos: {ex.Message}\nDetalles del error: {ex.InnerException?.Message}";

                    // Registrar detalles adicionales si es posible
                    foreach (var dato in datos)
                    {
                        _logger.LogError($"Datos actuales - Plataforma: {dato.PlataformaUtilizada}, Cliente: {dato.CorreoElectrónico}, Transacción: {dato.IDdelaTransacción}");
                    }

                    _logger.LogError(errorDetails);
                    throw;
                }
            }
        }

        // Método para extraer el número de la transacción
        private int ExtractTransactionNumber(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId) || transactionId.Length < 4)
            {
                throw new ArgumentException("ID de transacción no válido");
            }

            // Asume que los primeros 3 caracteres son 'TXN'
            var numberPart = transactionId.Substring(3);
            return int.Parse(numberPart);
        }


    }
}