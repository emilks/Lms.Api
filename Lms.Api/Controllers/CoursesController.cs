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
    public class CoursesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CoursesController(LmsApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            //_context = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            var courses = await uow.CourseRepository.GetAllCourses();
            var courseDto = mapper.Map<IEnumerable<Course>>(courses);

            return Ok(courseDto);
            //return Ok(await uow.CourseRepository.GetAllCourses());
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await uow.CourseRepository.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Course>(course));
            //return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseDto course)
        {

            //_context.Entry(course).State = EntityState.Modified;
            var update = await uow.CourseRepository.GetCourse(id);

            if (update is null) return NotFound();

            update.StartDate = course.StartDate;
            update.Title = course.Title;

            try
            {
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseDto courseDto)
        {
            var course = mapper.Map<Course>(courseDto);
            uow.CourseRepository.Add(course);
            await uow.CompleteAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await uow.CourseRepository.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            uow.CourseRepository.Remove(course);
            await uow.CompleteAsync();

            return Ok();
        }

        private async Task<bool?> CourseExists(int id)
        {
            return await uow.CourseRepository.AnyAsync(id);
        }

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course == null) return NotFound();

            var coursedto = mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(coursedto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(coursedto, course);

            await uow.CompleteAsync();

            return Ok(mapper.Map<CourseDto>(course));
        }
    }
}
