using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(CourseLibraryRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesForAuthorFromRepo = courseLibraryRepository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromRepo));
        }

        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = courseLibraryRepository.GetCourse(authorId, courseId);
            if (null == courseForAuthorFromRepo)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseDto>(courseForAuthorFromRepo));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(
            Guid authorId, CourseForCreationDto course)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = mapper.Map<Course>(course);
            courseLibraryRepository.AddCourse(authorId ,courseEntity);
            courseLibraryRepository.Save();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute(
                "GetCourseForAuthor",
                new { authorId = authorId, courseId = courseToReturn.Id }, 
                courseToReturn
            );
        }
    }
}
