using CrudAPIWithRepositoryPattern.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Models;
using CrudAPIWithRepositoryPattern.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentCourseRepository _studentCourseRepository;

        public StudentsController(//StudentAndCoursesContext context,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IUnitOfWork unitOfWork,
            IStudentCourseRepository studentCourseRepository


            )
        {
            //  _context = context;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _studentCourseRepository = studentCourseRepository;
        }

        [Route("Students")]
        [HttpGet]
        public IActionResult Get()
        {
            var result = _studentRepository.Get();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentViewModel studentViewModel)
        {
            //if (ModelState.IsValid)
            //{
            var student = new Student()
            {
                DateOfBirth = studentViewModel.DateOfBirth,
                Email = studentViewModel.Email,
                Name = studentViewModel.Name
            };

            // _context.Add(student);
            // await _context.SaveChangesAsync();
            await _studentRepository.Add(student);
            await _unitOfWork.SaveChangesAsync();

            var studentId = student.ID;
            var courses = studentViewModel.Courses.Where(x => x.IsChecked);
            foreach (var course in courses)
            {
                var studentCourse = new StudentCourse()
                {
                    StudentId = studentId,
                    CourseId = course.Id
                };
                // _context.StudentCourses.Add(studentCourse);
                _studentCourseRepository.Add(studentCourse);
                //  await _context.SaveChangesAsync();
                _unitOfWork.SaveChanges();
            }

            return Ok(studentViewModel);
            // return RedirectToAction(nameof(Index));
            //}
            //return View(studentViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int? id,bool loadCourses=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentAsQueryable(id)
                .Select(r => new StudentViewModel()
                {
                    ID = r.ID,
                    Email = r.Email,
                    DateOfBirth = r.DateOfBirth,
                    Name = r.Name
                }).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }
            var allCourses = _courseRepository.GetActiveCourse().ToList();
            var selectedCourses = _studentCourseRepository.GetStudentCoursesByStudentId(student.ID).ToList();
            //if (loadCourses)
            //{
                var checkBoxList = new List<CheckBoxViewModel>();
                foreach (var item in allCourses)
                {
                    //item
                    var checkboxItem = new CheckBoxViewModel()
                    {
                        Id = item.ID,
                        IsChecked = selectedCourses.Any(x => x.CourseId == item.ID),
                        Title = item.Title
                    };
                    checkBoxList.Add(checkboxItem);
                }
                student.Courses = checkBoxList;
            //}
            //else
            //{
            //    student.StudentCourses = selectedCourses;
            //}
            
            return Ok(student);
        }


       
        [HttpPut("{id}")]
        //[Route("SaveEdit")]
        public async Task<IActionResult> Put(int id, [FromBody] StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.ID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var student = new Student()
                {
                    ID = studentViewModel.ID,
                    Email = studentViewModel.Email,
                    DateOfBirth = studentViewModel.DateOfBirth,
                    Name = studentViewModel.Name
                };

                // _context.StudentCourses.RemoveRange(_context.StudentCourses.Where(x => x.StudentId == studentViewModel.ID));
                _studentCourseRepository.RemoveRange(studentViewModel.ID);
                //  _context.Update(student);
                _studentRepository.Update(student);

                //await _context.SaveChangesAsync();
                await _unitOfWork.SaveChangesAsync();

                var courses = studentViewModel.Courses.Where(x => x.IsChecked);
                foreach (var course in courses)
                {
                    var studentCourse = new StudentCourse()
                    {
                        StudentId = student.ID,
                        CourseId = course.Id
                    };
                    //_context.StudentCourses.Add(studentCourse);
                    _studentCourseRepository.Add(studentCourse);
                    // await _context.SaveChangesAsync();
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_studentRepository.StudentExists(studentViewModel.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //}
            return Ok(studentViewModel);
        }
        //[HttpDelete]
        [Route("delete/student/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetStudent(id);
            // _context.Students.Remove(student);
            _studentRepository.RemoveStudent(student);

            //   _context.StudentCourses.RemoveRange(_context.StudentCourses.Where(x => x.StudentId == id));
            _studentCourseRepository.RemoveRange(id);
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return Ok(true);
        }
        //private bool StudentExists(int id)
        //{
        //    return _studentRepository.StudentExists(id);
        //}
    }
}
