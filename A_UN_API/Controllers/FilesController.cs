using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesProdAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public FilesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileReadDto>>> GetAllFiles([FromQuery] QueryStringParameters paginationParameters)
        {
            var files = await _repository.File.GetAllFilesAsync(paginationParameters);

            var metadata = new
            {
                files.TotalCount,
                files.PageSize,
                files.CurrentPage,
                files.TotalPages,
                files.HasNext,
                files.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all files from database.");

            var filesReadDto = _mapper.Map<IEnumerable<FileReadDto>>(files);

            return Ok(filesReadDto);
        }



        [HttpGet("{id}", Name = "FileById")]
        public async Task<ActionResult<FileReadDto>> GetFileById(Guid id)
        {
            var file = await _repository.File.GetFileByIdAsync(id);

            if (file == null)
            {
                _logger.LogError($"File with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned fileWriteDto with id: {id}");

                var fileReadDto = _mapper.Map<FileReadDto>(file);
                
                return Ok(fileReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<FileReadDto>> CreateFile([FromBody] FileWriteDto file)
        {
            if (file == null)
            {
                _logger.LogError("File object sent from file is null.");
                return BadRequest("File object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid fileWriteDto object sent from file.");
                return BadRequest("Invalid model object");
            }

            var fileEntity = _mapper.Map<File>(file);

            if (await _repository.File.FileExistAsync(fileEntity))
            {
                ModelState.AddModelError("", "This File exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.File.CreateFileAsync(fileEntity);
            await _repository.SaveAsync();

            var fileReadDto = _mapper.Map<FileReadDto>(fileEntity);
            return CreatedAtRoute("FileById", new { id = fileReadDto.Id }, fileReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<FileReadDto>> UpdateFile(Guid id, [FromBody] FileWriteDto fileWriteDto)
        {
            if (fileWriteDto == null)
            {
                _logger.LogError("File object sent from file is null.");
                return BadRequest("File object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid fileWriteDto object sent from file.");
                return BadRequest("Invalid model object");
            }

            var fileEntity = await _repository.File.GetFileByIdAsync(id);
            if (fileEntity == null)
            {
                _logger.LogError($"File with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(fileWriteDto, fileEntity);


            await _repository.File.UpdateFileAsync(fileEntity);
            await _repository.SaveAsync();

            var fileReadDto = _mapper.Map<FileReadDto>(fileEntity);
            return Ok(fileReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFile(Guid id)
        {
            var file = await _repository.File.GetFileByIdAsync(id);

            if (file == null)
            {
                _logger.LogError($"File with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.File.DeleteFileAsync(file);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
