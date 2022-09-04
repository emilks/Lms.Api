using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ModulesController(LmsApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            //_context = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModule()
        {
            var modules = await uow.ModuleRepository.GetAllModules();
            var moduleDto = mapper.Map<IEnumerable<Module>>(modules);

            return Ok(moduleDto);
            //return Ok(await uow.ModuleRepository.GetAllModules());
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var @module = await uow.ModuleRepository.FindAsync(id);

            if (@module == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Module>(@module));
            //return @module;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleDto @module)
        {
            //TryUpdateModelAsync()
            var update = await uow.ModuleRepository.GetModule(id);

            if (update is null) return NotFound();

            update.StartDate = module.StartDate;
            update.Title = module.Title;

            //update = @module;

            //mapper.Map(dto, codeevent);

            try
            {
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();

            //return Ok(mapper.Map<CodeEventDto>(codeevent));
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModuleDto moduledto)
        {
            var module = mapper.Map<Module>(moduledto);
            uow.ModuleRepository.Add(module);
            await uow.CompleteAsync();

            return CreatedAtAction("GetModule", new { id = module.Id }, module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var @module = await uow.ModuleRepository.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            uow.ModuleRepository.Remove(@module);
            await uow.CompleteAsync();

            return Ok();
        }

        private async Task<bool?> ModuleExistsAsync(int id)
        {
            return await uow.ModuleRepository.AnyAsync(id);
        }

        [HttpPatch("{moduleId}")]
        public async Task<ActionResult<ModuleDto>> PatchCourse(int moduleId, JsonPatchDocument<ModuleDto> patchDocument)
        {
            var module = await uow.ModuleRepository.GetModule(moduleId);

            if (module == null) return NotFound();

            var moduledto = mapper.Map<ModuleDto>(module);

            patchDocument.ApplyTo(moduledto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(moduledto, module);

            await uow.CompleteAsync();

            return Ok(mapper.Map<ModuleDto>(module));
        }
    }
}
