using DevChallenge.Import.OFX.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Api.Controllers
{
    [Route("[controller]")]
    public class ImportarArquivoController : ControllerBase
    {
        const String folderName = "arquivoOFX";
        readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        private readonly IImportarArquivoAppService _importarArquivoAppService;
        public ImportarArquivoController(IImportarArquivoAppService importarArquivoAppService)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            _importarArquivoAppService = importarArquivoAppService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(
                    [FromForm(Name = "arquivoOFX")]IFormFile arquivoOFX)
        {
            var pathCompleto = "";

            using (var fileContentStream = new MemoryStream())
            {
                await arquivoOFX.CopyToAsync(fileContentStream);
                pathCompleto = Path.Combine(folderPath, arquivoOFX.FileName);
                await System.IO.File.WriteAllBytesAsync(pathCompleto, fileContentStream.ToArray());
            }
            var retorno = _importarArquivoAppService.ExecutarLeituraAsync(pathCompleto);
            return Ok(retorno);
        }
    }
}