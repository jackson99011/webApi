﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;
        private readonly ILogger<CoursesController> _logger;
        public CoursesController(ContosouniversityContext context,
            ILogger<CoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: api/Courses
        [HttpGet(Name = nameof(GetCourseAll))]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourseAll()
        {
            List<Course> courses = await _context.Course.AsNoTracking().ToListAsync();

            using (_logger.BeginScope("GetCourseAll"))
            {
                _logger.LogInformation("GetCourseAll: {CourseCount}", courses.Count);
            }

            return courses;
        }

        // GET: api/Courses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        //{
        //    return await _context.Course.ToListAsync();
        //}

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
