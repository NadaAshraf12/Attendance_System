using CleanArch.App.Features.Departments.Commands.Assign_User_to_Department;
using CleanArch.App.Features.Departments.Commands.CreateDepartment;
using CleanArch.App.Features.Departments.Commands.DeleteDepartment;
using CleanArch.App.Features.Departments.Commands.UpdateDepartment;
using CleanArch.App.Features.Departments.Queries.GetDepartmentByCode;
using CleanArch.App.Features.Departments.Queries.GetDepartmentById;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentRepository _deptRepo;
        public DepartmentController(IMediator mediator, IDepartmentRepository deptRepo)
        {
            _deptRepo = deptRepo;
            _mediator = mediator;
        }

        [HttpPost("departments")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand cmd)
        {
            var res = await _mediator.Send(cmd);
            if (res.IsError) return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var list = await _deptRepo.GetAllWithChildrenAsync(); 

            var result = list.Select(d => MapToDepartmentDto(d)).ToList();

            return Ok(result);
        }

        
        private DepartmentDto MapToDepartmentDto(Department dept)
        {
            return new DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Code = dept.Code,
                SubDepartments = dept.SubDepartments?.Select(MapToDepartmentDto).ToList() ?? new List<DepartmentDto>()
            };
        }


        [HttpGet("departments/{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var res = await _mediator.Send(new GetDepartmentByIdQuery(id));
            if (res.IsError) return NotFound(res);
            return Ok(res);
        }

        [HttpPut("departments/{id}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentCommand cmd)
        {
            if (id != cmd.Id) return BadRequest(ResponseModel.Fail("Id mismatch"));
            var res = await _mediator.Send(cmd);
            if (res.IsError) return BadRequest(res);
            return Ok(res);
        }

        [HttpDelete("departments/{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var res = await _mediator.Send(new DeleteDepartmentCommand(id));
            if (res.IsError) return BadRequest(res);
            return Ok(res);
        }

        [HttpPut("users/{userId}/department/{deptId}")]
        public async Task<IActionResult> AssignUser(Guid userId, Guid deptId)
        {
            var res = await _mediator.Send(new AssignUserToDepartmentCommand(userId, deptId));
            if (res.IsError) return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("departments/code/{code}")]
        public async Task<IActionResult> GetDepartmentByCode(string code)
            => Ok(await _mediator.Send(new GetDepartmentByCodeQuery(code)));

    }
}
