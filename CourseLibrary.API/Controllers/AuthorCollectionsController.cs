﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authorcollections")]
    [ApiController]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        //public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollection)
        //{
        //    var authorEntities = mapper.Map<IEnumerable<Author>>(authorCollection);
        //    foreach (var author in authorEntities)
        //    {
        //        courseLibraryRepository.AddAuthor(author);
        //    }

        //    courseLibraryRepository.Save();
        //    return Ok();

        //}

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public ActionResult GetAuthorCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (null == ids)
            {
                return BadRequest();
            }
            var authorEntities = courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorsToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(
            IEnumerable<AuthorForCreationDto> authorCollection)
        {
            var authorEntities = mapper.Map<IEnumerable<Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                courseLibraryRepository.AddAuthor(author);
            }

            courseLibraryRepository.Save();

            var authorCollectionToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute(
                "GetAuthorCollection",
                new { ids = idsAsString},
                authorCollectionToReturn
            );
        }
    }
}
